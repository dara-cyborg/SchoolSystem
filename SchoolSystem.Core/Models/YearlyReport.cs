namespace SchoolSystem.Core.Models;

public class YearlyReport : BaseEntity {
    public int ClassId { get; set; }
    public short SchoolYear { get; set; }
    public int? SubmittedBy { get; set; }
    public DateTime? SubmittedAt { get; set; }

    // Navigation properties
    public Class Class { get; set; } = null!;
    public User? SubmittedByUser { get; set; }
    public ICollection<YearlyReportEntry> Entries { get; set; } = new List<YearlyReportEntry>();
    public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
}