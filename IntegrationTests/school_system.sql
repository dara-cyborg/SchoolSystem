-- ENUMS
CREATE TYPE role_name AS ENUM ('super_admin', 'teacher', 'homeroom', 'parent');
CREATE TYPE attendance_status AS ENUM ('present', 'informed_absent', 'uninformed_absent');
CREATE TYPE report_type AS ENUM ('monthly', 'semester', 'yearly');
CREATE TYPE sex_type AS ENUM ('male', 'female');

-- TABLES
CREATE TABLE roles (
    id SERIAL PRIMARY KEY,
    name role_name UNIQUE NOT NULL
);

CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    sex sex_type NOT NULL,
    dob DATE,
    contact VARCHAR(50),
    password_hash TEXT NOT NULL,
    is_active BOOLEAN DEFAULT true,
    created_at TIMESTAMPTZ DEFAULT now()
);

CREATE TABLE user_roles (
    user_id INTEGER REFERENCES users(id),
    role_id INTEGER REFERENCES roles(id),
    PRIMARY KEY(user_id, role_id)
);

CREATE TABLE grades (
    id SERIAL PRIMARY KEY,
    name VARCHAR(10) UNIQUE NOT NULL
);

CREATE TABLE classes (
    id SERIAL PRIMARY KEY,
    grade_id INTEGER REFERENCES grades(id),
    school_year SMALLINT NOT NULL,
    name VARCHAR(20) NOT NULL,
    homeroom_user_id INTEGER REFERENCES users(id),
    UNIQUE(grade_id, name, school_year)
);

CREATE TABLE subjects (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) UNIQUE NOT NULL
);

CREATE TABLE class_subjects (
    id SERIAL PRIMARY KEY,
    class_id INTEGER REFERENCES classes(id),
    subject_id INTEGER REFERENCES subjects(id),
    teacher_user_id INTEGER REFERENCES users(id),
    UNIQUE(class_id, subject_id)
);

CREATE TABLE students (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    sex sex_type NOT NULL,
    dob DATE,
    contact VARCHAR(50),
    class_id INTEGER REFERENCES classes(id),
    created_at TIMESTAMPTZ DEFAULT now()
);

CREATE TABLE parent_student (
    parent_user_id INTEGER REFERENCES users(id),
    student_id INTEGER REFERENCES students(id),
    PRIMARY KEY(parent_user_id, student_id)
);

CREATE TABLE attendance (
    id SERIAL PRIMARY KEY,
    student_id INTEGER REFERENCES students(id),
    class_subject_id INTEGER REFERENCES class_subjects(id),
    date DATE NOT NULL,
    status attendance_status NOT NULL,
    UNIQUE(student_id, class_subject_id, date)
);

CREATE TABLE gradebook_entries (
    id SERIAL PRIMARY KEY,
    class_subject_id INTEGER REFERENCES class_subjects(id),
    student_id INTEGER REFERENCES students(id),
    label VARCHAR(100) NOT NULL,
    max_score NUMERIC(6,2) NOT NULL,
    score NUMERIC(6,2) NOT NULL,
    entry_date DATE NOT NULL,
    created_at TIMESTAMPTZ DEFAULT now()
);

CREATE TABLE monthly_scores (
    id SERIAL PRIMARY KEY,
    class_subject_id INTEGER REFERENCES class_subjects(id),
    student_id INTEGER REFERENCES students(id),
    month SMALLINT CHECK(month BETWEEN 1 AND 12),
    school_year SMALLINT NOT NULL,
    final_score NUMERIC(6,2) NOT NULL,
    submitted_at TIMESTAMPTZ,
    submitted_by INTEGER REFERENCES users(id),
    is_locked BOOLEAN DEFAULT false,
    UNIQUE(class_subject_id, student_id, month, school_year)
);

CREATE TABLE semester_scores (
    id SERIAL PRIMARY KEY,
    class_subject_id INTEGER REFERENCES class_subjects(id),
    student_id INTEGER REFERENCES students(id),
    semester SMALLINT CHECK(semester BETWEEN 1 AND 2),
    school_year SMALLINT NOT NULL,
    final_score NUMERIC(6,2),
    UNIQUE(class_subject_id, student_id, semester, school_year)
);

