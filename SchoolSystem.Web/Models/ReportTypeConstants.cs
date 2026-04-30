namespace SchoolSystem.Web.Models;

public static class ReportTypeConstants
{
    public const string Monthly = "monthly";
    public const string Semester = "semester";
    public const string Yearly = "yearly";

    public const string MonthlyEndpoint = "monthly-reports";
    public const string SemesterEndpoint = "semester-reports";
    public const string YearlyEndpoint = "yearly-reports";

    public static string GetEndpoint(string reportType) => reportType.ToLower() switch
    {
        Monthly => MonthlyEndpoint,
        Semester => SemesterEndpoint,
        Yearly => YearlyEndpoint,
        _ => string.Empty
    };
}
