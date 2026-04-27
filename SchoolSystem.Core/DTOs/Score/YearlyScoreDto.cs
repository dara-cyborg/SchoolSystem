namespace SchoolSystem.Core.DTOs.Score;

public class YearlyScoreDto {
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int ClassSubjectId { get; set; }
    public short SchoolYear { get; set; }
    public decimal? AverageScore { get; set; }
}
