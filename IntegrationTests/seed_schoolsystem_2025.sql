BEGIN;

-- Grades (skip existing Id=2, insert only requested new rows)
INSERT INTO "Grades" ("Id", "Name", "CreatedAt", "UpdatedAt")
VALUES
    (3, 'Grade 11', NOW(), NOW()),
    (4, 'Grade 12', NOW(), NOW())
ON CONFLICT DO NOTHING;

-- Subjects (Ids 1-11)
INSERT INTO "Subjects" ("Id", "Name", "CreatedAt", "UpdatedAt")
VALUES
    (1,  'Khmer',         NOW(), NOW()),
    (2,  'English',       NOW(), NOW()),
    (3,  'Math Geometry', NOW(), NOW()),
    (4,  'Math Algebra',  NOW(), NOW()),
    (5,  'Chemistry',     NOW(), NOW()),
    (6,  'Physics',       NOW(), NOW()),
    (7,  'Robotics',      NOW(), NOW()),
    (8,  'History',       NOW(), NOW()),
    (9,  'Morality',      NOW(), NOW()),
    (10, 'Geography',     NOW(), NOW()),
    (11, 'Biology',       NOW(), NOW())
ON CONFLICT DO NOTHING;

-- Teachers (Ids 10-20)
INSERT INTO "Users" ("Id", "Name", "Sex", "Dob", "Contact", "PasswordHash", "IsActive", "CreatedAt", "UpdatedAt")
VALUES
    (10, 'Ethan Walker',      'Male',   '1982-03-14 00:00:00+00', 'teacher10@school.com', '$2b$12$C6UzMDM.H6dfI/f/IKcEe.6Y7Nf6kRDeo63eFUsVTNff7kwh28F5e', TRUE, NOW(), NOW()),
    (11, 'Olivia Carter',     'Female', '1987-07-09 00:00:00+00', 'teacher11@school.com', '$2b$12$C6UzMDM.H6dfI/f/IKcEe.6Y7Nf6kRDeo63eFUsVTNff7kwh28F5e', TRUE, NOW(), NOW()),
    (12, 'Noah Bennett',      'Male',   '1979-11-26 00:00:00+00', 'teacher12@school.com', '$2b$12$C6UzMDM.H6dfI/f/IKcEe.6Y7Nf6kRDeo63eFUsVTNff7kwh28F5e', TRUE, NOW(), NOW()),
    (13, 'Sophia Reed',       'Female', '1984-01-18 00:00:00+00', 'teacher13@school.com', '$2b$12$C6UzMDM.H6dfI/f/IKcEe.6Y7Nf6kRDeo63eFUsVTNff7kwh28F5e', TRUE, NOW(), NOW()),
    (14, 'Liam Foster',       'Male',   '1990-05-02 00:00:00+00', 'teacher14@school.com', '$2b$12$C6UzMDM.H6dfI/f/IKcEe.6Y7Nf6kRDeo63eFUsVTNff7kwh28F5e', TRUE, NOW(), NOW()),
    (15, 'Ava Mitchell',      'Female', '1981-09-30 00:00:00+00', 'teacher15@school.com', '$2b$12$C6UzMDM.H6dfI/f/IKcEe.6Y7Nf6kRDeo63eFUsVTNff7kwh28F5e', TRUE, NOW(), NOW()),
    (16, 'Mason Hughes',      'Male',   '1977-12-12 00:00:00+00', 'teacher16@school.com', '$2b$12$C6UzMDM.H6dfI/f/IKcEe.6Y7Nf6kRDeo63eFUsVTNff7kwh28F5e', TRUE, NOW(), NOW()),
    (17, 'Isabella Brooks',   'Female', '1992-04-21 00:00:00+00', 'teacher17@school.com', '$2b$12$C6UzMDM.H6dfI/f/IKcEe.6Y7Nf6kRDeo63eFUsVTNff7kwh28F5e', TRUE, NOW(), NOW()),
    (18, 'Jacob Hayes',       'Male',   '1986-08-16 00:00:00+00', 'teacher18@school.com', '$2b$12$C6UzMDM.H6dfI/f/IKcEe.6Y7Nf6kRDeo63eFUsVTNff7kwh28F5e', TRUE, NOW(), NOW()),
    (19, 'Mia Richardson',    'Female', '1989-02-25 00:00:00+00', 'teacher19@school.com', '$2b$12$C6UzMDM.H6dfI/f/IKcEe.6Y7Nf6kRDeo63eFUsVTNff7kwh28F5e', TRUE, NOW(), NOW()),
    (20, 'Daniel Simmons',    'Male',   '1978-06-07 00:00:00+00', 'teacher20@school.com', '$2b$12$C6UzMDM.H6dfI/f/IKcEe.6Y7Nf6kRDeo63eFUsVTNff7kwh28F5e', TRUE, NOW(), NOW())
