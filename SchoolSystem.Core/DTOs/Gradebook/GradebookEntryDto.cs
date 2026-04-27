namespace SchoolSystem.Core.DTOs.Gradebook;

public class GradebookEntryDto {
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int ClassSubjectId { get; set; }
    public string Label { get; set; } = string.Empty;
    public decimal Score { get; set; }
    public decimal MaxScore { get; set; }
    public DateTime CreatedAt { get; set; }
}
