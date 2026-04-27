using SchoolSystem.Core.DTOs;
using SchoolSystem.Core.DTOs.Student;

namespace SchoolSystem.Core.Interfaces;

public interface IStudentService {
    Task<PagedResult<StudentDto>> GetStudentsAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<StudentDto?> GetStudentByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<StudentDto?> CreateStudentAsync(CreateStudentDto createStudentDto, CancellationToken cancellationToken = default);
    Task<StudentDto?> UpdateStudentAsync(int id, UpdateStudentDto updateStudentDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteStudentAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<StudentDto>> GetStudentsByClassAsync(int classId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<StudentDto>> GetStudentsByParentAsync(int parentUserId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<bool> LinkParentAsync(int studentId, LinkParentDto linkParentDto, CancellationToken cancellationToken = default);
}
