using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolSystem.Core.DTOs;
using SchoolSystem.Core.DTOs.Feedback;
using SchoolSystem.Core.DTOs.Student;
using SchoolSystem.Web.Services;
using System.Security.Claims;
using System.Text.Json;

namespace SchoolSystem.Web.Pages.Feedback;

[Authorize]
public class HistoryModel : AuthenticatedPageModel
{
    private readonly ILogger<HistoryModel> _logger;

    public List<FeedbackHistoryViewModel> FeedbackHistory { get; set; } = new();
    public string ReportType { get; set; } = string.Empty;
    public int ReportId { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public int MatchedStudentId { get; set; } // Added to construct working report links

    public HistoryModel(ApiHttpClientFactory apiClientFactory, ILogger<HistoryModel> logger)
        : base(apiClientFactory)
    {
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync(string reportType, int reportId)
    {
        var tokenCheck = CheckToken();
        if (tokenCheck != null) return tokenCheck;
        
        ReportType = reportType;
        ReportId = reportId;

        if (!await VerifyOwnershipAsync(reportType, reportId))
        {
            return Forbid();
        }

        try
        {
            var apiBaseUrl = ApiClientFactory.GetApiBaseUrl();
            var httpClient = ApiClientFactory.CreateAuthenticatedClient();

            var response = await httpClient.GetAsync($"{apiBaseUrl}/api/feedback/report/{reportType}/{reportId}");

            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = "Failed to load feedback history.";
                return Page();
            }

            var content = await response.Content.ReadAsStringAsync();
            var feedbacks = JsonSerializer.Deserialize<List<FeedbackDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (feedbacks != null)
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                feedbacks = feedbacks
                    .Where(f => f.ParentUserId == currentUserId)
                    .ToList();

                FeedbackHistory = feedbacks.Select(f => new FeedbackHistoryViewModel
                {
                    Id = f.Id,
                    ReportType = f.ReportType.ToString(),
                    ReportId = f.MonthlyReportId ?? f.SemesterReportId ?? f.YearlyReportId ?? reportId,
                    Message = f.Message,
                    SubmittedAt = f.CreatedAt,
                    IsRead = false, // Backend does not support read receipts currently
                    ReportLink = $"/Reports/{f.ReportType}?reportId={reportId}&studentId={MatchedStudentId}"
                }).OrderByDescending(f => f.SubmittedAt).ToList();
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error loading feedback history: {ex.Message}");
            ErrorMessage = "An unexpected error occurred.";
            return Page();
        }
    }

    private async Task<bool> VerifyOwnershipAsync(string reportType, int reportId)
    {
        if (string.IsNullOrEmpty(reportType) || reportId <= 0) return false;

        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var parentUserId))
            {
                return false;
            }

            var apiBaseUrl = ApiClientFactory.GetApiBaseUrl();
            var httpClient = ApiClientFactory.CreateAuthenticatedClient();

            // 1. Get parent's students
            var studentsResponse = await httpClient.GetAsync($"{apiBaseUrl}/api/students/parent/{parentUserId}?pageSize=100");
            if (!studentsResponse.IsSuccessStatusCode) return false;

            var content = await studentsResponse.Content.ReadAsStringAsync();
            var pagedResult = JsonSerializer.Deserialize<PagedResult<StudentDto>>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
            var studentList = pagedResult?.Items ?? new List<StudentDto>();

            var parentStudentIds = studentList.Select(s => s.Id).ToHashSet();
            if (!parentStudentIds.Any()) return false;

            // 2. Get report and check if any entry belongs to parent's child
            var reportEndpoint = reportType.ToLower() switch
            {
                "monthly" => "monthly-reports",
                "semester" => "semester-reports",
                "yearly" => "yearly-reports",
                _ => null
            };

            if (reportEndpoint == null) return false;

            var reportResponse = await httpClient.GetAsync($"{apiBaseUrl}/api/{reportEndpoint}/{reportId}");
            if (!reportResponse.IsSuccessStatusCode) return false;

            var reportContent = await reportResponse.Content.ReadAsStringAsync();
            
            using var document = JsonDocument.Parse(reportContent);
            if (document.RootElement.TryGetProperty("entries", out var entriesElement) || 
                document.RootElement.TryGetProperty("Entries", out entriesElement))
            {
                foreach (var entry in entriesElement.EnumerateArray())
                {
                    if ((entry.TryGetProperty("studentId", out var studentIdElement) || 
                         entry.TryGetProperty("StudentId", out studentIdElement)) && 
                        studentIdElement.TryGetInt32(out var studentId))
                    {
                        if (parentStudentIds.Contains(studentId))
                        {
                            MatchedStudentId = studentId;
                            return true;
                        }
                    }
                }
            }
            
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error verifying ownership: {ex.Message}");
            return false;
        }
    }
}

public class FeedbackHistoryViewModel
{
    public int Id { get; set; }
    public string ReportType { get; set; } = string.Empty;
    public int ReportId { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime SubmittedAt { get; set; }
    public bool IsRead { get; set; }
    public string ReportLink { get; set; } = string.Empty;
}
