using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolSystem.Core.DTOs;
using SchoolSystem.Core.Enums;
using SchoolSystem.Web.Models;
using SchoolSystem.Web.Models.ViewModels;
using SchoolSystem.Web.Services;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace SchoolSystem.Web.Pages.Feedback;

[Authorize]
public class SubmitModel : AuthenticatedPageModel
{
    private readonly ILogger<SubmitModel> _logger;

    [BindProperty]
    public SubmitInputModel Input { get; set; } = new();

    public string ErrorMessage { get; set; } = string.Empty;

    public SubmitModel(ApiHttpClientFactory apiClientFactory, ILogger<SubmitModel> logger)
        : base(apiClientFactory, logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync(string? reportType, int reportId = 0)
    {
        var tokenCheck = CheckToken();
        if (tokenCheck != null) return tokenCheck;

        if (string.IsNullOrWhiteSpace(reportType) || reportId <= 0)
        {
            ErrorMessage = "Please select a report from the dashboard before submitting feedback.";
            return Page();
        }

        Input.ReportType = reportType;
        Input.ReportId = reportId;

        var (isOwner, _) = await VerifyOwnershipAsync(reportType, reportId);
        if (!isOwner)
        {
            return Forbid();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var tokenCheck = CheckToken();
        if (tokenCheck != null) return tokenCheck;

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var (isOwner, _) = await VerifyOwnershipAsync(Input.ReportType, Input.ReportId);
        if (!isOwner)
        {
            return Forbid();
        }

        try
        {
            var apiBaseUrl = ApiClientFactory.GetApiBaseUrl();
            var httpClient = ApiClientFactory.CreateAuthenticatedClient();

            if (!Enum.TryParse<ReportType>(Input.ReportType, true, out var parsedReportType))
            {
                ErrorMessage = "Invalid report type.";
                return Page();
            }

            var payload = new
            {
                reportType = (int)parsedReportType,
                reportId = Input.ReportId,
                message = Input.Message
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"{apiBaseUrl}/api/feedback", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Feedback/History", new { reportType = Input.ReportType, reportId = Input.ReportId });
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogWarning($"Failed to submit feedback: {response.StatusCode} - {errorContent}");
            ErrorMessage = "Failed to submit feedback. Please try again later.";
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error submitting feedback: {ex.Message}");
            ErrorMessage = "An unexpected error occurred while submitting feedback.";
            return Page();
        }
    }
}
