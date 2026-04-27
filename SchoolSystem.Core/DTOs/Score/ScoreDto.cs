namespace SchoolSystem.Core.DTOs.Score;

public class CreateGradebookEntryDto {
    public int ClassSubjectId { get; set; }
    public int StudentId { get; set; }
    public string Label { get; set; } = string.Empty;
    public decimal MaxScore { get; set; }
    public decimal Score { get; set; }
    public DateTime EntryDate { get; set; }
}

public class UpdateGradebookEntryDto {
    public string Label { get; set; } = string.Empty;
    public decimal MaxScore { get; set; }
    public decimal Score { get; set; }
    public DateTime EntryDate { get; set; }
}

public class GradebookEntryResponseDto {
    public int Id { get; set; }
    public int ClassSubjectId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public decimal MaxScore { get; set; }
    public decimal Score { get; set; }
    public DateTime EntryDate { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateMonthlyScoreDto {
    public int ClassSubjectId { get; set; }
    public int StudentId { get; set; }
    public short Month { get; set; }
    public short SchoolYear { get; set; }
    public decimal FinalScore { get; set; }
}

public class UpdateMonthlyScoreDto {
    public decimal FinalScore { get; set; }
    public bool IsLocked { get; set; }
}

public class MonthlyScoreResponseDto {
    public int Id { get; set; }
    public int ClassSubjectId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public short Month { get; set; }
    public short SchoolYear { get; set; }
    public decimal FinalScore { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public bool IsLocked { get; set; }
}

public class CreateSemesterScoreDto {
    public int ClassSubjectId { get; set; }
    public int StudentId { get; set; }
    public short Semester { get; set; }
    public short SchoolYear { get; set; }
    public decimal? FinalScore { get; set; }
}

public class SemesterScoreResponseDto {
    public int Id { get; set; }
    public int ClassSubjectId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public short Semester { get; set; }
    public short SchoolYear { get; set; }
    public decimal? FinalScore { get; set; }
}

public class CreateYearlyScoreDto {
    public int ClassSubjectId { get; set; }
    public int StudentId { get; set; }
    public short SchoolYear { get; set; }
    public decimal? FinalScore { get; set; }
}

public class YearlyScoreResponseDto {
    public int Id { get; set; }
    public int ClassSubjectId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public short SchoolYear { get; set; }
    public decimal? FinalScore { get; set; }
}