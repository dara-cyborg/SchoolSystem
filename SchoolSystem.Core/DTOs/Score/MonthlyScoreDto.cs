namespace SchoolSystem.Core.DTOs.Score;

public class MonthlyScoreDto {
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int ClassSubjectId { get; set; }
    public short Month { get; set; }
    public short SchoolYear { get; set; }
    public decimal FinalScore { get; set; }
    public bool IsLocked { get; set; }
}
