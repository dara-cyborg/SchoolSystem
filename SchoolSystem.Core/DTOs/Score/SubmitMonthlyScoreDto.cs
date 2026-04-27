namespace SchoolSystem.Core.DTOs.Score;

public class SubmitMonthlyScoreDto {
    public int StudentId { get; set; }
    public int ClassSubjectId { get; set; }
    public short Month { get; set; }
    public short SchoolYear { get; set; }
    public decimal FinalScore { get; set; }
}
