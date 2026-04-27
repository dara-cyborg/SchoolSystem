namespace SchoolSystem.Core.DTOs.Report;

public class SemesterReportEntryDto {
    public int StudentId { get; set; }
    public decimal? TotalScore { get; set; }
    public short? Rank { get; set; }
}

public class SemesterReportDto {
    public int Id { get; set; }
    public int ClassId { get; set; }
    public short Semester { get; set; }
    public short SchoolYear { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<SemesterReportEntryDto> Entries { get; set; } = new();
}
