using Microsoft.EntityFrameworkCore;
using SchoolSystem.Core.Models;

namespace SchoolSystem.Api.Data;

public class AppDbContext : DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
    }

    // DbSets for all entities
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<Grade> Grades => Set<Grade>();
    public DbSet<Class> Classes => Set<Class>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<ClassSubject> ClassSubjects => Set<ClassSubject>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<ParentStudent> ParentStudents => Set<ParentStudent>();
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<GradebookEntry> GradebookEntries => Set<GradebookEntry>();
    public DbSet<MonthlyScore> MonthlyScores => Set<MonthlyScore>();
    public DbSet<SemesterScore> SemesterScores => Set<SemesterScore>();
    public DbSet<YearlyScore> YearlyScores => Set<YearlyScore>();
    public DbSet<MonthlyReport> MonthlyReports => Set<MonthlyReport>();
    public DbSet<MonthlyReportEntry> MonthlyReportEntries => Set<MonthlyReportEntry>();
    public DbSet<SemesterReport> SemesterReports => Set<SemesterReport>();
    public DbSet<SemesterReportEntry> SemesterReportEntries => Set<SemesterReportEntry>();
    public DbSet<YearlyReport> YearlyReports => Set<YearlyReport>();
    public DbSet<YearlyReportEntry> YearlyReportEntries => Set<YearlyReportEntry>();
    public DbSet<Feedback> Feedbacks => Set<Feedback>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Sex)
                .HasConversion<string>();
        });

        // User - Role many-to-many
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Name)
                .HasConversion<string>();
        });

        // Grade - Class one-to-many
        modelBuilder.Entity<Class>()
            .HasOne(c => c.Grade)
            .WithMany(g => g.Classes)
            .HasForeignKey(c => c.GradeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Class - User (homeroom) many-to-one
        modelBuilder.Entity<Class>()
            .HasOne(c => c.HomeroomTeacher)
            .WithMany(u => u.HomeroomClasses)
            .HasForeignKey(c => c.HomeroomUserId)
            .OnDelete(DeleteBehavior.SetNull);

        // Class - ClassSubject one-to-many
        modelBuilder.Entity<ClassSubject>()
            .HasOne(cs => cs.Class)
            .WithMany(c => c.ClassSubjects)
            .HasForeignKey(cs => cs.ClassId)
            .OnDelete(DeleteBehavior.Cascade);

        // Subject - ClassSubject one-to-many
        modelBuilder.Entity<ClassSubject>()
            .HasOne(cs => cs.Subject)
            .WithMany(s => s.ClassSubjects)
            .HasForeignKey(cs => cs.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);

        // ClassSubject - User (teacher) many-to-one
        modelBuilder.Entity<ClassSubject>()
            .HasOne(cs => cs.Teacher)
            .WithMany(u => u.TeachingClassSubjects)
            .HasForeignKey(cs => cs.TeacherUserId)
            .OnDelete(DeleteBehavior.SetNull);

        // Composite unique constraint for ClassSubject
        modelBuilder.Entity<ClassSubject>()
            .HasIndex(cs => new { cs.ClassId, cs.SubjectId })
            .IsUnique();

        // Student - Class many-to-one
        modelBuilder.Entity<Student>()
            .HasOne(s => s.Class)
            .WithMany(c => c.Students)
            .HasForeignKey(s => s.ClassId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Student>(entity =>
        {
            entity.Property(e => e.Sex)
                .HasConversion<string>();
        });

        // ParentStudent many-to-many
        modelBuilder.Entity<ParentStudent>()
            .HasKey(ps => new { ps.ParentUserId, ps.StudentId });

        modelBuilder.Entity<ParentStudent>()
            .HasOne(ps => ps.Parent)
            .WithMany(u => u.ParentStudents)
            .HasForeignKey(ps => ps.ParentUserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ParentStudent>()
            .HasOne(ps => ps.Student)
            .WithMany(s => s.ParentStudents)
            .HasForeignKey(ps => ps.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Attendance - unique constraint
        modelBuilder.Entity<Attendance>()
            .HasIndex(a => new { a.StudentId, a.ClassSubjectId, a.Date })
            .IsUnique();

        modelBuilder.Entity<Attendance>()
            .HasOne(a => a.Student)
            .WithMany(s => s.Attendances)
            .HasForeignKey(a => a.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Attendance>()
            .HasOne(a => a.ClassSubject)
            .WithMany(cs => cs.Attendances)
            .HasForeignKey(a => a.ClassSubjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.Property(e => e.Status)
                .HasConversion<string>();
        });

        // GradebookEntry
        modelBuilder.Entity<GradebookEntry>()
            .HasOne(g => g.ClassSubject)
            .WithMany(cs => cs.GradebookEntries)
            .HasForeignKey(g => g.ClassSubjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GradebookEntry>()
            .HasOne(g => g.Student)
            .WithMany(s => s.GradebookEntries)
            .HasForeignKey(g => g.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        // MonthlyScore - unique constraint
        modelBuilder.Entity<MonthlyScore>()
            .HasIndex(ms => new { ms.ClassSubjectId, ms.StudentId, ms.Month, ms.SchoolYear })
            .IsUnique();

        modelBuilder.Entity<MonthlyScore>()
            .HasOne(ms => ms.ClassSubject)
            .WithMany(cs => cs.MonthlyScores)
            .HasForeignKey(ms => ms.ClassSubjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MonthlyScore>()
            .HasOne(ms => ms.Student)
            .WithMany(s => s.MonthlyScores)
            .HasForeignKey(ms => ms.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MonthlyScore>()
            .HasOne(ms => ms.SubmittedByUser)
            .WithMany(u => u.SubmittedMonthlyScores)
            .HasForeignKey(ms => ms.SubmittedBy)
            .OnDelete(DeleteBehavior.SetNull);

        // SemesterScore - unique constraint
        modelBuilder.Entity<SemesterScore>()
            .HasIndex(ss => new { ss.ClassSubjectId, ss.StudentId, ss.Semester, ss.SchoolYear })
            .IsUnique();

        modelBuilder.Entity<SemesterScore>()
            .HasOne(ss => ss.ClassSubject)
            .WithMany(cs => cs.SemesterScores)
            .HasForeignKey(ss => ss.ClassSubjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SemesterScore>()
            .HasOne(ss => ss.Student)
            .WithMany(s => s.SemesterScores)
            .HasForeignKey(ss => ss.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        // YearlyScore - unique constraint
        modelBuilder.Entity<YearlyScore>()
            .HasIndex(ys => new { ys.ClassSubjectId, ys.StudentId, ys.SchoolYear })
            .IsUnique();

        modelBuilder.Entity<YearlyScore>()
            .HasOne(ys => ys.ClassSubject)
            .WithMany(cs => cs.YearlyScores)
            .HasForeignKey(ys => ys.ClassSubjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<YearlyScore>()
            .HasOne(ys => ys.Student)
            .WithMany(s => s.YearlyScores)
            .HasForeignKey(ys => ys.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        // MonthlyReport - unique constraint
        modelBuilder.Entity<MonthlyReport>()
            .HasIndex(mr => new { mr.ClassId, mr.Month, mr.SchoolYear })
            .IsUnique();

        modelBuilder.Entity<MonthlyReport>()
            .HasOne(mr => mr.Class)
            .WithMany(c => c.MonthlyReports)
            .HasForeignKey(mr => mr.ClassId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MonthlyReport>()
            .HasOne(mr => mr.SubmittedByUser)
            .WithMany(u => u.SubmittedMonthlyReports)
            .HasForeignKey(mr => mr.SubmittedBy)
            .OnDelete(DeleteBehavior.SetNull);

        // MonthlyReportEntry - unique constraint
        modelBuilder.Entity<MonthlyReportEntry>()
            .HasIndex(mre => new { mre.ReportId, mre.StudentId })
            .IsUnique();

        modelBuilder.Entity<MonthlyReportEntry>()
            .HasOne(mre => mre.Report)
            .WithMany(r => r.Entries)
            .HasForeignKey(mre => mre.ReportId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MonthlyReportEntry>()
            .HasOne(mre => mre.Student)
            .WithMany(s => s.MonthlyReportEntries)
            .HasForeignKey(mre => mre.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        // SemesterReport - unique constraint
        modelBuilder.Entity<SemesterReport>()
            .HasIndex(sr => new { sr.ClassId, sr.Semester, sr.SchoolYear })
            .IsUnique();

        modelBuilder.Entity<SemesterReport>()
            .HasOne(sr => sr.Class)
            .WithMany(c => c.SemesterReports)
            .HasForeignKey(sr => sr.ClassId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SemesterReport>()
            .HasOne(sr => sr.SubmittedByUser)
            .WithMany(u => u.SubmittedSemesterReports)
            .HasForeignKey(sr => sr.SubmittedBy)
            .OnDelete(DeleteBehavior.SetNull);

        // SemesterReportEntry - unique constraint
        modelBuilder.Entity<SemesterReportEntry>()
            .HasIndex(sre => new { sre.ReportId, sre.StudentId })
            .IsUnique();

        modelBuilder.Entity<SemesterReportEntry>()
            .HasOne(sre => sre.Report)
            .WithMany(r => r.Entries)
            .HasForeignKey(sre => sre.ReportId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SemesterReportEntry>()
            .HasOne(sre => sre.Student)
            .WithMany(s => s.SemesterReportEntries)
            .HasForeignKey(sre => sre.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        // YearlyReport - unique constraint
        modelBuilder.Entity<YearlyReport>()
            .HasIndex(yr => new { yr.ClassId, yr.SchoolYear })
            .IsUnique();

        modelBuilder.Entity<YearlyReport>()
            .HasOne(yr => yr.Class)
            .WithMany(c => c.YearlyReports)
            .HasForeignKey(yr => yr.ClassId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<YearlyReport>()
            .HasOne(yr => yr.SubmittedByUser)
            .WithMany(u => u.SubmittedYearlyReports)
            .HasForeignKey(yr => yr.SubmittedBy)
            .OnDelete(DeleteBehavior.SetNull);

        // YearlyReportEntry - unique constraint
        modelBuilder.Entity<YearlyReportEntry>()
            .HasIndex(yre => new { yre.ReportId, yre.StudentId })
            .IsUnique();

        modelBuilder.Entity<YearlyReportEntry>()
            .HasOne(yre => yre.Report)
            .WithMany(r => r.Entries)
            .HasForeignKey(yre => yre.ReportId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<YearlyReportEntry>()
            .HasOne(yre => yre.Student)
            .WithMany(s => s.YearlyReportEntries)
            .HasForeignKey(yre => yre.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Feedback
        modelBuilder.Entity<Feedback>()
            .HasOne(f => f.MonthlyReport)
            .WithMany(mr => mr.Feedbacks)
            .HasForeignKey(f => f.MonthlyReportId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Feedback>()
            .HasOne(f => f.SemesterReport)
            .WithMany(sr => sr.Feedbacks)
            .HasForeignKey(f => f.SemesterReportId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Feedback>()
            .HasOne(f => f.YearlyReport)
            .WithMany(yr => yr.Feedbacks)
            .HasForeignKey(f => f.YearlyReportId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Feedback>()
            .HasOne(f => f.Parent)
            .WithMany(u => u.ParentFeedbacks)
            .HasForeignKey(f => f.ParentUserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.Property(e => e.ReportType)
                .HasConversion<string>();
        });

        // Grade unique constraint
        modelBuilder.Entity<Grade>()
            .HasIndex(g => g.Name)
            .IsUnique();

        // Subject unique constraint
        modelBuilder.Entity<Subject>()
            .HasIndex(s => s.Name)
            .IsUnique();

        // Class unique constraint
        modelBuilder.Entity<Class>()
            .HasIndex(c => new { c.GradeId, c.Name, c.SchoolYear })
            .IsUnique();
    }
}