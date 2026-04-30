using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolSystem.Core.DTOs.Feedback;
using SchoolSystem.Core.DTOs.Student;
using System.Security.Claims;
using System.Text.Json;

namespace SchoolSystem.Web.Pages.Feedback;

[Authorize]
public class HistoryModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<HistoryModel> _logger;

    public List<FeedbackHistoryViewModel> FeedbackHistory { get; set; } = new();
    public string ReportType { get; set; } = string.Empty;
    public int ReportId { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public int MatchedStudentId { get; set; } // Added to construct working report links

    public HistoryModel(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<HistoryModel> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync(string reportType, int reportId)
    {
        ReportType = reportType;
        ReportId = reportId;

        if (!await VerifyOwnershipAsync(reportType, reportId))
        {
            return Forbid();
        }

        try
        {
            var apiBaseUrl = _configuration["ApiBaseUrl"] ?? "https://localhost:5001";
            var httpClient = _httpClientFactory.CreateClient();

            if (Request.Headers.TryGetValue("Cookie", out var cookieValues))
            {
                httpClient.DefaultRequestHeaders.Add("Cookie", cookieValues.ToString());
            }

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

            var apiBaseUrl = _configuration["ApiBaseUrl"] ?? "https://localhost:5001";
            var httpClient = _httpClientFactory.CreateClient();

            // 1. Get parent's students
            var studentsResponse = await httpClient.GetAsync($"{apiBaseUrl}/api/students/parent/{parentUserId}?pageSize=100");
            if (!studentsResponse.IsSuccessStatusCode) return false;

            var content = await studentsResponse.Content.ReadAsStringAsync();
            var studentsData = JsonSerializer.Deserialize<dynamic>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            var studentList = new List<StudentDto>();
            if (studentsData is not null)
            {
                if (studentsData.GetType().GetProperty("Items") != null)
                {
                    var itemsJson = JsonSerializer.Serialize(studentsData.GetProperty("Items"));
                    studentList = JsonSerializer.Deserialize<List<StudentDto>>(itemsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<StudentDto>();
                }
                else
                {
                    var listJson = JsonSerializer.Serialize(studentsData);
                    studentList = JsonSerializer.Deserialize<List<StudentDto>>(listJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<StudentDto>();
                }
            }

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
