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
public class MonthlyModel : AuthenticatedPageModel
{
    private readonly ILogger<MonthlyModel> _logger;
    private readonly AppDbContext _context;

    public MonthlyReportViewModel ReportViewModel { get; set; } = new();
    public List<SubjectScoreViewModel> SubjectScores { get; set; } = new();
    public List<MonthlyReportDto> AvailableReports { get; set; } = new();
    public bool ShowReportSelection => AvailableReports.Any();
    public int RequestedStudentId { get; set; }

    public MonthlyModel(ApiHttpClientFactory apiClientFactory, ILogger<MonthlyModel> logger, AppDbContext context)
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

            // 2. If reportId is not provided, fetch reports that include this student
            if (reportId == 0)
            {
                var availableReports = await _context.MonthlyReports
                    .AsNoTracking()
                    .Include(mr => mr.Entries)
                    .Where(mr => mr.Entries.Any(e => e.StudentId == studentId))
                    .OrderByDescending(mr => mr.SchoolYear)
                    .ThenByDescending(mr => mr.Month)
                    .ToListAsync();

                if (!availableReports.Any())
                {
                    ReportViewModel.ErrorMessage = "No monthly reports are available for this student's class.";
                    return Page();
                }

                // Map to DTOs for display
                AvailableReports = availableReports.Select(mr => new MonthlyReportDto
                {
                    Id = mr.Id,
                    Month = mr.Month,
                    SchoolYear = mr.SchoolYear
                }).ToList();

                return Page();
            }

            // 3. Get report data
            var reportResponse = await httpClient.GetAsync($"{apiBaseUrl}/api/monthly-reports/{reportId}");
            if (!reportResponse.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var reportContent = await reportResponse.Content.ReadAsStringAsync();
            var report = JsonSerializer.Deserialize<MonthlyReportDto>(reportContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (report == null)
            {
                return NotFound();
            }

            if (!report.Entries.Any(e => e.StudentId == studentId))
            {
                return Forbid();
            }

            SubjectScores = await _context.MonthlyScores
                .AsNoTracking()
                .Include(ms => ms.ClassSubject)
                .ThenInclude(cs => cs.Subject)
                .Where(ms => ms.StudentId == studentId && ms.Month == report.Month && ms.SchoolYear == report.SchoolYear)
                .OrderBy(ms => ms.ClassSubject.Subject.Name)
                .Select(ms => new SubjectScoreViewModel
                {
                    SubjectName = ms.ClassSubject.Subject.Name,
                    FinalScore = ms.FinalScore
                })
                .ToListAsync();

            // 3. Get attendance data
            var attendanceResponse = await httpClient.GetAsync($"{apiBaseUrl}/api/attendance/summary/{studentId}?month={report.Month}&schoolYear={report.SchoolYear}");
            AttendanceSummaryDto? attendance = null;
            if (attendanceResponse.IsSuccessStatusCode)
            {
                var attContent = await attendanceResponse.Content.ReadAsStringAsync();
                attendance = JsonSerializer.Deserialize<AttendanceSummaryDto>(attContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            // 4. Map to ViewModel
            var studentEntry = report.Entries.FirstOrDefault(e => e.StudentId == studentId);

            ReportViewModel = new MonthlyReportViewModel
            {
                StudentName = student.Name,
                ClassName = student.ClassName,
                SchoolYear = report.SchoolYear.ToString(),
                Month = report.Month,
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
            _logger.LogError($"Error in Monthly Report OnGet: {ex.Message}");
            ReportViewModel.ErrorMessage = "An unexpected error occurred.";
            return Page();
        }
    }
}

public class MonthlyReportViewModel
{
    public string StudentName { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public string SchoolYear { get; set; } = string.Empty;
    public int Month { get; set; }
    public List<EntryViewModel> Entries { get; set; } = new();
    public int TotalScore { get; set; }
    public int Rank { get; set; }
    public int InformedAbsences { get; set; }
    public int UninformedAbsences { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public int StudentId { get; set; }
}

