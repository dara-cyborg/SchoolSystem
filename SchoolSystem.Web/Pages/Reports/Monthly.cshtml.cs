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
public class MonthlyModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<MonthlyModel> _logger;

    public MonthlyReportViewModel ReportViewModel { get; set; } = new();

    public MonthlyModel(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<MonthlyModel> logger)
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
                    studentList = JsonSerializer.Deserialize<List<StudentDto>>(itemsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
                }
                else
                {
                    var listJson = JsonSerializer.Serialize(studentsData);
                    studentList = JsonSerializer.Deserialize<List<StudentDto>>(listJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
                }
            }

            var student = studentList.FirstOrDefault(s => s.Id == studentId);
            if (student == null)
            {
                return Forbid();
            }

            // 2. Get report data
            var reportResponse = await httpClient.GetAsync($"{apiBaseUrl}/api/monthly-reports/{reportId}");
            if (!reportResponse.IsSuccessStatusCode)
            {
                ReportViewModel.ErrorMessage = "Failed to load report data.";
                return Page();
            }

            var reportContent = await reportResponse.Content.ReadAsStringAsync();
            var report = JsonSerializer.Deserialize<MonthlyReportDto>(reportContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (report == null)
            {
                ReportViewModel.ErrorMessage = "Report not found.";
                return Page();
            }

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

public class EntryViewModel
{
    public int StudentId { get; set; }
    public int TotalScore { get; set; }
    public int Rank { get; set; }
}