ON CONFLICT DO NOTHING;

-- UserRoles for teachers
INSERT INTO "UserRoles" ("UserId", "RoleId")
SELECT g, 2
FROM generate_series(10, 20) AS g
ON CONFLICT ("UserId", "RoleId") DO NOTHING;

-- Teachers 10-15 are also homeroom
INSERT INTO "UserRoles" ("UserId", "RoleId")
SELECT g, 3
FROM generate_series(10, 15) AS g
ON CONFLICT ("UserId", "RoleId") DO NOTHING;

-- Classes (Ids 1-6)
INSERT INTO "Classes" ("Id", "GradeId", "SchoolYear", "Name", "HomeroomUserId", "CreatedAt", "UpdatedAt")
VALUES
    (1, 2, 2025, '10A', 10, NOW(), NOW()),
    (2, 2, 2025, '10B', 11, NOW(), NOW()),
    (3, 3, 2025, '11A', 12, NOW(), NOW()),
    (4, 3, 2025, '11B', 13, NOW(), NOW()),
    (5, 4, 2025, '12A', 14, NOW(), NOW()),
    (6, 4, 2025, '12B', 15, NOW(), NOW())
ON CONFLICT DO NOTHING;

-- ClassSubjects (Ids 1-66)
INSERT INTO "ClassSubjects" ("Id", "ClassId", "SubjectId", "TeacherUserId", "CreatedAt", "UpdatedAt")
SELECT
    ((c.class_id - 1) * 11) + s.subject_id AS "Id",
    c.class_id AS "ClassId",
    s.subject_id AS "SubjectId",
    s.subject_id + 9 AS "TeacherUserId", -- Subject 1->Teacher 10 ... Subject 11->Teacher 20
    NOW(),
    NOW()
FROM generate_series(1, 6) AS c(class_id)
CROSS JOIN generate_series(1, 11) AS s(subject_id)
ON CONFLICT DO NOTHING;

-- Parents (Ids 100-189)
WITH first_names AS (
    SELECT ARRAY[
        'James','Emma','Benjamin','Charlotte','Henry','Amelia','Alexander','Evelyn','Michael','Harper',
        'William','Abigail','Lucas','Ella','Samuel','Scarlett','Matthew','Grace','Joseph','Chloe'
    ] AS arr
),
last_names AS (
    SELECT ARRAY[
        'Anderson','Thompson','Parker','Collins','Stewart','Murphy','Bailey','Cook','Morgan','Powell',
        'Peterson','Ward','Cooper','Diaz','Howard','Ward','Rogers','Kelly','Price','Long'
    ] AS arr
),
parent_rows AS (
    SELECT
        gs AS id,
        (SELECT arr[1 + ((gs - 100) % 20)] FROM first_names) || ' ' ||
        (SELECT arr[1 + (((gs - 100) * 3) % 20)] FROM last_names) AS full_name,
        CASE WHEN gs % 2 = 0 THEN 'Male' ELSE 'Female' END AS sex,
        ('1970-01-01'::date + (floor(random() * ((DATE '1990-12-31' - DATE '1970-01-01') + 1))::int))::timestamp AT TIME ZONE 'UTC' AS dob,
        ('parent' || gs::text || '@gmail.com') AS contact
    FROM generate_series(100, 189) AS gs
)
INSERT INTO "Users" ("Id", "Name", "Sex", "Dob", "Contact", "PasswordHash", "IsActive", "CreatedAt", "UpdatedAt")
SELECT
    p.id,
    p.full_name,
    p.sex,
    p.dob,
    p.contact,
    '$2b$12$C6UzMDM.H6dfI/f/IKcEe.6Y7Nf6kRDeo63eFUsVTNff7kwh28F5e',
    TRUE,
    NOW(),
    NOW()
