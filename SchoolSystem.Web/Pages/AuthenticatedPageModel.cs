using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolSystem.Core.DTOs;
using SchoolSystem.Core.DTOs.Student;
using SchoolSystem.Web.Models;
using SchoolSystem.Web.Services;
using System.Security.Claims;
using System.Text.Json;

namespace SchoolSystem.Web.Pages;

public abstract class AuthenticatedPageModel : PageModel
{
    protected readonly ApiHttpClientFactory ApiClientFactory;
    protected readonly ILogger? Logger;

    protected AuthenticatedPageModel(ApiHttpClientFactory apiClientFactory, ILogger? logger = null)
    {
        ApiClientFactory = apiClientFactory;
        Logger = logger;
    }

    protected IActionResult? CheckToken()
    {
        if (!ApiClientFactory.HasToken)
        {
            return RedirectToPage("/Auth/Login");
        }
        return null;
    }

    protected async Task<(bool isOwner, int matchedStudentId)> VerifyOwnershipAsync(string reportType, int reportId)
    {
        if (string.IsNullOrEmpty(reportType) || reportId <= 0) 
            return (false, 0);

        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var parentUserId))
            {
                return (false, 0);
            }

            var apiBaseUrl = ApiClientFactory.GetApiBaseUrl();
            var httpClient = ApiClientFactory.CreateAuthenticatedClient();

            // 1. Get parent's students
            var studentsResponse = await httpClient.GetAsync($"{apiBaseUrl}/api/students/parent/{parentUserId}?pageSize=100");
            if (!studentsResponse.IsSuccessStatusCode) 
                return (false, 0);

            var content = await studentsResponse.Content.ReadAsStringAsync();
            var pagedResult = JsonSerializer.Deserialize<PagedResult<StudentDto>>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
            var studentList = pagedResult?.Items ?? new List<StudentDto>();

            var parentStudentIds = studentList.Select(s => s.Id).ToHashSet();
            if (!parentStudentIds.Any()) 
                return (false, 0);

            // 2. Get report and check if any entry belongs to parent's child
            var reportEndpoint = ReportTypeConstants.GetEndpoint(reportType);

            if (string.IsNullOrEmpty(reportEndpoint)) 
                return (false, 0);

            var reportResponse = await httpClient.GetAsync($"{apiBaseUrl}/api/{reportEndpoint}/{reportId}");
            if (!reportResponse.IsSuccessStatusCode) 
                return (false, 0);

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
                            return (true, studentId);
                        }
                    }
                }
            }

            return (false, 0);
        }
        catch (Exception ex)
        {
            Logger?.LogError($"Error verifying ownership: {ex.Message}");
            return (false, 0);
        }
    }
}
