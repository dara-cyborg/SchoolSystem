using Microsoft.EntityFrameworkCore;
using SchoolSystem.Api.Data;
using SchoolSystem.Core.DTOs.Report;
using SchoolSystem.Core.Interfaces;
using SchoolSystem.Core.Models;

namespace SchoolSystem.Api.Services;

public class MonthlyReportService : IMonthlyReportService {
    private readonly AppDbContext _context;

    public MonthlyReportService(AppDbContext context) {
        _context = context;
    }

    public async Task<MonthlyReportDto> SubmitReportAsync(SubmitMonthlyReportDto submitDto, int homeroomUserId, CancellationToken cancellationToken = default) {
        var existingReport = await _context.MonthlyReports
            .FirstOrDefaultAsync(mr => 
                mr.ClassId == submitDto.ClassId &&
                mr.Month == submitDto.Month &&
                mr.SchoolYear == submitDto.SchoolYear,
                cancellationToken);

        if (existingReport != null) {
            throw new InvalidOperationException($"A report already exists for Class {submitDto.ClassId}, Month {submitDto.Month}, School Year {submitDto.SchoolYear}.");
        }

        var classExists = await _context.Classes
            .AnyAsync(c => c.Id == submitDto.ClassId, cancellationToken);

        if (!classExists) {
            throw new InvalidOperationException($"Class with ID {submitDto.ClassId} does not exist.");
        }

        // Load all monthly scores for this class+month+schoolyear
        var monthlyScores = await _context.MonthlyScores
            .Include(ms => ms.ClassSubject)
            .Where(ms => 
                ms.ClassSubject.ClassId == submitDto.ClassId &&
                ms.Month == submitDto.Month &&
                ms.SchoolYear == submitDto.SchoolYear)
            .ToListAsync(cancellationToken);

        // Lock all scores
        foreach (var score in monthlyScores) {
            score.IsLocked = true;
            score.UpdatedAt = DateTime.UtcNow;
        }

        // Compute totals per student
        var studentTotals = monthlyScores
            .GroupBy(ms => ms.StudentId)
            .Select(g => new {
                StudentId = g.Key,
                TotalScore = g.Sum(ms => ms.FinalScore)
            })
            .OrderByDescending(st => st.TotalScore)
            .ToList();

        // Assign ranks
        var rankings = studentTotals
            .Select((item, index) => new {
                item.StudentId,
                item.TotalScore,
                Rank = (short)(index + 1)
            })
            .ToList();

        // Create report
        var report = new MonthlyReport {
            ClassId = submitDto.ClassId,
            Month = submitDto.Month,
            SchoolYear = submitDto.SchoolYear,
            SubmittedAt = DateTime.UtcNow,
            SubmittedBy = homeroomUserId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.MonthlyReports.Add(report);
        await _context.SaveChangesAsync(cancellationToken);

        // Create entries
        var entries = rankings.Select(r => new MonthlyReportEntry {
            ReportId = report.Id,
            StudentId = r.StudentId,
            TotalScore = r.TotalScore,
            Rank = r.Rank,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        }).ToList();

        _context.MonthlyReportEntries.AddRange(entries);

        // Update monthly scores to locked
        _context.MonthlyScores.UpdateRange(monthlyScores);

        await _context.SaveChangesAsync(cancellationToken);

        report.Entries = entries;

        return MapToDto(report);
    }

    public async Task<MonthlyReportDto?> GetReportAsync(int id, CancellationToken cancellationToken = default) {
        var report = await _context.MonthlyReports
            .Include(mr => mr.Entries)
            .FirstOrDefaultAsync(mr => mr.Id == id, cancellationToken);

        if (report == null) {
            return null;
        }

        return MapToDto(report);
    }

    private static MonthlyReportDto MapToDto(MonthlyReport report) {
        return new MonthlyReportDto {
            Id = report.Id,
            ClassId = report.ClassId,
            Month = report.Month,
            SchoolYear = report.SchoolYear,
            CreatedAt = report.CreatedAt,
            Entries = report.Entries
                .OrderBy(e => e.Rank)
                .Select(e => new MonthlyReportEntryDto {
                    StudentId = e.StudentId,
                    TotalScore = e.TotalScore,
                    Rank = e.Rank
                })
                .ToList()
        };
    }
}
