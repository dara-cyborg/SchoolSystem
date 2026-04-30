-- Web Portal Test Seed
-- Parent login: TestParent / password123
-- Student: TestStudent (Id: 900)
-- Reports: Monthly (Id:900), Semester (Id:900), Yearly (Id:900)
-- Run AFTER seed_schoolsystem_2025.sql

-- Note: statements run individually to avoid aborting the whole script on a single error

-- Users
INSERT INTO "Users" ("Id", "Name", "Sex", "Dob", "Contact", "PasswordHash", "IsActive", "CreatedAt", "UpdatedAt")
VALUES
    (900, 'TestParent', 'Male', NULL, 'testparent@test.com', '$2a$12$4Tbyfz2s9nzUJ3VEqhSj/OuLRfU3APIMCyAT9W64zEg9qXChU135i', TRUE, NOW(), NOW()),
    (901, 'TestTeacher', 'Female', NULL, 'testteacher@test.com', '$2a$12$4Tbyfz2s9nzUJ3VEqhSj/OuLRfU3APIMCyAT9W64zEg9qXChU135i', TRUE, NOW(), NOW()),
    (902, 'TestHomeroom', 'Male', NULL, 'testhomeroom@test.com', '$2a$12$4Tbyfz2s9nzUJ3VEqhSj/OuLRfU3APIMCyAT9W64zEg9qXChU135i', TRUE, NOW(), NOW())
ON CONFLICT DO NOTHING;

-- UserRoles (use RoleIds from existing seed: Teacher=2, Homeroom=3, Parent=4)
INSERT INTO "UserRoles" ("UserId", "RoleId") VALUES (900, 4) ON CONFLICT ("UserId", "RoleId") DO NOTHING;
INSERT INTO "UserRoles" ("UserId", "RoleId") VALUES (901, 2) ON CONFLICT ("UserId", "RoleId") DO NOTHING;
INSERT INTO "UserRoles" ("UserId", "RoleId") VALUES (902, 3) ON CONFLICT ("UserId", "RoleId") DO NOTHING;

-- Grade
INSERT INTO "Grades" ("Id", "Name", "CreatedAt", "UpdatedAt")
VALUES (900, 'Test Grade', NOW(), NOW())
ON CONFLICT DO NOTHING;

-- Class (use GradeId by name to avoid FK issues if grade exists with different Id)
INSERT INTO "Classes" ("Id", "GradeId", "SchoolYear", "Name", "HomeroomUserId", "CreatedAt", "UpdatedAt")
VALUES (900, COALESCE((SELECT "Id" FROM "Grades" WHERE "Name" = 'Test Grade'), 900), 2025, 'Test Class 9A', 902, NOW(), NOW())
ON CONFLICT DO NOTHING;

-- Subjects
INSERT INTO "Subjects" ("Id", "Name", "CreatedAt", "UpdatedAt")
VALUES
    (900, 'Mathematics', NOW(), NOW()),
    (901, 'Science', NOW(), NOW()),
    (902, 'Khmer', NOW(), NOW())
ON CONFLICT DO NOTHING;

INSERT INTO "ClassSubjects" ("Id", "ClassId", "SubjectId", "TeacherUserId", "CreatedAt", "UpdatedAt")
VALUES
    (900, 900, COALESCE((SELECT "Id" FROM "Subjects" WHERE "Name" = 'Mathematics'), 900), 901, NOW(), NOW()),
    (901, 900, COALESCE((SELECT "Id" FROM "Subjects" WHERE "Name" = 'Science'), 901), 901, NOW(), NOW()),
    (902, 900, COALESCE((SELECT "Id" FROM "Subjects" WHERE "Name" = 'Khmer'), 902), 901, NOW(), NOW())
ON CONFLICT DO NOTHING;

-- Student
INSERT INTO "Students" ("Id", "Name", "Sex", "Dob", "Contact", "ClassId", "CreatedAt", "UpdatedAt")
VALUES
    (900, 'TestStudent', 'Female', '2010-05-15 00:00:00+00', NULL, 900, NOW(), NOW())
