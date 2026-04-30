namespace SchoolSystem.Web.Models.ViewModels;

public class SubjectScoreViewModel
{
    public string SubjectName { get; set; } = string.Empty;
    public decimal FinalScore { get; set; }
}

public class EntryViewModel
{
    public int StudentId { get; set; }
    public int TotalScore { get; set; }
    public int Rank { get; set; }
}
