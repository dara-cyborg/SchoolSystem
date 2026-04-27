namespace SchoolSystem.Core.DTOs.Report;

public class YearlyReportEntryDto {
    public int StudentId { get; set; }
    public decimal? TotalScore { get; set; }
    public short? Rank { get; set; }
}

public class YearlyReportDto {
    public int Id { get; set; }
    public int ClassId { get; set; }
    public short SchoolYear { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<YearlyReportEntryDto> Entries { get; set; } = new();
}