FROM parent_rows p
ON CONFLICT DO NOTHING;

-- UserRoles for parents
INSERT INTO "UserRoles" ("UserId", "RoleId")
SELECT g, 4
FROM generate_series(100, 189) AS g
ON CONFLICT ("UserId", "RoleId") DO NOTHING;

-- Students (Ids 1-90), 15 per class
WITH first_names AS (
    SELECT ARRAY[
        'Aiden','Lily','Logan','Hannah','Owen','Zoe','Caleb','Nora','Ryan','Layla',
        'Nathan','Audrey','Connor','Stella','Julian','Ruby','Aaron','Lucy','Isaac','Naomi'
    ] AS arr
),
last_names AS (
    SELECT ARRAY[
        'Miller','Evans','Turner','Harris','Clark','Lewis','Young','Allen','King','Wright',
        'Scott','Green','Baker','Adams','Nelson','Hill','Campbell','Mitchell','Roberts','Carter'
    ] AS arr
),
student_rows AS (
    SELECT
        gs AS id,
        ((gs - 1) / 15) + 1 AS class_id,
        (SELECT arr[1 + ((gs - 1) % 20)] FROM first_names) || ' ' ||
        (SELECT arr[1 + (((gs - 1) * 7) % 20)] FROM last_names) AS full_name,
        CASE WHEN gs % 2 = 0 THEN 'Male' ELSE 'Female' END AS sex,
        ('2005-01-01'::date + (floor(random() * ((DATE '2010-12-31' - DATE '2005-01-01') + 1))::int))::timestamp AT TIME ZONE 'UTC' AS dob,
        ('student' || gs::text || '@gmail.com') AS contact
    FROM generate_series(1, 90) AS gs
)
INSERT INTO "Students" ("Id", "Name", "Sex", "Dob", "Contact", "ClassId", "CreatedAt", "UpdatedAt")
SELECT
    s.id,
    s.full_name,
    s.sex,
    s.dob,
    s.contact,
    s.class_id,
    NOW(),
    NOW()
FROM student_rows s
ON CONFLICT DO NOTHING;

-- ParentStudents (Ids 1-90, one-to-one mapping)
INSERT INTO "ParentStudents" ("Id", "ParentUserId", "StudentId", "CreatedAt", "UpdatedAt")
SELECT
    gs,
    99 + gs,
    gs,
    NOW(),
    NOW()
FROM generate_series(1, 90) AS gs
ON CONFLICT ("ParentUserId", "StudentId") DO NOTHING;

-- MonthlyScores: each ClassSubject x Student-in-class x Month (1-12)
INSERT INTO "MonthlyScores" ("ClassSubjectId", "StudentId", "Month", "SchoolYear", "FinalScore", "SubmittedAt", "SubmittedBy", "IsLocked", "CreatedAt", "UpdatedAt")
SELECT
    cs."Id",
    st."Id",
    m.mon::smallint,
    2025::smallint,
    ROUND((50 + random() * 50)::numeric, 2),
    NOW(),
    cs."TeacherUserId",
    TRUE,
    NOW(),
    NOW()
FROM "ClassSubjects" cs
JOIN "Students" st ON st."ClassId" = cs."ClassId"
CROSS JOIN generate_series(1, 12) AS m(mon)
ON CONFLICT ("ClassSubjectId", "StudentId", "Month", "SchoolYear") DO NOTHING;

