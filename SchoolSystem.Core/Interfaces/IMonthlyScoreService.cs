using SchoolSystem.Core.DTOs.Score;

namespace SchoolSystem.Core.Interfaces;

public interface IMonthlyScoreService {
    Task<MonthlyScoreDto> SubmitAsync(SubmitMonthlyScoreDto submitDto, int teacherUserId, CancellationToken cancellationToken = default);
    Task<MonthlyScoreDto?> UpdateAsync(int id, decimal finalScore, CancellationToken cancellationToken = default);
}