ON CONFLICT DO NOTHING;

-- ParentStudent
INSERT INTO "ParentStudents" ("Id", "ParentUserId", "StudentId", "CreatedAt", "UpdatedAt")
VALUES (900, 900, 900, NOW(), NOW())
ON CONFLICT ("ParentUserId", "StudentId") DO NOTHING;

-- Attendance records for month 1, 2025
INSERT INTO "Attendances" ("StudentId", "ClassSubjectId", "Date", "Status", "CreatedAt", "UpdatedAt")
VALUES
    (900, 900, '2025-01-06 00:00:00+00', 'Present', NOW(), NOW()),
    (900, 900, '2025-01-07 00:00:00+00', 'InformedAbsent', NOW(), NOW()),
    (900, 900, '2025-01-08 00:00:00+00', 'Present', NOW(), NOW()),
    (900, 900, '2025-01-09 00:00:00+00', 'UninformedAbsent', NOW(), NOW()),
    (900, 900, '2025-01-10 00:00:00+00', 'Present', NOW(), NOW())
ON CONFLICT ("StudentId", "ClassSubjectId", "Date") DO NOTHING;

-- Gradebook Entries
INSERT INTO "GradebookEntries" ("ClassSubjectId", "StudentId", "Label", "MaxScore", "Score", "EntryDate", "CreatedAt", "UpdatedAt")
VALUES
    (900, 900, 'Quiz 1', 10.0, 8.5, '2025-01-06 08:00:00+00', NOW(), NOW()),
    (901, 900, 'Quiz 1', 10.0, 9.0, '2025-01-06 08:00:00+00', NOW(), NOW()),
    (902, 900, 'Quiz 1', 10.0, 7.5, '2025-01-06 08:00:00+00', NOW(), NOW())
ON CONFLICT DO NOTHING;

-- MonthlyScores (Month 1, SchoolYear 2025) - locked
INSERT INTO "MonthlyScores" ("ClassSubjectId", "StudentId", "Month", "SchoolYear", "FinalScore", "SubmittedAt", "SubmittedBy", "IsLocked", "CreatedAt", "UpdatedAt")
VALUES
    (900, 900, 1, 2025, 85.0, NOW(), 901, TRUE, NOW(), NOW()),
    (901, 900, 1, 2025, 90.0, NOW(), 901, TRUE, NOW(), NOW()),
    (902, 900, 1, 2025, 78.0, NOW(), 901, TRUE, NOW(), NOW())
ON CONFLICT ("ClassSubjectId", "StudentId", "Month", "SchoolYear") DO NOTHING;

-- MonthlyReport Id 900
INSERT INTO "MonthlyReports" ("Id", "ClassId", "Month", "SchoolYear", "SubmittedBy", "SubmittedAt", "CreatedAt", "UpdatedAt")
VALUES (900, 900, 1, 2025, 901, NOW(), NOW(), NOW())
ON CONFLICT DO NOTHING;

-- MonthlyReportEntry for the student
INSERT INTO "MonthlyReportEntries" ("ReportId", "StudentId", "TotalScore", "Rank", "CreatedAt", "UpdatedAt")
VALUES (900, 900, 253.0, 1, NOW(), NOW())
ON CONFLICT ("ReportId", "StudentId") DO NOTHING;

-- SemesterScores (Semester 1, SchoolYear 2025) - averages equal monthly since one month
INSERT INTO "SemesterScores" ("ClassSubjectId", "StudentId", "Semester", "SchoolYear", "FinalScore", "CreatedAt", "UpdatedAt")
VALUES
    (900, 900, 1, 2025, 85.0, NOW(), NOW()),
    (901, 900, 1, 2025, 90.0, NOW(), NOW()),
    (902, 900, 1, 2025, 78.0, NOW(), NOW())
ON CONFLICT ("ClassSubjectId", "StudentId", "Semester", "SchoolYear") DO NOTHING;

-- SemesterReport Id 900
INSERT INTO "SemesterReports" ("Id", "ClassId", "Semester", "SchoolYear", "SubmittedBy", "SubmittedAt", "CreatedAt", "UpdatedAt")
VALUES (900, 900, 1, 2025, 901, NOW(), NOW(), NOW())
ON CONFLICT DO NOTHING;

