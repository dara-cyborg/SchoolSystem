using Microsoft.EntityFrameworkCore;
using SchoolSystem.Api.Data;
using SchoolSystem.Core.DTOs.Feedback;
using SchoolSystem.Core.Enums;
using SchoolSystem.Core.Interfaces;
using SchoolSystem.Core.Models;

namespace SchoolSystem.Api.Services;

public class FeedbackService : IFeedbackService {
    private readonly AppDbContext _context;

    public FeedbackService(AppDbContext context) {
        _context = context;
    }

    public async Task<FeedbackDto> CreateAsync(CreateFeedbackDto createDto, int parentUserId, CancellationToken cancellationToken = default) {
        // Validate report exists and get class ID based on ReportType
        int? classId = null;

        if (createDto.ReportType == ReportType.Monthly) {
            var report = await _context.MonthlyReports
                .FirstOrDefaultAsync(mr => mr.Id == createDto.ReportId, cancellationToken);

            if (report == null) {
                throw new InvalidOperationException("Report not found.");
            }
            classId = report.ClassId;
        } else if (createDto.ReportType == ReportType.Semester) {
            var report = await _context.SemesterReports
                .FirstOrDefaultAsync(sr => sr.Id == createDto.ReportId, cancellationToken);

            if (report == null) {
                throw new InvalidOperationException("Report not found.");
            }
            classId = report.ClassId;
        } else if (createDto.ReportType == ReportType.Yearly) {
            var report = await _context.YearlyReports
                .FirstOrDefaultAsync(yr => yr.Id == createDto.ReportId, cancellationToken);

            if (report == null) {
                throw new InvalidOperationException("Report not found.");
            }
            classId = report.ClassId;
        }

        // Validate parent ownership: report's class must contain a student linked to this parent
        var hasAccessToClass = await _context.ParentStudents
            .Include(ps => ps.Student)
            .AnyAsync(ps => 
                ps.ParentUserId == parentUserId &&
                ps.Student.ClassId == classId,
                cancellationToken);

        if (!hasAccessToClass) {
            throw new InvalidOperationException("You are not authorized to submit feedback for this report.");
        }

        // Create feedback with correct FK set
        var feedback = new Feedback {
            ParentUserId = parentUserId,
            ReportType = createDto.ReportType,
            MonthlyReportId = createDto.ReportType == ReportType.Monthly ? createDto.ReportId : null,
            SemesterReportId = createDto.ReportType == ReportType.Semester ? createDto.ReportId : null,
            YearlyReportId = createDto.ReportType == ReportType.Yearly ? createDto.ReportId : null,
            Content = createDto.Message,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Feedbacks.Add(feedback);
        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(feedback);
    }

    public async Task<List<FeedbackDto>> GetByReportAsync(ReportType reportType, int reportId, int? parentUserId = null, CancellationToken cancellationToken = default) {
        // If parentUserId provided, validate parent access
        if (parentUserId.HasValue) {
            int? classId = null;

            if (reportType == ReportType.Monthly) {
                var report = await _context.MonthlyReports
                    .FirstOrDefaultAsync(mr => mr.Id == reportId, cancellationToken);
                if (report != null) classId = report.ClassId;
            } else if (reportType == ReportType.Semester) {
                var report = await _context.SemesterReports
                    .FirstOrDefaultAsync(sr => sr.Id == reportId, cancellationToken);
                if (report != null) classId = report.ClassId;
            } else if (reportType == ReportType.Yearly) {
                var report = await _context.YearlyReports
                    .FirstOrDefaultAsync(yr => yr.Id == reportId, cancellationToken);
                if (report != null) classId = report.ClassId;
            }

            if (classId.HasValue) {
                var hasAccessToClass = await _context.ParentStudents
                    .Include(ps => ps.Student)
                    .AnyAsync(ps => 
                        ps.ParentUserId == parentUserId.Value &&
                        ps.Student.ClassId == classId.Value,
                        cancellationToken);

                if (!hasAccessToClass) {
                    return new List<FeedbackDto>();
                }
            }
        }

        // Get feedback based on report type
        List<Feedback> feedbacks;

        if (reportType == ReportType.Monthly) {
            feedbacks = await _context.Feedbacks
                .Where(f => f.ReportType == ReportType.Monthly && f.MonthlyReportId == reportId)
                .OrderBy(f => f.Id)
                .ToListAsync(cancellationToken);
        } else if (reportType == ReportType.Semester) {
            feedbacks = await _context.Feedbacks
                .Where(f => f.ReportType == ReportType.Semester && f.SemesterReportId == reportId)
                .OrderBy(f => f.Id)
                .ToListAsync(cancellationToken);
        } else {
            feedbacks = await _context.Feedbacks
                .Where(f => f.ReportType == ReportType.Yearly && f.YearlyReportId == reportId)
                .OrderBy(f => f.Id)
                .ToListAsync(cancellationToken);
        }

        return feedbacks.Select(MapToDto).ToList();
    }

    public async Task<List<FeedbackDto>> GetByClassAsync(int classId, CancellationToken cancellationToken = default) {
        // Get all class IDs (in case needed for future hierarchy)
        var classExists = await _context.Classes
            .AnyAsync(c => c.Id == classId, cancellationToken);

        if (!classExists) {
            return new List<FeedbackDto>();
        }

        // Get all monthly reports for this class
        var monthlyReportIds = await _context.MonthlyReports
            .Where(mr => mr.ClassId == classId)
            .Select(mr => mr.Id)
            .ToListAsync(cancellationToken);

        // Get all semester reports for this class
        var semesterReportIds = await _context.SemesterReports
            .Where(sr => sr.ClassId == classId)
            .Select(sr => sr.Id)
            .ToListAsync(cancellationToken);

        // Get all yearly reports for this class
        var yearlyReportIds = await _context.YearlyReports
            .Where(yr => yr.ClassId == classId)
            .Select(yr => yr.Id)
            .ToListAsync(cancellationToken);

        // Get all feedback across all report types for this class
        var feedbacks = await _context.Feedbacks
            .Where(f => 
                (f.ReportType == ReportType.Monthly && f.MonthlyReportId.HasValue && monthlyReportIds.Contains(f.MonthlyReportId.Value)) ||
                (f.ReportType == ReportType.Semester && f.SemesterReportId.HasValue && semesterReportIds.Contains(f.SemesterReportId.Value)) ||
                (f.ReportType == ReportType.Yearly && f.YearlyReportId.HasValue && yearlyReportIds.Contains(f.YearlyReportId.Value)))
            .OrderBy(f => f.Id)
            .ToListAsync(cancellationToken);

        return feedbacks.Select(MapToDto).ToList();
    }

    private static FeedbackDto MapToDto(Feedback feedback) {
        return new FeedbackDto {
            Id = feedback.Id,
            ParentUserId = feedback.ParentUserId,
            ReportType = feedback.ReportType,
            MonthlyReportId = feedback.MonthlyReportId,
            SemesterReportId = feedback.SemesterReportId,
            YearlyReportId = feedback.YearlyReportId,
            Message = feedback.Content,
            CreatedAt = feedback.CreatedAt
        };
    }
}
