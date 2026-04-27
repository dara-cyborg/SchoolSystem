using Microsoft.EntityFrameworkCore;
using SchoolSystem.Api.Data;
using SchoolSystem.Core.DTOs.Report;
using SchoolSystem.Core.Interfaces;
using SchoolSystem.Core.Models;

namespace SchoolSystem.Api.Services;

public class SemesterReportService : ISemesterReportService {
    private readonly AppDbContext _context;

    public SemesterReportService(AppDbContext context) {
        _context = context;
    }

    public async Task<SemesterReportDto> SubmitReportAsync(SubmitSemesterReportDto submitDto, int homeroomUserId, CancellationToken cancellationToken = default) {
        var existingReport = await _context.SemesterReports
            .FirstOrDefaultAsync(sr => 
                sr.ClassId == submitDto.ClassId &&
                sr.Semester == submitDto.Semester &&
                sr.SchoolYear == submitDto.SchoolYear,
                cancellationToken);

        if (existingReport != null) {
            throw new InvalidOperationException($"A report already exists for Class {submitDto.ClassId}, Semester {submitDto.Semester}, School Year {submitDto.SchoolYear}.");
        }

        var classExists = await _context.Classes
            .AnyAsync(c => c.Id == submitDto.ClassId, cancellationToken);

        if (!classExists) {
            throw new InvalidOperationException($"Class with ID {submitDto.ClassId} does not exist.");
        }

        // Determine month range for semester
        var (startMonth, endMonth) = submitDto.Semester == 1 
            ? ((short)1, (short)6) 
            : ((short)7, (short)12);

        // Get all class subjects for this class
        var classSubjects = await _context.ClassSubjects
            .Where(cs => cs.ClassId == submitDto.ClassId)
            .Select(cs => cs.Id)
            .ToListAsync(cancellationToken);

        // Get all locked monthly scores for this semester
        var monthlyScores = await _context.MonthlyScores
            .Where(ms => 
                classSubjects.Contains(ms.ClassSubjectId) &&
                ms.Month >= startMonth &&
                ms.Month <= endMonth &&
                ms.SchoolYear == submitDto.SchoolYear &&
                ms.IsLocked)
            .ToListAsync(cancellationToken);

        // Get all students in this class
        var studentIds = await _context.Students
            .Where(s => s.ClassId == submitDto.ClassId)
            .Select(s => s.Id)
            .ToListAsync(cancellationToken);

        // Verify all required monthly scores are present
        var expectedScoreCount = studentIds.Count * classSubjects.Count * 6; // 6 months per semester
        var actualScoreCount = monthlyScores.Count;

        if (actualScoreCount < expectedScoreCount) {
            throw new InvalidOperationException("Not all monthly scores are submitted and locked for this semester.");
        }

        // Aggregate monthly scores into semester scores
        var semesterScoreMap = new Dictionary<(int studentId, int classSubjectId), List<decimal>>();

        foreach (var score in monthlyScores) {
            var key = (score.StudentId, score.ClassSubjectId);
            if (!semesterScoreMap.ContainsKey(key)) {
                semesterScoreMap[key] = new List<decimal>();
            }
            semesterScoreMap[key].Add(score.FinalScore);
        }

        var semesterScores = new List<SemesterScore>();
        foreach (var kvp in semesterScoreMap) {
            var (studentId, classSubjectId) = kvp.Key;
            var averageScore = kvp.Value.Count > 0 ? kvp.Value.Average() : (decimal?)null;

            var semesterScore = new SemesterScore {
                StudentId = studentId,
                ClassSubjectId = classSubjectId,
                Semester = submitDto.Semester,
                SchoolYear = submitDto.SchoolYear,
                FinalScore = averageScore,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            semesterScores.Add(semesterScore);
        }

        _context.SemesterScores.AddRange(semesterScores);
        await _context.SaveChangesAsync(cancellationToken);

        // Compute totals per student
        var studentTotals = semesterScores
            .GroupBy(ss => ss.StudentId)
            .Select(g => new {
                StudentId = g.Key,
                TotalScore = g.Sum(ss => ss.FinalScore ?? 0)
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
        var report = new SemesterReport {
            ClassId = submitDto.ClassId,
            Semester = submitDto.Semester,
            SchoolYear = submitDto.SchoolYear,
            SubmittedAt = DateTime.UtcNow,
            SubmittedBy = homeroomUserId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.SemesterReports.Add(report);
        await _context.SaveChangesAsync(cancellationToken);

        // Create entries
        var entries = rankings.Select(r => new SemesterReportEntry {
            ReportId = report.Id,
            StudentId = r.StudentId,
            TotalScore = r.TotalScore,
            Rank = r.Rank,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        }).ToList();

        _context.SemesterReportEntries.AddRange(entries);
        await _context.SaveChangesAsync(cancellationToken);

        report.Entries = entries;

        return MapToDto(report);
    }

    public async Task<SemesterReportDto?> GetReportAsync(int id, CancellationToken cancellationToken = default) {
        var report = await _context.SemesterReports
            .Include(sr => sr.Entries)
            .FirstOrDefaultAsync(sr => sr.Id == id, cancellationToken);

        if (report == null) {
            return null;
        }

        return MapToDto(report);
    }

    private static SemesterReportDto MapToDto(SemesterReport report) {
        return new SemesterReportDto {
            Id = report.Id,
            ClassId = report.ClassId,
            Semester = report.Semester,
            SchoolYear = report.SchoolYear,
            CreatedAt = report.CreatedAt,
            Entries = report.Entries
                .OrderBy(e => e.Rank)
                .Select(e => new SemesterReportEntryDto {
                    StudentId = e.StudentId,
                    TotalScore = e.TotalScore,
                    Rank = e.Rank
                })
                .ToList()
        };
    }
}
