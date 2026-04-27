using SchoolSystem.Core.DTOs.Feedback;
using SchoolSystem.Core.Enums;

namespace SchoolSystem.Core.Interfaces;

public interface IFeedbackService {
    Task<FeedbackDto> CreateAsync(CreateFeedbackDto createDto, int parentUserId, CancellationToken cancellationToken = default);
    Task<List<FeedbackDto>> GetByReportAsync(ReportType reportType, int reportId, int? parentUserId = null, CancellationToken cancellationToken = default);
    Task<List<FeedbackDto>> GetByClassAsync(int classId, CancellationToken cancellationToken = default);
}
