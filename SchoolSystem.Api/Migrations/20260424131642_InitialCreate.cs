using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SchoolSystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Sex = table.Column<int>(type: "integer", nullable: false),
                    Dob = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Contact = table.Column<string>(type: "text", nullable: true),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GradeId = table.Column<int>(type: "integer", nullable: false),
                    SchoolYear = table.Column<short>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    HomeroomUserId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classes_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Classes_Users_HomeroomUserId",
                        column: x => x.HomeroomUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassSubjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    SubjectId = table.Column<int>(type: "integer", nullable: false),
                    TeacherUserId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassSubjects_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassSubjects_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassSubjects_Users_TeacherUserId",
                        column: x => x.TeacherUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    Month = table.Column<short>(type: "smallint", nullable: false),
                    SchoolYear = table.Column<short>(type: "smallint", nullable: false),
                    SubmittedBy = table.Column<int>(type: "integer", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthlyReports_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyReports_Users_SubmittedBy",
                        column: x => x.SubmittedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "SemesterReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    Semester = table.Column<short>(type: "smallint", nullable: false),
                    SchoolYear = table.Column<short>(type: "smallint", nullable: false),
                    SubmittedBy = table.Column<int>(type: "integer", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SemesterReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SemesterReports_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SemesterReports_Users_SubmittedBy",
                        column: x => x.SubmittedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Sex = table.Column<int>(type: "integer", nullable: false),
                    Dob = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Contact = table.Column<string>(type: "text", nullable: true),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "YearlyReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    SchoolYear = table.Column<short>(type: "smallint", nullable: false),
                    SubmittedBy = table.Column<int>(type: "integer", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YearlyReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YearlyReports_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_YearlyReports_Users_SubmittedBy",
                        column: x => x.SubmittedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    ClassSubjectId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attendances_ClassSubjects_ClassSubjectId",
                        column: x => x.ClassSubjectId,
                        principalTable: "ClassSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attendances_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GradebookEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClassSubjectId = table.Column<int>(type: "integer", nullable: false),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    Label = table.Column<string>(type: "text", nullable: false),
                    MaxScore = table.Column<decimal>(type: "numeric", nullable: false),
                    Score = table.Column<decimal>(type: "numeric", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradebookEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GradebookEntries_ClassSubjects_ClassSubjectId",
                        column: x => x.ClassSubjectId,
                        principalTable: "ClassSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradebookEntries_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyReportEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReportId = table.Column<int>(type: "integer", nullable: false),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    TotalScore = table.Column<decimal>(type: "numeric", nullable: true),
                    Rank = table.Column<short>(type: "smallint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyReportEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthlyReportEntries_MonthlyReports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "MonthlyReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyReportEntries_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClassSubjectId = table.Column<int>(type: "integer", nullable: false),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    Month = table.Column<short>(type: "smallint", nullable: false),
                    SchoolYear = table.Column<short>(type: "smallint", nullable: false),
                    FinalScore = table.Column<decimal>(type: "numeric", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SubmittedBy = table.Column<int>(type: "integer", nullable: true),
                    IsLocked = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthlyScores_ClassSubjects_ClassSubjectId",
                        column: x => x.ClassSubjectId,
                        principalTable: "ClassSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyScores_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyScores_Users_SubmittedBy",
                        column: x => x.SubmittedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ParentStudents",
                columns: table => new
                {
                    ParentUserId = table.Column<int>(type: "integer", nullable: false),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentStudents", x => new { x.ParentUserId, x.StudentId });
                    table.ForeignKey(
                        name: "FK_ParentStudents_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentStudents_Users_ParentUserId",
                        column: x => x.ParentUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SemesterReportEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReportId = table.Column<int>(type: "integer", nullable: false),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    TotalScore = table.Column<decimal>(type: "numeric", nullable: true),
                    Rank = table.Column<short>(type: "smallint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SemesterReportEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SemesterReportEntries_SemesterReports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "SemesterReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SemesterReportEntries_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SemesterScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClassSubjectId = table.Column<int>(type: "integer", nullable: false),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    Semester = table.Column<short>(type: "smallint", nullable: false),
                    SchoolYear = table.Column<short>(type: "smallint", nullable: false),
                    FinalScore = table.Column<decimal>(type: "numeric", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SemesterScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SemesterScores_ClassSubjects_ClassSubjectId",
                        column: x => x.ClassSubjectId,
                        principalTable: "ClassSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SemesterScores_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "YearlyScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClassSubjectId = table.Column<int>(type: "integer", nullable: false),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    SchoolYear = table.Column<short>(type: "smallint", nullable: false),
                    FinalScore = table.Column<decimal>(type: "numeric", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YearlyScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YearlyScores_ClassSubjects_ClassSubjectId",
                        column: x => x.ClassSubjectId,
                        principalTable: "ClassSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_YearlyScores_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReportType = table.Column<int>(type: "integer", nullable: false),
                    MonthlyReportId = table.Column<int>(type: "integer", nullable: true),
                    SemesterReportId = table.Column<int>(type: "integer", nullable: true),
                    YearlyReportId = table.Column<int>(type: "integer", nullable: true),
                    ParentUserId = table.Column<int>(type: "integer", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Feedbacks_MonthlyReports_MonthlyReportId",
                        column: x => x.MonthlyReportId,
                        principalTable: "MonthlyReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Feedbacks_SemesterReports_SemesterReportId",
                        column: x => x.SemesterReportId,
                        principalTable: "SemesterReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Users_ParentUserId",
                        column: x => x.ParentUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Feedbacks_YearlyReports_YearlyReportId",
                        column: x => x.YearlyReportId,
                        principalTable: "YearlyReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "YearlyReportEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReportId = table.Column<int>(type: "integer", nullable: false),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    TotalScore = table.Column<decimal>(type: "numeric", nullable: true),
                    Rank = table.Column<short>(type: "smallint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YearlyReportEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YearlyReportEntries_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_YearlyReportEntries_YearlyReports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "YearlyReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_ClassSubjectId",
                table: "Attendances",
                column: "ClassSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_StudentId_ClassSubjectId_Date",
                table: "Attendances",
                columns: new[] { "StudentId", "ClassSubjectId", "Date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_GradeId_Name_SchoolYear",
                table: "Classes",
                columns: new[] { "GradeId", "Name", "SchoolYear" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_HomeroomUserId",
                table: "Classes",
                column: "HomeroomUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSubjects_ClassId_SubjectId",
                table: "ClassSubjects",
                columns: new[] { "ClassId", "SubjectId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassSubjects_SubjectId",
                table: "ClassSubjects",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSubjects_TeacherUserId",
                table: "ClassSubjects",
                column: "TeacherUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_MonthlyReportId",
                table: "Feedbacks",
                column: "MonthlyReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_ParentUserId",
                table: "Feedbacks",
                column: "ParentUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_SemesterReportId",
                table: "Feedbacks",
                column: "SemesterReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_YearlyReportId",
                table: "Feedbacks",
                column: "YearlyReportId");

            migrationBuilder.CreateIndex(
                name: "IX_GradebookEntries_ClassSubjectId",
                table: "GradebookEntries",
                column: "ClassSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_GradebookEntries_StudentId",
                table: "GradebookEntries",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_Name",
                table: "Grades",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyReportEntries_ReportId_StudentId",
                table: "MonthlyReportEntries",
                columns: new[] { "ReportId", "StudentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyReportEntries_StudentId",
                table: "MonthlyReportEntries",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyReports_ClassId_Month_SchoolYear",
                table: "MonthlyReports",
                columns: new[] { "ClassId", "Month", "SchoolYear" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyReports_SubmittedBy",
                table: "MonthlyReports",
                column: "SubmittedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyScores_ClassSubjectId_StudentId_Month_SchoolYear",
                table: "MonthlyScores",
                columns: new[] { "ClassSubjectId", "StudentId", "Month", "SchoolYear" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyScores_StudentId",
                table: "MonthlyScores",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyScores_SubmittedBy",
                table: "MonthlyScores",
                column: "SubmittedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ParentStudents_StudentId",
                table: "ParentStudents",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_SemesterReportEntries_ReportId_StudentId",
                table: "SemesterReportEntries",
                columns: new[] { "ReportId", "StudentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SemesterReportEntries_StudentId",
                table: "SemesterReportEntries",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_SemesterReports_ClassId_Semester_SchoolYear",
                table: "SemesterReports",
                columns: new[] { "ClassId", "Semester", "SchoolYear" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SemesterReports_SubmittedBy",
                table: "SemesterReports",
                column: "SubmittedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SemesterScores_ClassSubjectId_StudentId_Semester_SchoolYear",
                table: "SemesterScores",
                columns: new[] { "ClassSubjectId", "StudentId", "Semester", "SchoolYear" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SemesterScores_StudentId",
                table: "SemesterScores",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_ClassId",
                table: "Students",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_Name",
                table: "Subjects",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_YearlyReportEntries_ReportId_StudentId",
                table: "YearlyReportEntries",
                columns: new[] { "ReportId", "StudentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_YearlyReportEntries_StudentId",
                table: "YearlyReportEntries",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_YearlyReports_ClassId_SchoolYear",
                table: "YearlyReports",
                columns: new[] { "ClassId", "SchoolYear" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_YearlyReports_SubmittedBy",
                table: "YearlyReports",
                column: "SubmittedBy");

            migrationBuilder.CreateIndex(
                name: "IX_YearlyScores_ClassSubjectId_StudentId_SchoolYear",
                table: "YearlyScores",
                columns: new[] { "ClassSubjectId", "StudentId", "SchoolYear" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_YearlyScores_StudentId",
                table: "YearlyScores",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "GradebookEntries");

            migrationBuilder.DropTable(
                name: "MonthlyReportEntries");

            migrationBuilder.DropTable(
                name: "MonthlyScores");

            migrationBuilder.DropTable(
                name: "ParentStudents");

            migrationBuilder.DropTable(
                name: "SemesterReportEntries");

            migrationBuilder.DropTable(
                name: "SemesterScores");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "YearlyReportEntries");

            migrationBuilder.DropTable(
                name: "YearlyScores");

            migrationBuilder.DropTable(
                name: "MonthlyReports");

            migrationBuilder.DropTable(
                name: "SemesterReports");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "YearlyReports");

            migrationBuilder.DropTable(
                name: "ClassSubjects");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
