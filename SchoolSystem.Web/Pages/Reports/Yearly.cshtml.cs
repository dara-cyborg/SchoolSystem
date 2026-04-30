using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Api.Data;
using SchoolSystem.Core.DTOs;
using SchoolSystem.Core.DTOs.Attendance;
using SchoolSystem.Core.DTOs.Report;
using SchoolSystem.Core.DTOs.Student;
using SchoolSystem.Web.Models.ViewModels;
using SchoolSystem.Web.Services;
using System.Security.Claims;
using System.Text.Json;

namespace SchoolSystem.Web.Pages.Reports;

[Authorize]
public class YearlyModel : AuthenticatedPageModel
{
    private readonly ILogger<YearlyModel> _logger;
    private readonly AppDbContext _context;

    public YearlyReportViewModel ReportViewModel { get; set; } = new();
    public List<SubjectScoreViewModel> SubjectScores { get; set; } = new();
    public List<YearlyReportDto> AvailableReports { get; set; } = new();
    public bool ShowReportSelection => AvailableReports.Any();
    public int RequestedStudentId { get; set; }

    public YearlyModel(ApiHttpClientFactory apiClientFactory, ILogger<YearlyModel> logger, AppDbContext context)
        : base(apiClientFactory)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> OnGetAsync(int studentId, int reportId = 0)
    {
        var tokenCheck = CheckToken();
        if (tokenCheck != null) return tokenCheck;
        RequestedStudentId = studentId;
        
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var parentUserId))
            {
                return NotFound();
            }

            var apiBaseUrl = ApiClientFactory.GetApiBaseUrl();
            var httpClient = ApiClientFactory.CreateAuthenticatedClient();

            // 1. Verify ownership and get student details
            var studentsResponse = await httpClient.GetAsync($"{apiBaseUrl}/api/students/parent/{parentUserId}?pageSize=100");
            if (!studentsResponse.IsSuccessStatusCode)
            {
                _logger.LogWarning($"Failed to verify ownership for parent {parentUserId}");
                return NotFound();
            }

            var content = await studentsResponse.Content.ReadAsStringAsync();
            var pagedResult = JsonSerializer.Deserialize<PagedResult<StudentDto>>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
            var studentList = pagedResult?.Items ?? new List<StudentDto>();

            var student = studentList.FirstOrDefault(s => s.Id == studentId);
            if (student == null)
            {
                return NotFound();
            }

            // 2. Always populate available reports (lightweight — no entries loaded)
            var allYearlyReports = await _context.YearlyReportEntries
                .AsNoTracking()
                .Where(e => e.StudentId == studentId)
                .Select(e => new { e.ReportId, e.Report.SchoolYear })
                .Distinct()
                .OrderByDescending(e => e.SchoolYear)
                .ToListAsync();

            AvailableReports = allYearlyReports.Select(r => new YearlyReportDto
            {
                Id = r.ReportId,
                SchoolYear = r.SchoolYear
            }).ToList();

            if (reportId == 0)
            {
                if (!AvailableReports.Any())
                {
                    ReportViewModel.ErrorMessage = "No yearly reports are available for this student.";
                }
                return Page();
            }

            // 3. Get report data
            var reportResponse = await httpClient.GetAsync($"{apiBaseUrl}/api/yearly-reports/{reportId}");
            if (!reportResponse.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var reportContent = await reportResponse.Content.ReadAsStringAsync();
            var report = JsonSerializer.Deserialize<YearlyReportDto>(reportContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (report == null)
            {
                return NotFound();
            }

            if (!report.Entries.Any(e => e.StudentId == studentId))
            {
                return Forbid();
            }

            SubjectScores = await _context.YearlyScores
                .AsNoTracking()
                .Include(ys => ys.ClassSubject)
                .ThenInclude(cs => cs.Subject)
                .Where(ys => ys.StudentId == studentId && ys.SchoolYear == report.SchoolYear)
                .OrderBy(ys => ys.ClassSubject.Subject.Name)
                .Select(ys => new SubjectScoreViewModel
                {
                    SubjectName = ys.ClassSubject.Subject.Name,
                    FinalScore = ys.FinalScore ?? 0
                })
                .ToListAsync();

            // 3. Get attendance data (Using month=1 as default since API requires a valid month)
            int defaultMonthForAttendance = 1; 
            var attendanceResponse = await httpClient.GetAsync($"{apiBaseUrl}/api/attendance/summary/{studentId}?month={defaultMonthForAttendance}&schoolYear={report.SchoolYear}");
            AttendanceSummaryDto? attendance = null;
            if (attendanceResponse.IsSuccessStatusCode)
            {
                var attContent = await attendanceResponse.Content.ReadAsStringAsync();
                attendance = JsonSerializer.Deserialize<AttendanceSummaryDto>(attContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            // 4. Map to ViewModel
            var studentEntry = report.Entries.FirstOrDefault(e => e.StudentId == studentId);

            ReportViewModel = new YearlyReportViewModel
            {
                ReportId = reportId,
                StudentName = student.Name,
                ClassName = student.ClassName,
                SchoolYear = report.SchoolYear.ToString(),
                Entries = report.Entries.Select(e => new EntryViewModel
                {
                    StudentId = e.StudentId,
                    TotalScore = (int)(e.TotalScore ?? 0),
                    Rank = e.Rank ?? 0
                }).ToList(),
                TotalScore = (int)(studentEntry?.TotalScore ?? 0),
                Rank = studentEntry?.Rank ?? 0,
                InformedAbsences = attendance?.InformedAbsences ?? 0,
                UninformedAbsences = attendance?.UninformedAbsences ?? 0,
                StudentId = studentId
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in Yearly Report OnGet: {ex.Message}");
            ReportViewModel.ErrorMessage = "An unexpected error occurred.";
            return Page();
        }
    }
}

public class YearlyReportViewModel
{
    public int ReportId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public string SchoolYear { get; set; } = string.Empty;
    public List<EntryViewModel> Entries { get; set; } = new();
    public int TotalScore { get; set; }
    public int Rank { get; set; }
    public int InformedAbsences { get; set; }
    public int UninformedAbsences { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public int StudentId { get; set; }
}

