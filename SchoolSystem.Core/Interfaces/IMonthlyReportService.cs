using SchoolSystem.Core.DTOs.Report;

namespace SchoolSystem.Core.Interfaces;

public interface IMonthlyReportService
{
    Task<MonthlyReportDto> SubmitReportAsync(SubmitMonthlyReportDto submitDto, int homeroomUserId, CancellationToken cancellationToken = default);
    Task<MonthlyReportDto?> GetReportAsync(int id, CancellationToken cancellationToken = default);
    Task<MonthlyReportDto?> GetReportByClassAsync(int classId, short month, short schoolYear, CancellationToken cancellationToken = default);
}
