using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Api.Data;
using SchoolSystem.Core.DTOs;
using SchoolSystem.Core.DTOs.Student;
using SchoolSystem.Web.Models.ViewModels;
using SchoolSystem.Web.Services;
using System.Security.Claims;

namespace SchoolSystem.Web.Pages.Dashboard;

[Authorize]
public class IndexModel : AuthenticatedPageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly AppDbContext _context;

    public List<ChildDashboardViewModel> Children { get; set; } = new();

    public IndexModel(ApiHttpClientFactory apiClientFactory, ILogger<IndexModel> logger, AppDbContext context)
        : base(apiClientFactory)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var tokenCheck = CheckToken();
        if (tokenCheck != null) return tokenCheck;
        
        try
        {
            // Read parent user ID from authenticated claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var parentUserId))
            {
                _logger.LogWarning("Unable to extract parent user ID from claims.");
                return RedirectToPage("/Auth/Login");
            }

            var apiBaseUrl = ApiClientFactory.GetApiBaseUrl();
            var httpClient = ApiClientFactory.CreateAuthenticatedClient();

            // Fetch the parent's children list
            var studentsResponse = await httpClient.GetAsync($"{apiBaseUrl}/api/students/parent/{parentUserId}?pageSize=100");
            if (!studentsResponse.IsSuccessStatusCode)
            {
                _logger.LogWarning($"Failed to fetch children for parent {parentUserId}: {studentsResponse.StatusCode}");
                // Return page with empty children list (graceful handling)
                return Page();
            }

            var content = await studentsResponse.Content.ReadAsStringAsync();
            var pagedResult = System.Text.Json.JsonSerializer.Deserialize<PagedResult<StudentDto>>(
                content,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
            var studentList = pagedResult?.Items ?? new List<StudentDto>();

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

                await PopulateLatestReportSummaryAsync(childViewModel);

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

    private async Task PopulateLatestReportSummaryAsync(ChildDashboardViewModel child)
    {
        try
        {
            var latestYearly = await _context.YearlyReportEntries
                .AsNoTracking()
                .Where(e => e.StudentId == child.StudentId)
                .Select(e => new
                {
                    ReportId = e.ReportId,
                    e.TotalScore,
                    e.Rank,
                    e.Report.SchoolYear
                })
                .OrderByDescending(e => e.SchoolYear)
                .ThenByDescending(e => e.ReportId)
                .FirstOrDefaultAsync();

            child.LatestYearlyReportId = latestYearly?.ReportId;

            var latestSemester = await _context.SemesterReportEntries
                .AsNoTracking()
                .Where(e => e.StudentId == child.StudentId)
                .Select(e => new
                {
                    ReportId = e.ReportId,
                    e.TotalScore,
                    e.Rank,
                    e.Report.Semester,
                    e.Report.SchoolYear
                })
                .OrderByDescending(e => e.SchoolYear)
                .ThenByDescending(e => e.Semester)
                .ThenByDescending(e => e.ReportId)
                .FirstOrDefaultAsync();

            child.LatestSemesterReportId = latestSemester?.ReportId;

            var latestMonthly = await _context.MonthlyReportEntries
                .AsNoTracking()
                .Where(e => e.StudentId == child.StudentId)
                .Select(e => new
                {
                    ReportId = e.ReportId,
                    e.TotalScore,
                    e.Rank,
                    e.Report.Month,
                    e.Report.SchoolYear
                })
                .OrderByDescending(e => e.SchoolYear)
                .ThenByDescending(e => e.Month)
                .ThenByDescending(e => e.ReportId)
                .FirstOrDefaultAsync();

            child.LatestMonthlyReportId = latestMonthly?.ReportId;

            if (latestYearly != null)
            {
                child.LatestReportSummary = $"Yearly Score: {(latestYearly.TotalScore ?? 0):F2} | Rank: {latestYearly.Rank}";
            }
            else if (latestSemester != null)
            {
                child.LatestReportSummary = $"Semester {latestSemester.Semester} Score: {(latestSemester.TotalScore ?? 0):F2} | Rank: {latestSemester.Rank}";
            }
            else if (latestMonthly != null)
            {
                child.LatestReportSummary = $"Month {latestMonthly.Month} Score: {(latestMonthly.TotalScore ?? 0):F2} | Rank: {latestMonthly.Rank}";
            }
            else
            {
                child.LatestReportSummary = "No reports available";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching report summary for student {child.StudentId}: {ex.Message}");
            child.LatestReportSummary = "Error loading report";
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