-- SemesterReportEntry for the student
INSERT INTO "SemesterReportEntries" ("ReportId", "StudentId", "TotalScore", "Rank", "CreatedAt", "UpdatedAt")
VALUES (900, 900, 253.0, 1, NOW(), NOW())
ON CONFLICT ("ReportId", "StudentId") DO NOTHING;

-- YearlyScores (SchoolYear 2025)
INSERT INTO "YearlyScores" ("ClassSubjectId", "StudentId", "SchoolYear", "FinalScore", "CreatedAt", "UpdatedAt")
VALUES
    (900, 900, 2025, 85.0, NOW(), NOW()),
    (901, 900, 2025, 90.0, NOW(), NOW()),
    (902, 900, 2025, 78.0, NOW(), NOW())
ON CONFLICT ("ClassSubjectId", "StudentId", "SchoolYear") DO NOTHING;

-- YearlyReport Id 900
INSERT INTO "YearlyReports" ("Id", "ClassId", "SchoolYear", "SubmittedBy", "SubmittedAt", "CreatedAt", "UpdatedAt")
VALUES (900, 900, 2025, 901, NOW(), NOW(), NOW())
ON CONFLICT DO NOTHING;

-- YearlyReportEntry for the student
INSERT INTO "YearlyReportEntries" ("ReportId", "StudentId", "TotalScore", "Rank", "CreatedAt", "UpdatedAt")
VALUES (900, 900, 253.0, 1, NOW(), NOW())
ON CONFLICT ("ReportId", "StudentId") DO NOTHING;

-- Feedback from parent on monthly report
INSERT INTO "Feedbacks" ("ParentUserId", "ReportType", "MonthlyReportId", "SemesterReportId", "YearlyReportId", "Content", "CreatedAt", "UpdatedAt")
VALUES (900, 'Monthly', 900, NULL, NULL, 'Great progress this month!', NOW(), NOW())
ON CONFLICT DO NOTHING;

-- Advance sequences to cover inserted explicit high IDs
SELECT setval(pg_get_serial_sequence('"Grades"', 'Id'), GREATEST((SELECT COALESCE(MAX("Id"), 1) FROM "Grades"), 900), true);
SELECT setval(pg_get_serial_sequence('"Subjects"', 'Id'), GREATEST((SELECT COALESCE(MAX("Id"), 1) FROM "Subjects"), 902), true);
SELECT setval(pg_get_serial_sequence('"Users"', 'Id'), GREATEST((SELECT COALESCE(MAX("Id"), 1) FROM "Users"), 902), true);
SELECT setval(pg_get_serial_sequence('"Classes"', 'Id'), GREATEST((SELECT COALESCE(MAX("Id"), 1) FROM "Classes"), 900), true);
SELECT setval(pg_get_serial_sequence('"ClassSubjects"', 'Id'), GREATEST((SELECT COALESCE(MAX("Id"), 1) FROM "ClassSubjects"), 902), true);
SELECT setval(pg_get_serial_sequence('"Students"', 'Id'), GREATEST((SELECT COALESCE(MAX("Id"), 1) FROM "Students"), 900), true);
SELECT setval(pg_get_serial_sequence('"ParentStudents"', 'Id'), GREATEST((SELECT COALESCE(MAX("Id"), 1) FROM "ParentStudents"), 900), true);
SELECT setval(pg_get_serial_sequence('"MonthlyReports"', 'Id'), GREATEST((SELECT COALESCE(MAX("Id"), 1) FROM "MonthlyReports"), 900), true);
SELECT setval(pg_get_serial_sequence('"SemesterReports"', 'Id'), GREATEST((SELECT COALESCE(MAX("Id"), 1) FROM "SemesterReports"), 900), true);
SELECT setval(pg_get_serial_sequence('"YearlyReports"', 'Id'), GREATEST((SELECT COALESCE(MAX("Id"), 1) FROM "YearlyReports"), 900), true);

-- end of file
