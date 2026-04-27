namespace SchoolSystem.Core.DTOs.Gradebook;

public class UpdateGradebookEntryDto {
    public string Label { get; set; } = string.Empty;
    public decimal Score { get; set; }
    public decimal MaxScore { get; set; }
}
