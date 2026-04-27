namespace SchoolSystem.Core.DTOs.Gradebook;

public class CreateGradebookEntryDto {
    public int StudentId { get; set; }
    public int ClassSubjectId { get; set; }
    public string Label { get; set; } = string.Empty;
    public decimal Score { get; set; }
    public decimal MaxScore { get; set; }
}
