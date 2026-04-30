using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolSystem.Core.DTOs.Report;
using SchoolSystem.Core.DTOs.Student;
using SchoolSystem.Web.Models.ViewModels;
using System.Security.Claims;

namespace SchoolSystem.Web.Pages.Dashboard;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<IndexModel> _logger;
    private readonly IConfiguration _configuration;

    public List<ChildDashboardViewModel> Children { get; set; } = new();

    public IndexModel(IHttpClientFactory httpClientFactory, ILogger<IndexModel> logger, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            // Read parent user ID from authenticated claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var parentUserId))
            {
                _logger.LogWarning("Unable to extract parent user ID from claims.");
                return RedirectToPage("/Auth/Login");
            }

            var apiBaseUrl = _configuration["ApiBaseUrl"] ?? "https://localhost:5001";
            var httpClient = _httpClientFactory.CreateClient();

            // Fetch the parent's children list
            var studentsResponse = await httpClient.GetAsync($"{apiBaseUrl}/api/students/parent/{parentUserId}?pageSize=100");
            if (!studentsResponse.IsSuccessStatusCode)
            {
                _logger.LogWarning($"Failed to fetch children for parent {parentUserId}: {studentsResponse.StatusCode}");
                // Return page with empty children list (graceful handling)
                return Page();
            }

            var content = await studentsResponse.Content.ReadAsStringAsync();
            var studentsData = System.Text.Json.JsonSerializer.Deserialize<dynamic>(content, 
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Handle pagination result wrapper
            var studentList = new List<StudentDto>();
            if (studentsData is not null)
            {
                try
                {
                    // Check if it's a paginated response with Items property
                    if (studentsData.GetType().GetProperty("Items") != null)
                    {
                        var itemsJson = System.Text.Json.JsonSerializer.Serialize(studentsData.GetProperty("Items"));
                        studentList = System.Text.Json.JsonSerializer.Deserialize<List<StudentDto>>(itemsJson,
                            new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<StudentDto>();
                    }
                    else
                    {
                        var listJson = System.Text.Json.JsonSerializer.Serialize(studentsData);
                        studentList = System.Text.Json.JsonSerializer.Deserialize<List<StudentDto>>(listJson,
                            new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<StudentDto>();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error parsing students response: {ex.Message}");
                }
            }

            // For each child, fetch the latest report
            foreach (var student in studentList)
            {
                var childViewModel = new ChildDashboardViewModel
                {
                    StudentId = student.Id,
                    StudentName = student.Name,
                    ClassName = student.ClassName,
                    SchoolYear = GetCurrentSchoolYear(),
                    LatestReportSummary = ""
                };

                // Try to fetch the latest available report (yearly > semester > monthly)
                var reportSummary = await FetchLatestReportSummaryAsync(httpClient, student.Id, apiBaseUrl);
                childViewModel.LatestReportSummary = reportSummary;

                Children.Add(childViewModel);
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in Dashboard OnGet: {ex.Message}");
            // Return page with empty children list to avoid crash
            return Page();
        }
    }

    private async Task<string> FetchLatestReportSummaryAsync(HttpClient httpClient, int studentId, string apiBaseUrl)
    {
        try
        {
            // Try yearly report first (most comprehensive)
            var yearlyResponse = await httpClient.GetAsync($"{apiBaseUrl}/api/yearly-reports/{studentId}");
            if (yearlyResponse.IsSuccessStatusCode)
            {
                var content = await yearlyResponse.Content.ReadAsStringAsync();
                var report = System.Text.Json.JsonSerializer.Deserialize<YearlyReportDto>(content,
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (report?.Entries.FirstOrDefault() is YearlyReportEntryDto entry && entry.TotalScore.HasValue)
                {
                    return $"Yearly Score: {entry.TotalScore:F2} | Rank: {entry.Rank}";
                }
            }

            // Try semester report
            var semesterResponse = await httpClient.GetAsync($"{apiBaseUrl}/api/semester-reports/{studentId}");
            if (semesterResponse.IsSuccessStatusCode)
            {
                var content = await semesterResponse.Content.ReadAsStringAsync();
                var report = System.Text.Json.JsonSerializer.Deserialize<SemesterReportDto>(content,
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (report?.Entries.FirstOrDefault() is SemesterReportEntryDto entry && entry.TotalScore.HasValue)
                {
                    return $"Semester {report.Semester} Score: {entry.TotalScore:F2} | Rank: {entry.Rank}";
                }
            }

            // Try monthly report
            var monthlyResponse = await httpClient.GetAsync($"{apiBaseUrl}/api/monthly-reports/{studentId}");
            if (monthlyResponse.IsSuccessStatusCode)
            {
                var content = await monthlyResponse.Content.ReadAsStringAsync();
                var report = System.Text.Json.JsonSerializer.Deserialize<MonthlyReportDto>(content,
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (report?.Entries.FirstOrDefault() is MonthlyReportEntryDto entry && entry.TotalScore.HasValue)
                {
                    return $"Month {report.Month} Score: {entry.TotalScore:F2} | Rank: {entry.Rank}";
                }
            }

            return "No reports available";
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching report summary for student {studentId}: {ex.Message}");
            return "Error loading report";
        }
    }

    private string GetCurrentSchoolYear()
    {
        var now = DateTime.UtcNow;
        // School year typically runs from June to May or August to July
        // Adjust the logic based on your school's calendar
        return now.Month >= 6 ? $"{now.Year}/{now.Year + 1}" : $"{now.Year - 1}/{now.Year}";
    }
}
