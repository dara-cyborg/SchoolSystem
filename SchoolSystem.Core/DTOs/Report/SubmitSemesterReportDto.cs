namespace SchoolSystem.Core.DTOs.Report;

public class SubmitSemesterReportDto {
    public int ClassId { get; set; }
    public short Semester { get; set; }
    public short SchoolYear { get; set; }
}
