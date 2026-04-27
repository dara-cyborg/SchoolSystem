namespace SchoolSystem.Core.DTOs.Report;

public class CreateMonthlyReportDto {
    public int ClassId { get; set; }
    public short Month { get; set; }
    public short SchoolYear { get; set; }
}

public class MonthlyReportResponseDto {
    public int Id { get; set; }
    public int ClassId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public short Month { get; set; }
    public short SchoolYear { get; set; }
    public int? SubmittedBy { get; set; }
    public string? SubmittedByName { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<MonthlyReportEntryDto> Entries { get; set; } = new();
}

public class CreateSemesterReportDto {
    public int ClassId { get; set; }
    public short Semester { get; set; }
    public short SchoolYear { get; set; }
}

public class SemesterReportResponseDto {
    public int Id { get; set; }
    public int ClassId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public short Semester { get; set; }
    public short SchoolYear { get; set; }
    public int? SubmittedBy { get; set; }
    public string? SubmittedByName { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<SemesterReportEntryDto> Entries { get; set; } = new();
}


public class CreateYearlyReportDto {
    public int ClassId { get; set; }
    public short SchoolYear { get; set; }
}

public class YearlyReportResponseDto {
    public int Id { get; set; }
    public int ClassId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public short SchoolYear { get; set; }
    public int? SubmittedBy { get; set; }
    public string? SubmittedByName { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<YearlyReportEntryDto> Entries { get; set; } = new();
}


public class CreateFeedbackDto {
    public string ReportType { get; set; } = string.Empty;
    public int? MonthlyReportId { get; set; }
    public int? SemesterReportId { get; set; }
    public int? YearlyReportId { get; set; }
    public string Content { get; set; } = string.Empty;
}

public class FeedbackResponseDto {
    public int Id { get; set; }
    public string ReportType { get; set; } = string.Empty;
    public int? MonthlyReportId { get; set; }
    public int? SemesterReportId { get; set; }
    public int? YearlyReportId { get; set; }
    public int ParentUserId { get; set; }
    public string ParentName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}