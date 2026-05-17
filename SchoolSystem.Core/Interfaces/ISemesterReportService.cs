using SchoolSystem.Core.DTOs.Report;

namespace SchoolSystem.Core.Interfaces;

public interface ISemesterReportService
{
    Task<SemesterReportDto> SubmitReportAsync(SubmitSemesterReportDto submitDto, int homeroomUserId, CancellationToken cancellationToken = default);
    Task<SemesterReportDto?> GetReportAsync(int id, CancellationToken cancellationToken = default);
    Task<SemesterReportDto?> GetReportByClassAsync(int classId, short semester, short schoolYear, CancellationToken cancellationToken = default);
}
