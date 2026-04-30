using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Api.Data;
using SchoolSystem.Core.DTOs;
using SchoolSystem.Core.DTOs.Feedback;
using SchoolSystem.Core.DTOs.Student;
using SchoolSystem.Web.Services;
using System.Security.Claims;
using System.Text.Json;

namespace SchoolSystem.Web.Pages.Feedback;

[Authorize]
public class IndexModel : AuthenticatedPageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly AppDbContext _context;

    public List<AllFeedbackViewModel> Feedbacks { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

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
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var parentUserId))
                return RedirectToPage("/Auth/Login");

            var apiBaseUrl = ApiClientFactory.GetApiBaseUrl();
            var httpClient = ApiClientFactory.CreateAuthenticatedClient();

            // Step 1: Get all children of this parent
            var studentsResponse = await httpClient.GetAsync($"{apiBaseUrl}/api/students/parent/{parentUserId}?pageSize=100");
            if (!studentsResponse.IsSuccessStatusCode)
            {
                ErrorMessage = "Failed to load student data.";
                return Page();
            }

            var studentsContent = await studentsResponse.Content.ReadAsStringAsync();
            var pagedResult = JsonSerializer.Deserialize<PagedResult<StudentDto>>(studentsContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var studentList = pagedResult?.Items ?? new List<StudentDto>();

            if (!studentList.Any())
                return Page();

            // Step 2: For each child, get ALL report IDs from the DB
            var fetchTasks = new List<Task<List<AllFeedbackViewModel>>>();

            foreach (var student in studentList)
            {
                fetchTasks.Add(FetchFeedbacksForStudentAsync(student, parentUserId, apiBaseUrl, httpClient));
            }

            var results = await Task.WhenAll(fetchTasks);

            Feedbacks = results
                .SelectMany(r => r)
                .OrderByDescending(f => f.SubmittedAt)
                .ToList();

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error loading My Feedbacks: {ex.Message}");
            ErrorMessage = "An unexpected error occurred.";
            return Page();
        }
    }

    private async Task<List<AllFeedbackViewModel>> FetchFeedbacksForStudentAsync(
        StudentDto student, int parentUserId, string apiBaseUrl, HttpClient httpClient)
    {
        var results = new List<AllFeedbackViewModel>();

        try
        {
            // Get all monthly report IDs for this student
            var monthlyReports = await _context.MonthlyReportEntries
                .AsNoTracking()
                .Where(e => e.StudentId == student.Id)
                .Select(e => new { e.ReportId, e.Report.Month, e.Report.SchoolYear })
                .Distinct()
                .ToListAsync();

            // Get all semester report IDs for this student
            var semesterReports = await _context.SemesterReportEntries
                .AsNoTracking()
                .Where(e => e.StudentId == student.Id)
                .Select(e => new { e.ReportId, e.Report.Semester, e.Report.SchoolYear })
                .Distinct()
                .ToListAsync();

            // Get all yearly report IDs for this student
            var yearlyReports = await _context.YearlyReportEntries
                .AsNoTracking()
                .Where(e => e.StudentId == student.Id)
                .Select(e => new { e.ReportId, e.Report.SchoolYear })
                .Distinct()
                .ToListAsync();

            // Step 3: Fetch feedback for each report in parallel
            var feedbackTasks = new List<Task<List<AllFeedbackViewModel>>>();

            foreach (var r in monthlyReports)
                feedbackTasks.Add(FetchFeedbackAsync(httpClient, apiBaseUrl, "monthly", r.ReportId,
                    $"Monthly — Month {r.Month} / {r.SchoolYear}", student.Id, parentUserId));

            foreach (var r in semesterReports)
                feedbackTasks.Add(FetchFeedbackAsync(httpClient, apiBaseUrl, "semester", r.ReportId,
                    $"Semester {r.Semester} / {r.SchoolYear}", student.Id, parentUserId));

            foreach (var r in yearlyReports)
                feedbackTasks.Add(FetchFeedbackAsync(httpClient, apiBaseUrl, "yearly", r.ReportId,
                    $"Yearly {r.SchoolYear}", student.Id, parentUserId));

            var feedbackResults = await Task.WhenAll(feedbackTasks);
            results.AddRange(feedbackResults.SelectMany(f => f));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching feedbacks for student {student.Id}: {ex.Message}");
        }

        return results;
    }

    private async Task<List<AllFeedbackViewModel>> FetchFeedbackAsync(
        HttpClient httpClient, string apiBaseUrl, string reportType, int reportId,
        string reportLabel, int studentId, int parentUserId)
    {
        try
        {
            var response = await httpClient.GetAsync($"{apiBaseUrl}/api/feedback/report/{reportType}/{reportId}");
            if (!response.IsSuccessStatusCode) return new();

            var content = await response.Content.ReadAsStringAsync();
            var feedbacks = JsonSerializer.Deserialize<List<FeedbackDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (feedbacks == null) return new();

            return feedbacks
                .Where(f => f.ParentUserId == parentUserId)
                .Select(f => new AllFeedbackViewModel
                {
                    ReportType = reportType,
                    ReportId = reportId,
                    ReportLabel = reportLabel,
                    Message = f.Message,
                    SubmittedAt = f.CreatedAt,
                    StudentId = studentId,
                    ReportLink = $"/Reports/{char.ToUpper(reportType[0]) + reportType[1..]}?studentId={studentId}&reportId={reportId}",
                    HistoryLink = $"/Feedback/History?reportType={reportType}&reportId={reportId}"
                })
                .ToList();
        }
        catch
        {
            return new();
        }
    }
}

public class AllFeedbackViewModel
{
    public string ReportType { get; set; } = string.Empty;
    public int ReportId { get; set; }
    public string ReportLabel { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime SubmittedAt { get; set; }
    public int StudentId { get; set; }
    public string ReportLink { get; set; } = string.Empty;
    public string HistoryLink { get; set; } = string.Empty;
}