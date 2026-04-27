using SchoolSystem.Core.DTOs.Gradebook;

namespace SchoolSystem.Core.Interfaces;

public interface IGradebookService {
    Task<GradebookEntryDto> CreateAsync(CreateGradebookEntryDto createDto, int teacherUserId, CancellationToken cancellationToken = default);
    Task<GradebookEntryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<GradebookEntryDto?> UpdateAsync(int id, UpdateGradebookEntryDto updateDto, int teacherUserId, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, int teacherUserId, CancellationToken cancellationToken = default);
    Task<List<GradebookEntryDto>?> GetByClassSubjectAsync(int classSubjectId, CancellationToken cancellationToken = default);
    Task<List<GradebookEntryDto>?> GetByStudentAndClassSubjectAsync(int studentId, int classSubjectId, CancellationToken cancellationToken = default);
}
