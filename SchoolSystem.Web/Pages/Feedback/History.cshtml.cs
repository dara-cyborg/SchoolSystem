using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolSystem.Core.DTOs.Feedback;
using SchoolSystem.Web.Models.ViewModels;
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
    public int MatchedStudentId { get; set; }

    public HistoryModel(ApiHttpClientFactory apiClientFactory, ILogger<HistoryModel> logger)
        : base(apiClientFactory, logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync(string reportType, int reportId)
    {
        var tokenCheck = CheckToken();
        if (tokenCheck != null) return tokenCheck;

        ReportType = reportType;
        ReportId = reportId;

        var (isOwner, matchedStudentId) = await VerifyOwnershipAsync(reportType, reportId);
        if (!isOwner)
        {
            return Forbid();
        }

        MatchedStudentId = matchedStudentId;

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
                    IsRead = false,
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
}