-- SemesterScores: each ClassSubject x Student-in-class x Semester (1,2)
INSERT INTO "SemesterScores" ("ClassSubjectId", "StudentId", "Semester", "SchoolYear", "FinalScore", "CreatedAt", "UpdatedAt")
SELECT
    cs."Id",
    st."Id",
    sem.sem::smallint,
    2025::smallint,
    ROUND((50 + random() * 50)::numeric, 2),
    NOW(),
    NOW()
FROM "ClassSubjects" cs
JOIN "Students" st ON st."ClassId" = cs."ClassId"
CROSS JOIN generate_series(1, 2) AS sem(sem)
ON CONFLICT ("ClassSubjectId", "StudentId", "Semester", "SchoolYear") DO NOTHING;

-- YearlyScores: each ClassSubject x Student-in-class
INSERT INTO "YearlyScores" ("ClassSubjectId", "StudentId", "SchoolYear", "FinalScore", "CreatedAt", "UpdatedAt")
SELECT
    cs."Id",
    st."Id",
    2025::smallint,
    ROUND((50 + random() * 50)::numeric, 2),
    NOW(),
    NOW()
FROM "ClassSubjects" cs
JOIN "Students" st ON st."ClassId" = cs."ClassId"
ON CONFLICT ("ClassSubjectId", "StudentId", "SchoolYear") DO NOTHING;

-- Attendances: 20 records per student per class-subject
INSERT INTO "Attendances" ("StudentId", "ClassSubjectId", "Date", "Status", "CreatedAt", "UpdatedAt")
SELECT
    st."Id",
    cs."Id",
    make_timestamptz(
        2025,
        (((a.idx - 1) % 12) + 1)::int,
        (((a.idx * 7 + st."Id" + cs."Id") % 20) + 1)::int,
        7,
        30,
        0,
        'UTC'
    ),
    CASE
        WHEN r.v <= 4 THEN 'Present'
        WHEN r.v = 5 THEN 'Absent'
        ELSE 'Late'
    END AS "Status",
    NOW(),
    NOW()
FROM "ClassSubjects" cs
JOIN "Students" st ON st."ClassId" = cs."ClassId"
CROSS JOIN generate_series(1, 20) AS a(idx)
CROSS JOIN LATERAL (SELECT floor(random() * 6 + 1)::int AS v) AS r
ON CONFLICT ("StudentId", "ClassSubjectId", "Date") DO NOTHING;

-- GradebookEntries: 3 per student per class-subject
WITH entry_types AS (
    SELECT *
    FROM (VALUES
        ('Quiz'::text, 20::numeric),
        ('Midterm'::text, 50::numeric),
        ('Assignment'::text, 30::numeric)
    ) AS t(label, max_score)
)
INSERT INTO "GradebookEntries" ("ClassSubjectId", "StudentId", "Label", "MaxScore", "Score", "EntryDate", "CreatedAt", "UpdatedAt")
SELECT
    cs."Id",
    st."Id",
    et.label,
    et.max_score,
    ROUND((et.max_score * (0.5 + random() * 0.5))::numeric, 2),
    make_timestamptz(
        2025,
        (floor(random() * 12) + 1)::int,
        (floor(random() * 28) + 1)::int,
        8,
        0,
        0,
        'UTC'
    ),
    NOW(),
    NOW()
FROM "ClassSubjects" cs
JOIN "Students" st ON st."ClassId" = cs."ClassId"
CROSS JOIN entry_types et
WHERE NOT EXISTS (
    SELECT 1
    FROM "GradebookEntries" gbe
    WHERE gbe."ClassSubjectId" = cs."Id"
      AND gbe."StudentId" = st."Id"
      AND gbe."Label" = et.label
);

-- MonthlyReports: Ids 1-72 (6 classes x 12 months)
INSERT INTO "MonthlyReports" ("Id", "ClassId", "Month", "SchoolYear", "SubmittedBy", "SubmittedAt", "CreatedAt", "UpdatedAt")
SELECT
    ((c.class_id - 1) * 12) + m.mon AS id,
    c.class_id,
    m.mon::smallint,
    2025::smallint,
    c.class_id + 9,
    NOW(),
    NOW(),
    NOW()
