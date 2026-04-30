using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolSystem.Core.DTOs.Attendance;
using SchoolSystem.Core.DTOs.Report;
using SchoolSystem.Core.DTOs.Student;
using System.Security.Claims;
using System.Text.Json;

namespace SchoolSystem.Web.Pages.Reports;

[Authorize]
public class SemesterModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<SemesterModel> _logger;

    public SemesterReportViewModel ReportViewModel { get; set; } = new();

    public SemesterModel(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<SemesterModel> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync(int studentId, int reportId)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var parentUserId))
            {
                return Forbid();
            }

            var apiBaseUrl = _configuration["ApiBaseUrl"] ?? "https://localhost:5001";
            var httpClient = _httpClientFactory.CreateClient();

            // 1. Verify ownership and get student details
            var studentsResponse = await httpClient.GetAsync($"{apiBaseUrl}/api/students/parent/{parentUserId}?pageSize=100");
            if (!studentsResponse.IsSuccessStatusCode)
            {
                _logger.LogWarning($"Failed to verify ownership for parent {parentUserId}");
                return Forbid();
            }

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

            var student = studentList.FirstOrDefault(s => s.Id == studentId);
            if (student == null)
            {
                return Forbid();
            }

            // 2. Get report data
            var reportResponse = await httpClient.GetAsync($"{apiBaseUrl}/api/semester-reports/{reportId}");
            if (!reportResponse.IsSuccessStatusCode)
            {
                ReportViewModel.ErrorMessage = "Failed to load report data.";
                return Page();
            }

            var reportContent = await reportResponse.Content.ReadAsStringAsync();
            var report = JsonSerializer.Deserialize<SemesterReportDto>(reportContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (report == null)
            {
                ReportViewModel.ErrorMessage = "Report not found.";
                return Page();
            }

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

            ReportViewModel = new SemesterReportViewModel
            {
                StudentName = student.Name,
                ClassName = student.ClassName,
                SchoolYear = report.SchoolYear.ToString(),
                Semester = report.Semester,
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
            _logger.LogError($"Error in Semester Report OnGet: {ex.Message}");
            ReportViewModel.ErrorMessage = "An unexpected error occurred.";
            return Page();
        }
    }
}

public class SemesterReportViewModel
{
    public string StudentName { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public string SchoolYear { get; set; } = string.Empty;
    public int Semester { get; set; }
    public List<EntryViewModel> Entries { get; set; } = new();
    public int TotalScore { get; set; }
    public int Rank { get; set; }
    public int InformedAbsences { get; set; }
    public int UninformedAbsences { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public int StudentId { get; set; }
}
