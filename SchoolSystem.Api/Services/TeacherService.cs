using Microsoft.EntityFrameworkCore;
using SchoolSystem.Api.Data;
using SchoolSystem.Core.DTOs.ClassSubject;
using SchoolSystem.Core.Interfaces;

namespace SchoolSystem.Api.Services;

public class TeacherService : ITeacherService
{
    private readonly AppDbContext _context;

    public TeacherService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ClassSubjectWithStudentsDto>> GetMyClassSubjectsAsync(int teacherUserId)
    {
        var classSubjects = await _context.ClassSubjects
            .AsNoTracking()
            .Include(cs => cs.Class)
                .ThenInclude(c => c.Students)
            .Include(cs => cs.Subject)
            .Include(cs => cs.Teacher)
            .Where(cs => cs.TeacherUserId == teacherUserId)
            .OrderBy(cs => cs.Id)
            .ToListAsync();

        return classSubjects.Select(cs => new ClassSubjectWithStudentsDto
        {
            Id = cs.Id,
            ClassId = cs.ClassId,
            ClassName = cs.Class.Name,
            SubjectId = cs.SubjectId,
            SubjectName = cs.Subject.Name,
            TeacherUserId = cs.TeacherUserId,
            TeacherName = cs.Teacher?.Name,
            Students = cs.Class.Students
                .Where(student => student.ClassId == cs.ClassId)
                .OrderBy(student => student.Id)
                .Select(student => new StudentInClassDto
                {
                    Id = student.Id,
                    Name = student.Name,
                    Sex = student.Sex.ToString()
                })
                .ToList()
        }).ToList();
    }
}