FROM generate_series(1, 6) AS c(class_id)
CROSS JOIN generate_series(1, 12) AS m(mon)
ON CONFLICT DO NOTHING;

-- MonthlyReportEntries: 15 students per report, ranked by score
WITH base AS (
    SELECT
        mr."Id" AS report_id,
        st."Id" AS student_id,
        ROUND((50 + random() * 50)::numeric, 2) AS total_score
    FROM "MonthlyReports" mr
    JOIN "Students" st ON st."ClassId" = mr."ClassId"
), ranked AS (
    SELECT
        b.report_id,
        b.student_id,
        b.total_score,
        row_number() OVER (PARTITION BY b.report_id ORDER BY b.total_score DESC, b.student_id) AS rank_no
    FROM base b
)
INSERT INTO "MonthlyReportEntries" ("ReportId", "StudentId", "TotalScore", "Rank", "CreatedAt", "UpdatedAt")
SELECT
    r.report_id,
    r.student_id,
    r.total_score,
    r.rank_no::smallint,
    NOW(),
    NOW()
FROM ranked r
ON CONFLICT ("ReportId", "StudentId") DO NOTHING;

-- SemesterReports: Ids 1-12 (6 classes x 2 semesters)
INSERT INTO "SemesterReports" ("Id", "ClassId", "Semester", "SchoolYear", "SubmittedBy", "SubmittedAt", "CreatedAt", "UpdatedAt")
SELECT
    ((c.class_id - 1) * 2) + s.sem AS id,
    c.class_id,
    s.sem::smallint,
    2025::smallint,
    c.class_id + 9,
    NOW(),
    NOW(),
    NOW()
FROM generate_series(1, 6) AS c(class_id)
CROSS JOIN generate_series(1, 2) AS s(sem)
ON CONFLICT DO NOTHING;

-- SemesterReportEntries: ranked by score within each report
WITH base AS (
    SELECT
        sr."Id" AS report_id,
        st."Id" AS student_id,
        ROUND((50 + random() * 50)::numeric, 2) AS total_score
    FROM "SemesterReports" sr
    JOIN "Students" st ON st."ClassId" = sr."ClassId"
), ranked AS (
    SELECT
        b.report_id,
        b.student_id,
        b.total_score,
        row_number() OVER (PARTITION BY b.report_id ORDER BY b.total_score DESC, b.student_id) AS rank_no
    FROM base b
)
INSERT INTO "SemesterReportEntries" ("ReportId", "StudentId", "TotalScore", "Rank", "CreatedAt", "UpdatedAt")
SELECT
    r.report_id,
    r.student_id,
    r.total_score,
    r.rank_no::smallint,
    NOW(),
    NOW()
FROM ranked r
ON CONFLICT ("ReportId", "StudentId") DO NOTHING;

-- YearlyReports: Ids 1-6 (one per class)
INSERT INTO "YearlyReports" ("Id", "ClassId", "SchoolYear", "SubmittedBy", "SubmittedAt", "CreatedAt", "UpdatedAt")
SELECT
    c.class_id,
    c.class_id,
    2025::smallint,
    c.class_id + 9,
    NOW(),
    NOW(),
    NOW()
FROM generate_series(1, 6) AS c(class_id)
ON CONFLICT DO NOTHING;

-- YearlyReportEntries: ranked by score within each report
WITH base AS (
    SELECT
        yr."Id" AS report_id,
        st."Id" AS student_id,
        ROUND((50 + random() * 50)::numeric, 2) AS total_score
    FROM "YearlyReports" yr
    JOIN "Students" st ON st."ClassId" = yr."ClassId"
), ranked AS (
    SELECT
        b.report_id,
        b.student_id,
        b.total_score,
        row_number() OVER (PARTITION BY b.report_id ORDER BY b.total_score DESC, b.student_id) AS rank_no
    FROM base b
)
INSERT INTO "YearlyReportEntries" ("ReportId", "StudentId", "TotalScore", "Rank", "CreatedAt", "UpdatedAt")
SELECT
    r.report_id,
    r.student_id,
    r.total_score,
    r.rank_no::smallint,
    NOW(),
    NOW()
