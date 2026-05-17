using SchoolSystem.Core.DTOs.Report;

namespace SchoolSystem.Core.Interfaces;

public interface IYearlyReportService
{
    Task<YearlyReportDto> SubmitReportAsync(SubmitYearlyReportDto submitDto, int homeroomUserId, CancellationToken cancellationToken = default);
    Task<YearlyReportDto?> GetReportAsync(int id, CancellationToken cancellationToken = default);
    Task<YearlyReportDto?> GetReportByClassAsync(int classId, short schoolYear, CancellationToken cancellationToken = default);
}
