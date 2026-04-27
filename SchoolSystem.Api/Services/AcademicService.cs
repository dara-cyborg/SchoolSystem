using Microsoft.EntityFrameworkCore;
using SchoolSystem.Api.Data;
using SchoolSystem.Core.DTOs;
using SchoolSystem.Core.DTOs.Class;
using SchoolSystem.Core.DTOs.ClassSubject;
using SchoolSystem.Core.DTOs.Grade;
using SchoolSystem.Core.DTOs.Student;
using SchoolSystem.Core.DTOs.Subject;
using SchoolSystem.Core.Interfaces;
using SchoolSystem.Core.Models;

namespace SchoolSystem.Api.Services;

public class AcademicService : IAcademicService {
    private readonly AppDbContext _context;
    private const int MaxPageSize = 100;

    public AcademicService(AppDbContext context) {
        _context = context;
    }

    // ============ GRADES ============

    public async Task<PagedResult<GradeResponseDto>> GetGradesAsync(int page, int pageSize, CancellationToken cancellationToken = default) {
        if (pageSize < 1) pageSize = 1;
        if (pageSize > MaxPageSize) pageSize = MaxPageSize;

        var query = _context.Grades
            .OrderBy(g => g.Id)
            .AsQueryable();

        var totalCount = await query.CountAsync(cancellationToken);
        var grades = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<GradeResponseDto> {
            Items = grades.Select(g => MapGradeToDto(g)).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<GradeResponseDto?> GetGradeByIdAsync(int id, CancellationToken cancellationToken = default) {
        var grade = await _context.Grades
            .Include(g => g.Classes)
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

        return grade == null ? null : MapGradeToDto(grade);
    }

    public async Task<GradeResponseDto?> CreateGradeAsync(CreateGradeDto createGradeDto, CancellationToken cancellationToken = default) {
        if (string.IsNullOrWhiteSpace(createGradeDto.Name)) {
            throw new InvalidOperationException("Grade name is required.");
        }

        var grade = new Grade {
            Name = createGradeDto.Name,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Grades.Add(grade);
        await _context.SaveChangesAsync(cancellationToken);

        return MapGradeToDto(grade);
    }

    public async Task<bool> UpdateGradeAsync(int id, UpdateGradeDto updateGradeDto, CancellationToken cancellationToken = default) {
        if (string.IsNullOrWhiteSpace(updateGradeDto.Name)) {
            throw new InvalidOperationException("Grade name is required.");
        }

        var grade = await _context.Grades.FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

        if (grade == null) {
            return false;
        }

        grade.Name = updateGradeDto.Name;
        grade.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteGradeAsync(int id, CancellationToken cancellationToken = default) {
        var grade = await _context.Grades.FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

        if (grade == null) {
            return false;
        }

        _context.Grades.Remove(grade);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    // ============ SUBJECTS ============

    public async Task<PagedResult<SubjectResponseDto>> GetSubjectsAsync(int page, int pageSize, CancellationToken cancellationToken = default) {
        if (pageSize < 1) pageSize = 1;
        if (pageSize > MaxPageSize) pageSize = MaxPageSize;

        var query = _context.Subjects
            .OrderBy(s => s.Id)
            .AsQueryable();

        var totalCount = await query.CountAsync(cancellationToken);
        var subjects = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<SubjectResponseDto> {
            Items = subjects.Select(s => MapSubjectToDto(s)).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<SubjectResponseDto?> GetSubjectByIdAsync(int id, CancellationToken cancellationToken = default) {
        var subject = await _context.Subjects
            .Include(s => s.ClassSubjects)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        return subject == null ? null : MapSubjectToDto(subject);
    }

    public async Task<SubjectResponseDto?> CreateSubjectAsync(CreateSubjectDto createSubjectDto, CancellationToken cancellationToken = default) {
        if (string.IsNullOrWhiteSpace(createSubjectDto.Name)) {
            throw new InvalidOperationException("Subject name is required.");
        }

        var subject = new Subject {
            Name = createSubjectDto.Name,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync(cancellationToken);

        return MapSubjectToDto(subject);
    }

    public async Task<bool> UpdateSubjectAsync(int id, UpdateSubjectDto updateSubjectDto, CancellationToken cancellationToken = default) {
        if (string.IsNullOrWhiteSpace(updateSubjectDto.Name)) {
            throw new InvalidOperationException("Subject name is required.");
        }

        var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (subject == null) {
            return false;
        }

        subject.Name = updateSubjectDto.Name;
        subject.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteSubjectAsync(int id, CancellationToken cancellationToken = default) {
        var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (subject == null) {
            return false;
        }

        _context.Subjects.Remove(subject);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    // ============ CLASSES ============

    public async Task<PagedResult<ClassResponseDto>> GetClassesAsync(int? gradeId, short? schoolYear, int page, int pageSize, CancellationToken cancellationToken = default) {
        if (pageSize < 1) pageSize = 1;
        if (pageSize > MaxPageSize) pageSize = MaxPageSize;

        var query = _context.Classes
            .Include(c => c.Grade)
            .Include(c => c.HomeroomTeacher)
            .AsQueryable();

        if (gradeId.HasValue) {
            query = query.Where(c => c.GradeId == gradeId.Value);
        }

        if (schoolYear.HasValue) {
            query = query.Where(c => c.SchoolYear == schoolYear.Value);
        }

        query = query.OrderBy(c => c.Id);

        var totalCount = await query.CountAsync(cancellationToken);
        var classes = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new ClassResponseDto {
                Id = c.Id,
                GradeId = c.GradeId,
                GradeName = c.Grade!.Name,
                SchoolYear = c.SchoolYear,
                Name = c.Name,
                HomeroomUserId = c.HomeroomUserId,
                HomeroomTeacherName = c.HomeroomTeacher!.Name,
                CreatedAt = c.CreatedAt,
                StudentCount = _context.Students.Count(s => s.ClassId == c.Id)
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<ClassResponseDto> {
            Items = classes,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<ClassResponseDto?> GetClassByIdAsync(int id, CancellationToken cancellationToken = default) {
        var cls = await _context.Classes
            .Include(c => c.Grade)
            .Include(c => c.HomeroomTeacher)
            .Include(c => c.Students)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        return cls == null ? null : MapClassToDto(cls);
    }

    public async Task<ClassResponseDto?> CreateClassAsync(CreateClassDto createClassDto, CancellationToken cancellationToken = default) {
        if (string.IsNullOrWhiteSpace(createClassDto.Name)) {
            throw new InvalidOperationException("Class name is required.");
        }

        // Validate GradeId exists
        var gradeExists = await _context.Grades.AnyAsync(g => g.Id == createClassDto.GradeId, cancellationToken);
        if (!gradeExists) {
            throw new InvalidOperationException($"Grade with ID {createClassDto.GradeId} does not exist.");
        }

        // Validate HomeroomUserId if provided
        if (createClassDto.HomeroomUserId.HasValue) {
            var userExists = await _context.Users.AnyAsync(u => u.Id == createClassDto.HomeroomUserId.Value, cancellationToken);
            if (!userExists) {
                throw new InvalidOperationException($"User with ID {createClassDto.HomeroomUserId.Value} does not exist.");
            }
        }

        var cls = new Class {
            GradeId = createClassDto.GradeId,
            SchoolYear = createClassDto.SchoolYear,
            Name = createClassDto.Name,
            HomeroomUserId = createClassDto.HomeroomUserId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Classes.Add(cls);
        await _context.SaveChangesAsync(cancellationToken);

        return MapClassToDto(cls);
    }

    public async Task<ClassResponseDto?> UpdateClassAsync(int id, UpdateClassDto updateClassDto, CancellationToken cancellationToken = default) {
        if (string.IsNullOrWhiteSpace(updateClassDto.Name)) {
            throw new InvalidOperationException("Class name is required.");
        }

        // Validate GradeId exists
        var gradeExists = await _context.Grades.AnyAsync(g => g.Id == updateClassDto.GradeId, cancellationToken);
        if (!gradeExists) {
            throw new InvalidOperationException($"Grade with ID {updateClassDto.GradeId} does not exist.");
        }

        // Validate HomeroomUserId if provided
        if (updateClassDto.HomeroomUserId.HasValue) {
            var userExists = await _context.Users.AnyAsync(u => u.Id == updateClassDto.HomeroomUserId.Value, cancellationToken);
            if (!userExists) {
                throw new InvalidOperationException($"User with ID {updateClassDto.HomeroomUserId.Value} does not exist.");
            }
        }

        var cls = await _context.Classes.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (cls == null) {
            return null;
        }

        cls.GradeId = updateClassDto.GradeId;
        cls.SchoolYear = updateClassDto.SchoolYear;
        cls.Name = updateClassDto.Name;
        cls.HomeroomUserId = updateClassDto.HomeroomUserId;
        cls.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        // Reload with navigations
        cls = await _context.Classes
            .Include(c => c.Grade)
            .Include(c => c.HomeroomTeacher)
            .Include(c => c.Students)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        return cls == null ? null : MapClassToDto(cls);
    }

    public async Task<bool> DeleteClassAsync(int id, CancellationToken cancellationToken = default) {
        var cls = await _context.Classes.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (cls == null) {
            return false;
        }

        _context.Classes.Remove(cls);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<List<StudentDto>> GetClassStudentsAsync(int classId, CancellationToken cancellationToken = default) {
        var students = await _context.Students
            .Where(s => s.ClassId == classId)
            .OrderBy(s => s.Id)
            .ToListAsync(cancellationToken);

        return students.Select(s => new StudentDto {
            Id = s.Id,
            Name = s.Name,
            Sex = s.Sex,
            Dob = s.Dob,
            ClassId = s.ClassId,
            CreatedAt = s.CreatedAt
        }).ToList();
    }

    // ============ CLASS SUBJECTS ============

    public async Task<PagedResult<ClassSubjectResponseDto>> GetClassSubjectsAsync(int page, int pageSize, CancellationToken cancellationToken = default) {
        if (pageSize < 1) pageSize = 1;
        if (pageSize > MaxPageSize) pageSize = MaxPageSize;

        var query = _context.ClassSubjects
            .Include(cs => cs.Class)
            .Include(cs => cs.Subject)
            .Include(cs => cs.Teacher)
            .OrderBy(cs => cs.Id)
            .AsQueryable();

        var totalCount = await query.CountAsync(cancellationToken);
        var classSubjects = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<ClassSubjectResponseDto> {
            Items = classSubjects.Select(cs => MapClassSubjectToDto(cs)).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<ClassSubjectResponseDto?> GetClassSubjectByIdAsync(int id, CancellationToken cancellationToken = default) {
        var classSubject = await _context.ClassSubjects
            .Include(cs => cs.Class)
            .Include(cs => cs.Subject)
            .Include(cs => cs.Teacher)
            .FirstOrDefaultAsync(cs => cs.Id == id, cancellationToken);

        return classSubject == null ? null : MapClassSubjectToDto(classSubject);
    }

    public async Task<ClassSubjectResponseDto?> CreateClassSubjectAsync(CreateClassSubjectDto createClassSubjectDto, CancellationToken cancellationToken = default) {
        // Validate ClassId exists
        var classExists = await _context.Classes.AnyAsync(c => c.Id == createClassSubjectDto.ClassId, cancellationToken);
        if (!classExists) {
            throw new InvalidOperationException($"Class with ID {createClassSubjectDto.ClassId} does not exist.");
        }

        // Validate SubjectId exists
        var subjectExists = await _context.Subjects.AnyAsync(s => s.Id == createClassSubjectDto.SubjectId, cancellationToken);
        if (!subjectExists) {
            throw new InvalidOperationException($"Subject with ID {createClassSubjectDto.SubjectId} does not exist.");
        }

        // Validate TeacherUserId if provided
        if (createClassSubjectDto.TeacherUserId.HasValue) {
            var userExists = await _context.Users.AnyAsync(u => u.Id == createClassSubjectDto.TeacherUserId.Value, cancellationToken);
            if (!userExists) {
                throw new InvalidOperationException($"User with ID {createClassSubjectDto.TeacherUserId.Value} does not exist.");
            }
        }

        var classSubject = new ClassSubject {
            ClassId = createClassSubjectDto.ClassId,
            SubjectId = createClassSubjectDto.SubjectId,
            TeacherUserId = createClassSubjectDto.TeacherUserId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.ClassSubjects.Add(classSubject);
        await _context.SaveChangesAsync(cancellationToken);

        // Reload with navigation properties to populate Class, Subject, Teacher
        classSubject = await _context.ClassSubjects
            .Include(cs => cs.Class)
            .Include(cs => cs.Subject)
            .Include(cs => cs.Teacher)
            .FirstOrDefaultAsync(cs => cs.Id == classSubject.Id, cancellationToken);

        return classSubject == null ? null : MapClassSubjectToDto(classSubject);
    }

    public async Task<bool> UpdateClassSubjectAsync(int id, UpdateClassSubjectDto updateClassSubjectDto, CancellationToken cancellationToken = default) {
        // Validate TeacherUserId if provided
        if (updateClassSubjectDto.TeacherUserId.HasValue) {
            var userExists = await _context.Users.AnyAsync(u => u.Id == updateClassSubjectDto.TeacherUserId.Value, cancellationToken);
            if (!userExists) {
                throw new InvalidOperationException($"User with ID {updateClassSubjectDto.TeacherUserId.Value} does not exist.");
            }
        }

        var classSubject = await _context.ClassSubjects.FirstOrDefaultAsync(cs => cs.Id == id, cancellationToken);

        if (classSubject == null) {
            return false;
        }

        classSubject.TeacherUserId = updateClassSubjectDto.TeacherUserId;
        classSubject.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteClassSubjectAsync(int id, CancellationToken cancellationToken = default) {
        var classSubject = await _context.ClassSubjects.FirstOrDefaultAsync(cs => cs.Id == id, cancellationToken);

        if (classSubject == null) {
            return false;
        }

        _context.ClassSubjects.Remove(classSubject);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    // ============ MAPPING HELPERS ============

    private static GradeResponseDto MapGradeToDto(Grade grade) {
        return new GradeResponseDto {
            Id = grade.Id,
            Name = grade.Name,
            CreatedAt = grade.CreatedAt,
            ClassCount = grade.Classes?.Count ?? 0
        };
    }

    private static SubjectResponseDto MapSubjectToDto(Subject subject) {
        return new SubjectResponseDto {
            Id = subject.Id,
            Name = subject.Name,
            CreatedAt = subject.CreatedAt,
            ClassSubjectCount = subject.ClassSubjects?.Count ?? 0
        };
    }

    private static ClassResponseDto MapClassToDto(Class cls) {
        return new ClassResponseDto {
            Id = cls.Id,
            GradeId = cls.GradeId,
            GradeName = cls.Grade?.Name ?? string.Empty,
            SchoolYear = cls.SchoolYear,
            Name = cls.Name,
            HomeroomUserId = cls.HomeroomUserId,
            HomeroomTeacherName = cls.HomeroomTeacher?.Name,
            CreatedAt = cls.CreatedAt,
            StudentCount = cls.Students?.Count ?? 0
        };
    }

    private static ClassSubjectResponseDto MapClassSubjectToDto(ClassSubject classSubject) {
        return new ClassSubjectResponseDto {
            Id = classSubject.Id,
            ClassId = classSubject.ClassId,
            ClassName = classSubject.Class?.Name ?? string.Empty,
            SubjectId = classSubject.SubjectId,
            SubjectName = classSubject.Subject?.Name ?? string.Empty,
            TeacherUserId = classSubject.TeacherUserId,
            TeacherName = classSubject.Teacher?.Name,
            CreatedAt = classSubject.CreatedAt
        };
    }
}
