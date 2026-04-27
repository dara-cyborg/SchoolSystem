using Microsoft.EntityFrameworkCore;
using SchoolSystem.Api.Data;
using SchoolSystem.Core.DTOs;
using SchoolSystem.Core.DTOs.Student;
using SchoolSystem.Core.Interfaces;
using SchoolSystem.Core.Models;

namespace SchoolSystem.Api.Services;

public class StudentService : IStudentService {
    private readonly AppDbContext _context;
    private const int MaxPageSize = 100;

    public StudentService(AppDbContext context) {
        _context = context;
    }

    public async Task<PagedResult<StudentDto>> GetStudentsAsync(int page, int pageSize, CancellationToken cancellationToken = default) {
        if (pageSize < 1) pageSize = 1;
        if (pageSize > MaxPageSize) pageSize = MaxPageSize;

        var query = _context.Students
            .Include(s => s.Class)
            .OrderBy(s => s.Id)
            .AsQueryable();

        var totalCount = await query.CountAsync(cancellationToken);
        var students = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<StudentDto> {
            Items = students.Select(s => MapStudentToDto(s)).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<StudentDto?> GetStudentByIdAsync(int id, CancellationToken cancellationToken = default) {
        var student = await _context.Students
            .Include(s => s.Class)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        return student != null ? MapStudentToDto(student) : null;
    }

    public async Task<StudentDto?> CreateStudentAsync(CreateStudentDto createStudentDto, CancellationToken cancellationToken = default) {
        // Validate that ClassId exists
        var classExists = await _context.Classes
            .AnyAsync(c => c.Id == createStudentDto.ClassId, cancellationToken);

        if (!classExists) {
            throw new InvalidOperationException($"Class with ID {createStudentDto.ClassId} does not exist.");
        }

        var student = new Student {
            Name = createStudentDto.Name,
            Sex = createStudentDto.Sex,
            Dob = createStudentDto.Dob,
            Contact = createStudentDto.Contact,
            ClassId = createStudentDto.ClassId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Students.Add(student);
        await _context.SaveChangesAsync(cancellationToken);

        // Reload the entity with navigation properties
        await _context.Entry(student).Reference(s => s.Class).LoadAsync(cancellationToken);

        return MapStudentToDto(student);
    }

    public async Task<StudentDto?> UpdateStudentAsync(int id, UpdateStudentDto updateStudentDto, CancellationToken cancellationToken = default) {
        var student = await _context.Students
            .Include(s => s.Class)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (student == null) {
            return null;
        }

        // Validate that ClassId exists if it changed
        if (student.ClassId != updateStudentDto.ClassId) {
            var classExists = await _context.Classes
                .AnyAsync(c => c.Id == updateStudentDto.ClassId, cancellationToken);

            if (!classExists) {
                throw new InvalidOperationException($"Class with ID {updateStudentDto.ClassId} does not exist.");
            }
        }

        student.Name = updateStudentDto.Name;
        student.Sex = updateStudentDto.Sex;
        student.Dob = updateStudentDto.Dob;
        student.Contact = updateStudentDto.Contact;
        student.ClassId = updateStudentDto.ClassId;

        await _context.SaveChangesAsync(cancellationToken);

        return MapStudentToDto(student);
    }

    public async Task<bool> DeleteStudentAsync(int id, CancellationToken cancellationToken = default) {
        var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (student == null) {
            return false;
        }

        _context.Students.Remove(student);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<PagedResult<StudentDto>> GetStudentsByClassAsync(int classId, int page, int pageSize, CancellationToken cancellationToken = default) {
        if (pageSize < 1) pageSize = 1;
        if (pageSize > MaxPageSize) pageSize = MaxPageSize;

        var query = _context.Students
            .Where(s => s.ClassId == classId)
            .Include(s => s.Class)
            .OrderBy(s => s.Id)
            .AsQueryable();

        var totalCount = await query.CountAsync(cancellationToken);
        var students = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<StudentDto> {
            Items = students.Select(s => MapStudentToDto(s)).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<PagedResult<StudentDto>> GetStudentsByParentAsync(int parentUserId, int page, int pageSize, CancellationToken cancellationToken = default) {
        if (pageSize < 1) pageSize = 1;
        if (pageSize > MaxPageSize) pageSize = MaxPageSize;

        // Verify that the parent user exists
        var parentExists = await _context.Users
            .AnyAsync(u => u.Id == parentUserId, cancellationToken);

        if (!parentExists) {
            throw new InvalidOperationException($"Parent user with ID {parentUserId} does not exist.");
        }

        var query = _context.ParentStudents
            .Where(ps => ps.ParentUserId == parentUserId)
            .Include(ps => ps.Student)
                .ThenInclude(s => s.Class)
            .Select(ps => ps.Student)
            .OrderBy(s => s.Id)
            .AsQueryable();

        var totalCount = await query.CountAsync(cancellationToken);
        var students = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<StudentDto> {
            Items = students.Select(s => MapStudentToDto(s)).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<bool> LinkParentAsync(int studentId, LinkParentDto linkParentDto, CancellationToken cancellationToken = default) {
        // Validate that the student exists
        var studentExists = await _context.Students
            .AnyAsync(s => s.Id == studentId, cancellationToken);

        if (!studentExists) {
            throw new InvalidOperationException($"Student with ID {studentId} does not exist.");
        }

        // Validate that the parent user exists
        var parentExists = await _context.Users
            .AnyAsync(u => u.Id == linkParentDto.ParentUserId, cancellationToken);

        if (!parentExists) {
            throw new InvalidOperationException($"Parent user with ID {linkParentDto.ParentUserId} does not exist.");
        }

        // Check if the link already exists
        var linkExists = await _context.ParentStudents
            .AnyAsync(ps => ps.StudentId == studentId && ps.ParentUserId == linkParentDto.ParentUserId, cancellationToken);

        if (linkExists) {
            throw new InvalidOperationException("Parent is already linked to this student.");
        }

        var parentStudent = new ParentStudent {
            StudentId = studentId,
            ParentUserId = linkParentDto.ParentUserId,
            CreatedAt = DateTime.UtcNow
        };

        _context.ParentStudents.Add(parentStudent);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static StudentDto MapStudentToDto(Student student) {
        return new StudentDto {
            Id = student.Id,
            Name = student.Name,
            Sex = student.Sex,
            Dob = student.Dob,
            Contact = student.Contact,
            ClassId = student.ClassId,
            ClassName = student.Class?.Name ?? string.Empty,
            CreatedAt = student.CreatedAt
        };
    }
}