CREATE TABLE yearly_scores (
    id SERIAL PRIMARY KEY,
    class_subject_id INTEGER REFERENCES class_subjects(id),
    student_id INTEGER REFERENCES students(id),
    school_year SMALLINT NOT NULL,
    final_score NUMERIC(6,2),
    UNIQUE(class_subject_id, student_id, school_year)
);

CREATE TABLE monthly_reports (
    id SERIAL PRIMARY KEY,
    class_id INTEGER REFERENCES classes(id),
    month SMALLINT CHECK(month BETWEEN 1 AND 12),
    school_year SMALLINT NOT NULL,
    submitted_by INTEGER REFERENCES users(id),
    submitted_at TIMESTAMPTZ,
    UNIQUE(class_id, month, school_year)
);

CREATE TABLE monthly_report_entries (
    id SERIAL PRIMARY KEY,
    report_id INTEGER REFERENCES monthly_reports(id),
    student_id INTEGER REFERENCES students(id),
    total_score NUMERIC(8,2),
    rank SMALLINT,
    UNIQUE(report_id, student_id)
);

CREATE TABLE semester_reports (
    id SERIAL PRIMARY KEY,
    class_id INTEGER REFERENCES classes(id),
    semester SMALLINT CHECK(semester BETWEEN 1 AND 2),
    school_year SMALLINT NOT NULL,
    submitted_by INTEGER REFERENCES users(id),
    submitted_at TIMESTAMPTZ,
    UNIQUE(class_id, semester, school_year)
);

CREATE TABLE semester_report_entries (
    id SERIAL PRIMARY KEY,
    report_id INTEGER REFERENCES semester_reports(id),
    student_id INTEGER REFERENCES students(id),
    total_score NUMERIC(8,2),
    rank SMALLINT,
    UNIQUE(report_id, student_id)
);

CREATE TABLE yearly_reports (
    id SERIAL PRIMARY KEY,
    class_id INTEGER REFERENCES classes(id),
    school_year SMALLINT NOT NULL,
    submitted_by INTEGER REFERENCES users(id),
    submitted_at TIMESTAMPTZ,
    UNIQUE(class_id, school_year)
);

CREATE TABLE yearly_report_entries (
    id SERIAL PRIMARY KEY,
    report_id INTEGER REFERENCES yearly_reports(id),
    student_id INTEGER REFERENCES students(id),
    total_score NUMERIC(8,2),
    rank SMALLINT,
    UNIQUE(report_id, student_id)
);

CREATE TABLE feedback (
    id SERIAL PRIMARY KEY,
    report_type report_type NOT NULL,
    monthly_report_id INTEGER REFERENCES monthly_reports(id),
    semester_report_id INTEGER REFERENCES semester_reports(id),
    yearly_report_id INTEGER REFERENCES yearly_reports(id),
    parent_user_id INTEGER REFERENCES users(id),
    content TEXT NOT NULL,
    created_at TIMESTAMPTZ DEFAULT now()
);

-- INDEXES
CREATE INDEX idx_attendance_student_date ON attendance(student_id, date);
CREATE INDEX idx_gradebook_entries_class_student ON gradebook_entries(class_subject_id, student_id);
DROP INDEX IF EXISTS idx_monthly_scores_class_year_month;
CREATE INDEX idx_monthly_scores_class_year_month ON monthly_scores(class_subject_id, school_year, month);
CREATE INDEX idx_students_class_id ON students(class_id);
CREATE INDEX idx_classes_school_year ON classes(school_year);

-- SEED DATA
INSERT INTO roles (name) VALUES ('super_admin'), ('teacher'), ('homeroom'), ('parent');

INSERT INTO users (name, sex, password_hash) VALUES ('admin', 'male', 'HASH_ME');

INSERT INTO user_roles (user_id, role_id) VALUES ((SELECT id FROM users WHERE name = 'admin'), (SELECT id FROM roles WHERE name = 'super_admin'));

INSERT INTO grades (name) VALUES ('10'), ('11'), ('12');