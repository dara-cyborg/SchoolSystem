using Microsoft.EntityFrameworkCore;
using SchoolSystem.Api.Data;
using SchoolSystem.Core.DTOs.Score;
using SchoolSystem.Core.Interfaces;
using SchoolSystem.Core.Models;

namespace SchoolSystem.Api.Services;

public class MonthlyScoreService : IMonthlyScoreService {
    private readonly AppDbContext _context;

    public MonthlyScoreService(AppDbContext context) {
        _context = context;
    }

    public async Task<MonthlyScoreDto> SubmitAsync(SubmitMonthlyScoreDto submitDto, int teacherUserId, CancellationToken cancellationToken = default) {
        var studentExists = await _context.Students
            .AnyAsync(s => s.Id == submitDto.StudentId, cancellationToken);

        if (!studentExists) {
            throw new InvalidOperationException($"Student with ID {submitDto.StudentId} does not exist.");
        }

        var classSubject = await _context.ClassSubjects
            .FirstOrDefaultAsync(cs => cs.Id == submitDto.ClassSubjectId, cancellationToken);

        if (classSubject == null) {
            throw new InvalidOperationException($"ClassSubject with ID {submitDto.ClassSubjectId} does not exist.");
        }

        if (classSubject.TeacherUserId != teacherUserId) {
            throw new InvalidOperationException("You are not assigned to this class-subject.");
        }

        var existingScore = await _context.MonthlyScores
            .FirstOrDefaultAsync(ms => 
                ms.StudentId == submitDto.StudentId &&
                ms.ClassSubjectId == submitDto.ClassSubjectId &&
                ms.Month == submitDto.Month &&
                ms.SchoolYear == submitDto.SchoolYear,
                cancellationToken);

        if (existingScore != null) {
            existingScore.FinalScore = submitDto.FinalScore;
            existingScore.SubmittedAt = DateTime.UtcNow;
            existingScore.SubmittedBy = teacherUserId;
            existingScore.UpdatedAt = DateTime.UtcNow;
            _context.MonthlyScores.Update(existingScore);
        } else {
            var monthlyScore = new MonthlyScore {
                StudentId = submitDto.StudentId,
                ClassSubjectId = submitDto.ClassSubjectId,
                Month = submitDto.Month,
                SchoolYear = submitDto.SchoolYear,
                FinalScore = submitDto.FinalScore,
                SubmittedAt = DateTime.UtcNow,
                SubmittedBy = teacherUserId,
                IsLocked = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.MonthlyScores.Add(monthlyScore);
            existingScore = monthlyScore;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(existingScore);
    }

    public async Task<MonthlyScoreDto?> UpdateAsync(int id, decimal finalScore, CancellationToken cancellationToken = default) {
        var score = await _context.MonthlyScores
            .FirstOrDefaultAsync(ms => ms.Id == id, cancellationToken);

        if (score == null) {
            return null;
        }

        if (score.IsLocked) {
            throw new InvalidOperationException("Score is locked and cannot be edited.");
        }

        score.FinalScore = finalScore;
        score.UpdatedAt = DateTime.UtcNow;

        _context.MonthlyScores.Update(score);
        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(score);
    }

    private static MonthlyScoreDto MapToDto(MonthlyScore score) {
        return new MonthlyScoreDto {
            Id = score.Id,
            StudentId = score.StudentId,
            ClassSubjectId = score.ClassSubjectId,
            Month = score.Month,
            SchoolYear = score.SchoolYear,
            FinalScore = score.FinalScore,
            IsLocked = score.IsLocked
        };
    }
}
