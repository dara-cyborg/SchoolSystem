using Microsoft.EntityFrameworkCore;
using SchoolSystem.Api.Data;
using SchoolSystem.Core.DTOs.Gradebook;
using SchoolSystem.Core.Interfaces;
using SchoolSystem.Core.Models;

namespace SchoolSystem.Api.Services;

public class GradebookService : IGradebookService {
    private readonly AppDbContext _context;

    public GradebookService(AppDbContext context) {
        _context = context;
    }

    public async Task<GradebookEntryDto> CreateAsync(CreateGradebookEntryDto createDto, int teacherUserId, CancellationToken cancellationToken = default) {
        var studentExists = await _context.Students
            .AnyAsync(s => s.Id == createDto.StudentId, cancellationToken);

        if (!studentExists) {
            throw new InvalidOperationException($"Student with ID {createDto.StudentId} does not exist.");
        }

        var classSubject = await _context.ClassSubjects
            .FirstOrDefaultAsync(cs => cs.Id == createDto.ClassSubjectId, cancellationToken);

        if (classSubject == null) {
            throw new InvalidOperationException($"ClassSubject with ID {createDto.ClassSubjectId} does not exist.");
        }

        if (classSubject.TeacherUserId != teacherUserId) {
            throw new InvalidOperationException("You are not assigned to this class-subject.");
        }

        var gradebookEntry = new GradebookEntry {
            StudentId = createDto.StudentId,
            ClassSubjectId = createDto.ClassSubjectId,
            Label = createDto.Label,
            Score = createDto.Score,
            MaxScore = createDto.MaxScore,
            EntryDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.GradebookEntries.Add(gradebookEntry);
        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(gradebookEntry);
    }

    public async Task<GradebookEntryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default) {
        var entry = await _context.GradebookEntries
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        return entry == null ? null : MapToDto(entry);
    }

    public async Task<GradebookEntryDto?> UpdateAsync(int id, UpdateGradebookEntryDto updateDto, int teacherUserId, CancellationToken cancellationToken = default) {
        var entry = await _context.GradebookEntries
            .Include(e => e.ClassSubject)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        if (entry == null) {
            return null;
        }

        if (entry.ClassSubject.TeacherUserId != teacherUserId) {
            throw new InvalidOperationException("You are not assigned to this class-subject.");
        }

        entry.Label = updateDto.Label;
        entry.Score = updateDto.Score;
        entry.MaxScore = updateDto.MaxScore;
        entry.UpdatedAt = DateTime.UtcNow;

        _context.GradebookEntries.Update(entry);
        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(entry);
    }

    public async Task<bool> DeleteAsync(int id, int teacherUserId, CancellationToken cancellationToken = default) {
        var entry = await _context.GradebookEntries
            .Include(e => e.ClassSubject)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        if (entry == null) {
            return false;
        }

        if (entry.ClassSubject.TeacherUserId != teacherUserId) {
            throw new InvalidOperationException("You are not assigned to this class-subject.");
        }

        _context.GradebookEntries.Remove(entry);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<List<GradebookEntryDto>?> GetByClassSubjectAsync(int classSubjectId, CancellationToken cancellationToken = default) {
        var classSubjectExists = await _context.ClassSubjects
            .AnyAsync(cs => cs.Id == classSubjectId, cancellationToken);

        if (!classSubjectExists) {
            return null;
        }

        var entries = await _context.GradebookEntries
            .Where(e => e.ClassSubjectId == classSubjectId)
            .OrderBy(e => e.Id)
            .ToListAsync(cancellationToken);

        return entries.Select(MapToDto).ToList();
    }

    public async Task<List<GradebookEntryDto>?> GetByStudentAndClassSubjectAsync(int studentId, int classSubjectId, CancellationToken cancellationToken = default) {
        var classSubjectExists = await _context.ClassSubjects
            .AnyAsync(cs => cs.Id == classSubjectId, cancellationToken);

        if (!classSubjectExists) {
            return null;
        }

        var entries = await _context.GradebookEntries
            .Where(e => e.StudentId == studentId && e.ClassSubjectId == classSubjectId)
            .OrderBy(e => e.Id)
            .ToListAsync(cancellationToken);

        return entries.Select(MapToDto).ToList();
    }

    private static GradebookEntryDto MapToDto(GradebookEntry entry) {
        return new GradebookEntryDto {
            Id = entry.Id,
            StudentId = entry.StudentId,
            ClassSubjectId = entry.ClassSubjectId,
            Label = entry.Label,
            Score = entry.Score,
            MaxScore = entry.MaxScore,
            CreatedAt = entry.CreatedAt
        };
    }
}
