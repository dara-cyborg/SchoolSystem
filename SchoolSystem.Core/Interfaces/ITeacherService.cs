using SchoolSystem.Core.DTOs.ClassSubject;

namespace SchoolSystem.Core.Interfaces;

public interface ITeacherService {
    Task<List<ClassSubjectWithStudentsDto>> GetMyClassSubjectsAsync(int teacherUserId);
}