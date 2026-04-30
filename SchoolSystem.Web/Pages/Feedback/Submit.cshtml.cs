using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolSystem.Core.DTOs.Student;
using SchoolSystem.Core.Enums;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace SchoolSystem.Web.Pages.Feedback;

[Authorize]
public class SubmitModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<SubmitModel> _logger;

    [BindProperty]
    public SubmitInputModel Input { get; set; } = new();

    public string ErrorMessage { get; set; } = string.Empty;

    public SubmitModel(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<SubmitModel> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync(string reportType, int reportId)
    {
        Input.ReportType = reportType;
        Input.ReportId = reportId;

        if (!await VerifyOwnershipAsync(reportType, reportId))
        {
            return Forbid();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (!await VerifyOwnershipAsync(Input.ReportType, Input.ReportId))
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
                    studentList = JsonSerializer.Deserialize<List<StudentDto>>(itemsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
                }
                else
                {
                    var listJson = JsonSerializer.Serialize(studentsData);
                    studentList = JsonSerializer.Deserialize<List<StudentDto>>(listJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
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

public class SubmitInputModel
{
    public string ReportType { get; set; } = string.Empty;
    public int ReportId { get; set; }
    public string Message { get; set; } = string.Empty;
}
