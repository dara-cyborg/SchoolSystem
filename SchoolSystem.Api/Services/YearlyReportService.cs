using Microsoft.EntityFrameworkCore;
using SchoolSystem.Api.Data;
using SchoolSystem.Core.DTOs.Report;
using SchoolSystem.Core.Interfaces;
using SchoolSystem.Core.Models;

namespace SchoolSystem.Api.Services;

public class YearlyReportService : IYearlyReportService {
    private readonly AppDbContext _context;

    public YearlyReportService(AppDbContext context) {
        _context = context;
    }

    public async Task<YearlyReportDto> SubmitReportAsync(SubmitYearlyReportDto submitDto, int homeroomUserId, CancellationToken cancellationToken = default) {
        var existingReport = await _context.YearlyReports
            .FirstOrDefaultAsync(yr => 
                yr.ClassId == submitDto.ClassId &&
                yr.SchoolYear == submitDto.SchoolYear,
                cancellationToken);

        if (existingReport != null) {
            throw new InvalidOperationException($"A report already exists for Class {submitDto.ClassId}, School Year {submitDto.SchoolYear}.");
        }

        var classExists = await _context.Classes
            .AnyAsync(c => c.Id == submitDto.ClassId, cancellationToken);

        if (!classExists) {
            throw new InvalidOperationException($"Class with ID {submitDto.ClassId} does not exist.");
        }

        // Get all class subjects for this class
        var classSubjects = await _context.ClassSubjects
            .Where(cs => cs.ClassId == submitDto.ClassId)
            .Select(cs => cs.Id)
            .ToListAsync(cancellationToken);

        // Get all students in this class
        var studentIds = await _context.Students
            .Where(s => s.ClassId == submitDto.ClassId)
            .Select(s => s.Id)
            .ToListAsync(cancellationToken);

        // Get all semester scores for both semesters
        var semesterScores = await _context.SemesterScores
            .Where(ss => 
                classSubjects.Contains(ss.ClassSubjectId) &&
                ss.SchoolYear == submitDto.SchoolYear)
            .ToListAsync(cancellationToken);

        // Verify all required semester scores are present (2 semesters per student per classsubject)
        var expectedScoreCount = studentIds.Count * classSubjects.Count * 2;
        var actualScoreCount = semesterScores.Count;

        if (actualScoreCount < expectedScoreCount) {
            throw new InvalidOperationException("Not all semester scores are submitted for this school year.");
        }

        // Aggregate semester scores into yearly scores
        var yearlyScoreMap = new Dictionary<(int studentId, int classSubjectId), List<decimal>>();

        foreach (var score in semesterScores) {
            var key = (score.StudentId, score.ClassSubjectId);
            if (!yearlyScoreMap.ContainsKey(key)) {
                yearlyScoreMap[key] = new List<decimal>();
            }
            if (score.FinalScore.HasValue) {
                yearlyScoreMap[key].Add(score.FinalScore.Value);
            }
        }

        var yearlyScores = new List<YearlyScore>();
        foreach (var kvp in yearlyScoreMap) {
            var (studentId, classSubjectId) = kvp.Key;
            var averageScore = kvp.Value.Count > 0 ? kvp.Value.Average() : (decimal?)null;

            var yearlyScore = new YearlyScore {
                StudentId = studentId,
                ClassSubjectId = classSubjectId,
                SchoolYear = submitDto.SchoolYear,
                FinalScore = averageScore,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            yearlyScores.Add(yearlyScore);
        }

        _context.YearlyScores.AddRange(yearlyScores);
        await _context.SaveChangesAsync(cancellationToken);

        // Compute totals per student
        var studentTotals = yearlyScores
            .GroupBy(ys => ys.StudentId)
            .Select(g => new {
                StudentId = g.Key,
                TotalScore = g.Sum(ys => ys.FinalScore ?? 0)
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
        var report = new YearlyReport {
            ClassId = submitDto.ClassId,
            SchoolYear = submitDto.SchoolYear,
            SubmittedAt = DateTime.UtcNow,
            SubmittedBy = homeroomUserId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.YearlyReports.Add(report);
        await _context.SaveChangesAsync(cancellationToken);

        // Create entries
        var entries = rankings.Select(r => new YearlyReportEntry {
            ReportId = report.Id,
            StudentId = r.StudentId,
            TotalScore = r.TotalScore,
            Rank = r.Rank,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        }).ToList();

        _context.YearlyReportEntries.AddRange(entries);
        await _context.SaveChangesAsync(cancellationToken);

        report.Entries = entries;

        return MapToDto(report);
    }

    public async Task<YearlyReportDto?> GetReportAsync(int id, CancellationToken cancellationToken = default) {
        var report = await _context.YearlyReports
            .Include(yr => yr.Entries)
            .FirstOrDefaultAsync(yr => yr.Id == id, cancellationToken);

        if (report == null) {
            return null;
        }

        return MapToDto(report);
    }

    private static YearlyReportDto MapToDto(YearlyReport report) {
        return new YearlyReportDto {
            Id = report.Id,
            ClassId = report.ClassId,
            SchoolYear = report.SchoolYear,
            CreatedAt = report.CreatedAt,
            Entries = report.Entries
                .OrderBy(e => e.Rank)
                .Select(e => new YearlyReportEntryDto {
                    StudentId = e.StudentId,
                    TotalScore = e.TotalScore,
                    Rank = e.Rank
                })
                .ToList()
        };
    }
}
