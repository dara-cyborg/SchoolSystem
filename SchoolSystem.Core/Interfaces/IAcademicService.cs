using SchoolSystem.Core.DTOs;
using SchoolSystem.Core.DTOs.Class;
using SchoolSystem.Core.DTOs.ClassSubject;
using SchoolSystem.Core.DTOs.Grade;
using SchoolSystem.Core.DTOs.Student;
using SchoolSystem.Core.DTOs.Subject;

namespace SchoolSystem.Core.Interfaces;

public interface IAcademicService {
    // Grade operations
    Task<PagedResult<GradeResponseDto>> GetGradesAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<GradeResponseDto?> GetGradeByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<GradeResponseDto?> CreateGradeAsync(CreateGradeDto createGradeDto, CancellationToken cancellationToken = default);
    Task<bool> UpdateGradeAsync(int id, UpdateGradeDto updateGradeDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteGradeAsync(int id, CancellationToken cancellationToken = default);

    // Subject operations
    Task<PagedResult<SubjectResponseDto>> GetSubjectsAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<SubjectResponseDto?> GetSubjectByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<SubjectResponseDto?> CreateSubjectAsync(CreateSubjectDto createSubjectDto, CancellationToken cancellationToken = default);
    Task<bool> UpdateSubjectAsync(int id, UpdateSubjectDto updateSubjectDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteSubjectAsync(int id, CancellationToken cancellationToken = default);

    // Class operations
    Task<PagedResult<ClassResponseDto>> GetClassesAsync(int? gradeId, short? schoolYear, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<ClassResponseDto?> GetClassByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ClassResponseDto?> CreateClassAsync(CreateClassDto createClassDto, CancellationToken cancellationToken = default);
    Task<ClassResponseDto?> UpdateClassAsync(int id, UpdateClassDto updateClassDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteClassAsync(int id, CancellationToken cancellationToken = default);
    Task<List<StudentDto>> GetClassStudentsAsync(int classId, CancellationToken cancellationToken = default);

    // ClassSubject operations
    Task<PagedResult<ClassSubjectResponseDto>> GetClassSubjectsAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<ClassSubjectResponseDto?> GetClassSubjectByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ClassSubjectResponseDto?> CreateClassSubjectAsync(CreateClassSubjectDto createClassSubjectDto, CancellationToken cancellationToken = default);
    Task<bool> UpdateClassSubjectAsync(int id, UpdateClassSubjectDto updateClassSubjectDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteClassSubjectAsync(int id, CancellationToken cancellationToken = default);
}
