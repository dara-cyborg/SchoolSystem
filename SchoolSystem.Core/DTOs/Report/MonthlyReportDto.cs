namespace SchoolSystem.Core.DTOs.Report;

public class MonthlyReportEntryDto {
    public int StudentId { get; set; }
    public decimal? TotalScore { get; set; }
    public short? Rank { get; set; }
}

public class MonthlyReportDto {
    public int Id { get; set; }
    public int ClassId { get; set; }
    public short Month { get; set; }
    public short SchoolYear { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<MonthlyReportEntryDto> Entries { get; set; } = new();
}