FROM ranked r
ON CONFLICT ("ReportId", "StudentId") DO NOTHING;

-- Feedbacks: 2 per parent (Monthly + Semester), yearly column intentionally NULL
WITH comments AS (
    SELECT ARRAY[
        'Thank you for the regular update. We are helping with homework every evening.',
        'I appreciate the teachers support this month. My child is motivated to improve.',
        'Please continue guiding my child in classroom participation and confidence.',
        'The report is clear and helpful. We will focus more on revision at home.',
        'Thank you for your effort and communication. We value the schools guidance.'
    ] AS arr
), parent_student AS (
    SELECT
        p."ParentUserId",
        p."StudentId",
        st."ClassId"
    FROM "ParentStudents" p
    JOIN "Students" st ON st."Id" = p."StudentId"
)
INSERT INTO "Feedbacks" ("ParentUserId", "ReportType", "MonthlyReportId", "SemesterReportId", "YearlyReportId", "Content", "CreatedAt", "UpdatedAt")
SELECT
    ps."ParentUserId",
    'Monthly',
    ((ps."ClassId" - 1) * 12) + 6,
    NULL::integer,
    NULL::integer,
    (SELECT arr[1 + ((ps."StudentId" - 1) % 5)] FROM comments),
    NOW(),
    NOW()
FROM parent_student ps
WHERE NOT EXISTS (
    SELECT 1
    FROM "Feedbacks" f
    WHERE f."ParentUserId" = ps."ParentUserId"
      AND f."ReportType" = 'Monthly'
)
UNION ALL
SELECT
    ps."ParentUserId",
    'Semester',
    NULL::integer,
    ((ps."ClassId" - 1) * 2) + 2,
    NULL::integer,
    (SELECT arr[1 + ((ps."StudentId" + 1) % 5)] FROM comments),
    NOW(),
    NOW()
FROM parent_student ps
WHERE NOT EXISTS (
    SELECT 1
    FROM "Feedbacks" f
    WHERE f."ParentUserId" = ps."ParentUserId"
      AND f."ReportType" = 'Semester'
);

-- Keep identity sequences aligned with explicit IDs
SELECT setval(pg_get_serial_sequence('"Grades"', 'Id'), GREATEST((SELECT COALESCE(MAX("Id"), 1) FROM "Grades"), 1), true);
SELECT setval(pg_get_serial_sequence('"Subjects"', 'Id'), GREATEST((SELECT COALESCE(MAX("Id"), 1) FROM "Subjects"), 1), true);
SELECT setval(pg_get_serial_sequence('"Users"', 'Id'), GREATEST((SELECT COALESCE(MAX("Id"), 1) FROM "Users"), 1), true);
SELECT setval(pg_get_serial_sequence('"Classes"', 'Id'), GREATEST((SELECT COALESCE(MAX("Id"), 1) FROM "Classes"), 1), true);
SELECT setval(pg_get_serial_sequence('"ClassSubjects"', 'Id'), GREATEST((SELECT COALESCE(MAX("Id"), 1) FROM "ClassSubjects"), 1), true);
SELECT setval(pg_get_serial_sequence('"Students"', 'Id'), GREATEST((SELECT COALESCE(MAX("Id"), 1) FROM "Students"), 1), true);
SELECT setval(pg_get_serial_sequence('"MonthlyReports"', 'Id'), GREATEST((SELECT COALESCE(MAX("Id"), 1) FROM "MonthlyReports"), 1), true);
SELECT setval(pg_get_serial_sequence('"SemesterReports"', 'Id'), GREATEST((SELECT COALESCE(MAX("Id"), 1) FROM "SemesterReports"), 1), true);
SELECT setval(pg_get_serial_sequence('"YearlyReports"', 'Id'), GREATEST((SELECT COALESCE(MAX("Id"), 1) FROM "YearlyReports"), 1), true);

COMMIT;
