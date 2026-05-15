# SchoolSystem - Project Documentation

## 1. Project Overview

**Project Name**: SchoolSystem  
**Project Type**: School Management Information System (SMIS)  
**Development Framework**: ASP.NET Core 10 with C# 13  
**Target Platform**: Web (Razor Pages) + Desktop (WinForms) + REST API  
**Database**: PostgreSQL with Entity Framework Core  
**Project Category**: Academic Management & Administration System  

### 1.1 Project Description

The SchoolSystem is a comprehensive school management platform designed to digitize and streamline academic operations. The system provides an integrated solution for managing students, classes, subjects, scoring, attendance, and comprehensive reporting with role-based access control.

### 1.2 System Scope

The SchoolSystem encompasses the following key operational areas:

- **Student & Class Management**: Create and organize students into classes by grades and school years
- **Subject & Curriculum Management**: Manage subjects and assign them to classes with teacher assignments
- **Academic Scoring System**: Three-tier scoring system (monthly, semester, yearly) for tracking student performance
- **Attendance Tracking**: Record and monitor student attendance for each class-subject combination
- **Gradebook Management**: Teachers record individual scores and assessments
- **Comprehensive Reporting**: Automated generation of monthly, semester, and yearly academic reports
- **Feedback System**: Parents provide feedback on academic reports
- **Role-Based Access Management**: Multi-tier user system with SuperAdmin, Teacher, Homeroom, and Parent roles

---

## 2. Problem Insight

### 2.1 Current Challenges in School Management

Modern educational institutions face several critical challenges in academic administration:

1. **Manual Record Keeping**: Paper-based systems for attendance, grades, and reports are time-consuming, error-prone, and difficult to retrieve
2. **Lack of Real-time Visibility**: Teachers, parents, and administrators cannot quickly access accurate student performance data
3. **Inconsistent Grading**: Without a unified system, grading standards vary across subjects and classes
4. **Limited Parent Engagement**: Parents have minimal access to their children's academic progress, hindering proactive involvement
5. **Inefficient Reporting**: Manual compilation of reports is labor-intensive and difficult to audit
6. **Data Integrity Issues**: Multiple manual data entry points create risk of inconsistencies and lost information
7. **Access Control**: No standardized mechanism to control who can view, edit, or submit specific information

### 2.2 Vision

**Transform educational administration through a unified, digital platform that:**

- **Centralizes Data**: Single source of truth for all academic information (students, scores, attendance, reports)
- **Enables Transparency**: Provides stakeholders (teachers, parents, administrators) appropriate access to relevant information
- **Automates Workflows**: Eliminates manual processes through automated score aggregation and report generation
- **Ensures Data Integrity**: Implements validation, locking mechanisms, and audit trails
- **Facilitates Communication**: Creates channels for parent-teacher collaboration through feedback systems
- **Supports Decision Making**: Provides comprehensive analytics and reporting for academic planning
- **Improves Efficiency**: Reduces administrative workload, allowing focus on educational quality

### 2.3 Target Users

| Role | Primary Functions | Pain Points Addressed |
|------|-------------------|----------------------|
| **SuperAdmin** | System configuration, user management, academic structure setup | Centralized control over system configuration |
| **Teacher** | Record grades, mark attendance, submit monthly scores | Quick score entry, clear submission workflows |
| **Homeroom Teacher** | Manage class, submit reports, lock scores, view feedback | Single dashboard for class management |
| **Parent** | View children's reports, submit feedback, monitor progress | Real-time access to child's academic status |

---

## 3. Development Plan

### 3.1 Phase I - Core Feature Implementation (Current Phase)

**Duration**: 12-14 weeks  
**Primary Output**: REST API + Database Schema + Core Services  
**Target Users**: API consumers (Desktop app, Web app)

#### 3.1.1 Sprint 1-2: Project Setup & Database Design
- Set up ASP.NET Core API project structure
- Design and implement database schema with PostgreSQL
- Create Entity Framework Core models and DbContext
- Implement database migrations
- Set up Entity-Relationship mapping

#### 3.1.2 Sprint 3-4: Authentication & Authorization
- Implement JWT token-based authentication
- Create role-based authorization policies
- Implement BCrypt password hashing
- Create AuthService with login/logout workflows
- Set up authorization handlers and middleware

#### 3.1.3 Sprint 5-6: User & Academic Structure Management
- Implement UserService (CRUD operations)
- Create GradeService for grade management
- Implement ClassService for class management
- Create SubjectService for subject management
- Implement ClassSubjectService for subject-class assignments

#### 3.1.4 Sprint 7-8: Student & Attendance Management
- Implement StudentService
- Create AttendanceService for attendance recording
- Build attendance tracking and retrieval endpoints
- Implement attendance statistics calculations
- Add validation and error handling

#### 3.1.5 Sprint 9-10: Scoring & Gradebook System
- Implement GradebookService for individual score entries
- Create MonthlyScoreService with aggregation logic
- Implement SemesterScoreService for semester calculations
- Create YearlyScoreService for yearly aggregations
- Build score submission and locking mechanisms

#### 3.1.6 Sprint 11-12: Reporting System
- Implement MonthlyReportService with report generation
- Create SemesterReportService with semester reporting
- Implement YearlyReportService for yearly reports
- Build report entry generation with ranking logic
- Add feedback system with FeedbackService

#### 3.1.7 Sprint 13-14: Testing, Documentation & API Polish
- Write unit tests for all services
- Implement integration tests
- Create API documentation (Swagger/OpenAPI)
- Set up error handling and validation
- Performance testing and optimization
- Complete API documentation

**Phase I Deliverables**:
- ✅ RESTful API with full CRUD operations
- ✅ PostgreSQL database with EF Core
- ✅ Authentication & authorization system
- ✅ Complete business logic layer
- ✅ API documentation
- ✅ Unit and integration tests

---

### 3.2 Phase II - Desktop WinForms UI Implementation

**Duration**: 10-12 weeks  
**Primary Output**: Desktop application for Teachers, Homeroom, and SuperAdmin  
**Target Users**: Teachers, Homeroom Teachers, System Administrators

#### 3.2.1 Project Structure Setup
- Create SchoolSystem.Desktop (WinForms project)
- Reference SchoolSystem.Core for shared models and DTOs
- Configure HttpClient for API communication
- Implement session management and caching

#### 3.2.2 Authentication Module
- Login screen with username/password
- Remember me functionality
- Logout with session cleanup
- Role-based startup screen redirection

#### 3.2.3 Superuser/Administrator Dashboard
- User management interface (CRUD)
- Role assignment interface
- Grade and class management screens
- Subject management interface
- System settings and configuration

#### 3.2.4 Teacher Module
- Dashboard showing assigned subjects and classes
- Attendance recording interface
- Gradebook entry form
- Student performance overview
- Monthly score submission form
- Edit/view submission history

#### 3.2.5 Homeroom Teacher Module
- Class overview dashboard
- Student management for homeroom class
- All students' scores across subjects
- Report generation interface
- Score locking/unlocking controls
- Feedback viewing interface
- Class attendance summary

#### 3.2.6 Reports & Analytics
- Report generation interface
- Report viewing with print/export options
- Student performance analytics
- Class performance comparisons
- Attendance reports

#### 3.2.7 Testing & Deployment
- Unit testing for UI logic
- User acceptance testing (UAT)
- Bug fixing and optimization
- Create installer/deployment package

**Phase II Deliverables**:
- ✅ Desktop WinForms application
- ✅ Full CRUD operations via desktop interface
- ✅ Report generation and viewing
- ✅ Offline caching where applicable
- ✅ User guide and training documentation

---

### 3.3 Phase III - Web Portal & Enhancements

**Duration**: 8-10 weeks  
**Primary Output**: Razor Pages web interface + system enhancements  
**Target Users**: Parents, Public access

#### 3.3.1 Parent Portal (Razor Pages)
- Responsive web interface for parents
- Child/children selection and dashboard
- Report viewing interface (monthly, semester, yearly)
- Feedback submission forms
- Attendance summary view
- Performance analytics visualization

#### 3.3.2 Public Features
- School information page
- System accessibility guidelines
- Privacy policy and terms
- Contact/support information

#### 3.3.3 Advanced Enhancements
- Performance optimization
- Mobile responsiveness improvements
- Advanced reporting and analytics
- Integration with notification systems (email, SMS)
- System backup and disaster recovery

#### 3.3.4 Deployment & Documentation
- Deploy to production environment
- Create comprehensive user documentation
- Administrator guide
- Teacher training materials
- Parent guide

**Phase III Deliverables**:
- ✅ Parent-facing web portal
- ✅ Responsive web interface
- ✅ Complete user documentation
- ✅ Production deployment
- ✅ Support and maintenance framework

---

## 4. Scope of Work (Phase I)

### 4.1 Included Features

**Core API Functionality**:
- User authentication and management
- Academic structure setup (Grades, Classes, Subjects, ClassSubjects)
- Student management
- Attendance recording and retrieval
- Gradebook entry management
- Monthly, semester, and yearly score calculations
- Monthly, semester, and yearly report generation
- Feedback submission and retrieval
- Role-based access control

**Database**:
- PostgreSQL schema design
- Entity Framework Core implementation
- Database migrations
- Indexes and constraints for performance

**Services**:
- AuthService for authentication/authorization
- UserService for user management
- AcademicService for grade/class/subject management
- StudentService for student operations
- AttendanceService for attendance tracking
- GradebookService for grade entries
- MonthlyScoreService, SemesterScoreService, YearlyScoreService
- MonthlyReportService, SemesterReportService, YearlyReportService
- FeedbackService for feedback management

**API Endpoints**:
- Authentication: Login, logout, token refresh
- Users: CRUD, role assignment
- Academic: Grades, classes, subjects, class-subjects
- Students: CRUD by class
- Attendance: Record, retrieve, statistics
- Gradebook: Record entries, retrieve by student
- Scores: Submit, update, retrieve, lock/unlock
- Reports: Generate, retrieve, view entries
- Feedback: Submit, retrieve, manage

**Testing**:
- Unit tests for services
- Integration tests for API endpoints
- Database context tests
- Authentication/authorization tests

### 4.2 Out of Scope (Phase I)

- Desktop UI implementation (Phase II)
- Web portal implementation (Phase III)
- Advanced analytics and dashboards
- Mobile application
- Real-time notifications
- Video conferencing integration
- Learning Management System (LMS) features
- Timetable management
- Exam scheduling

### 4.3 Constraints & Assumptions

**Technical Constraints**:
- Database: PostgreSQL only (no SQL Server or other DB engines)
- Authentication: JWT tokens for API only
- API Documentation: Must include Swagger/OpenAPI
- Maximum page size for pagination: 100 items

**Assumptions**:
- All users have unique usernames
- Passwords are minimum 8 characters
- School year is represented as short integer (e.g., 2024)
- Months are represented as 1-12 for calendar months
- Semesters are exactly 2 per school year (Sem 1: Months 1-5, Sem 2: Months 6-12)
- All scores are decimal with 2 decimal places
- Ranking is calculated based on average score (highest first)

---

## 5. Database Design

### 5.1 Entity Relationship Diagram Concept

```
┌──────────────────────────────────────────────────────────────────┐
│                        ACADEMIC STRUCTURE                         │
├──────────────────────────────────────────────────────────────────┤
│                                                                   │
│  Grade (1) ──────┐                                               │
│   ├─ Id          │                                               │
│   └─ Name        └─── (1:Many) ──→ Class                        │
│                         ├─ Id                                     │
│                         ├─ GradeId (FK)                          │
│                         ├─ SchoolYear                            │
│                         ├─ Name                                  │
│                         ├─ HomeroomUserId (FK)                   │
│                         └─ (1:Many) ──→ ClassSubject            │
│                              ├─ Id                               │
│                              ├─ ClassId (FK)                    │
│                              ├─ SubjectId (FK)                  │
│                              └─ TeacherUserId (FK)              │
│                                                                   │
│  Subject (1) ─────┐                                             │
│   ├─ Id           │                                             │
│   └─ Name         └─── (1:Many) ──→ ClassSubject               │
│                                                                   │
└──────────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────────┐
│                      USER & ACCESS MANAGEMENT                     │
├──────────────────────────────────────────────────────────────────┤
│                                                                   │
│  User (1) ─────────┬─────────────────────────────────────┐       │
│   ├─ Id            │                                     │       │
│   ├─ Name          │                                     │       │
│   ├─ Sex           │                                     │       │
│   ├─ Dob           │                                     │       │
│   ├─ Contact       │                                     │       │
│   ├─ PasswordHash  │                                     │       │
│   └─ IsActive      │                                     │       │
│         │          │                                     │       │
│         │ (Many:Many) via UserRole                       │       │
│         │          │                                     │       │
│    ┌────┼──────────┼─────────────────────────────┐       │       │
│    │    │          │                             │       │       │
│    ▼    ▼          ▼                             ▼       │       │
│  Role  Class    ClassSubject                  ParentStudent│    │
│                (as Teacher)                                │       │
│    │    │          │                                      │       │
│    └────┼──────────┴──────────────────────────────────────┘       │
│         │                                                         │
│         └─ (1:Many) ──→ Student                                  │
│              ├─ Id                                              │
│              ├─ ClassId (FK)                                    │
│              ├─ Name                                            │
│              ├─ Sex                                             │
│              ├─ Dob                                             │
│              └─ Contact                                         │
│                                                                   │
└──────────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────────┐
│                    ACADEMIC PERFORMANCE TRACKING                   │
├──────────────────────────────────────────────────────────────────┤
│                                                                   │
│  Student (1) ──────┬──────────────────────────┐                  │
│                    │                          │                  │
│                    ▼                          ▼                  │
│            Attendance                   GradebookEntry          │
│            ├─ StudentId (FK)            ├─ StudentId (FK)       │
│            ├─ ClassSubjectId (FK)       ├─ ClassSubjectId (FK)  │
│            ├─ Date                      ├─ Label                │
│            └─ Status                    ├─ MaxScore             │
│                                          ├─ Score                │
│                                          └─ EntryDate            │
│                                                                   │
│             ↓ (Aggregated to)                                    │
│                                                                   │
│            MonthlyScore                                          │
│            ├─ StudentId (FK)                                    │
│            ├─ ClassSubjectId (FK)                              │
│            ├─ Month (1-12)                                      │
│            ├─ SchoolYear                                        │
│            ├─ FinalScore                                        │
│            ├─ SubmittedBy (FK to User)                          │
│            └─ IsLocked                                          │
│                                                                   │
│             ↓ (Aggregated to)                                    │
│                                                                   │
│         ┌─────────────────┐    ┌──────────────────┐             │
│         │ SemesterScore   │    │  YearlyScore     │             │
│         ├─ StudentId (FK) │    ├─ StudentId (FK)  │             │
│         ├─ ClassSubj (FK) │    ├─ ClassSubj (FK)  │             │
│         ├─ Semester (1-2) │    ├─ SchoolYear      │             │
│         ├─ SchoolYear     │    └─ FinalScore      │             │
│         └─ FinalScore     │                       │             │
│         └────────────────┘    └──────────────────┘             │
│                                                                   │
└──────────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────────┐
│                         REPORTING SYSTEM                           │
├──────────────────────────────────────────────────────────────────┤
│                                                                   │
│  Class (1) ────────┬──────────────────────────┐                  │
│                    │                          │                  │
│            ┌───────┴─────────┐      ┌────────┴────────┐          │
│            ▼                 ▼      ▼                 ▼           │
│      MonthlyReport    SemesterReport    YearlyReport             │
│      ├─ ClassId (FK)  ├─ ClassId (FK)  ├─ ClassId (FK)          │
│      ├─ Month         ├─ Semester      ├─ SchoolYear            │
│      ├─ SchoolYear    ├─ SchoolYear    ├─ SubmittedBy (FK)       │
│      ├─ SubmittedBy   ├─ SubmittedBy   └─ SubmittedAt           │
│      ├─ SubmittedAt   ├─ SubmittedAt      │                     │
│      │                │                   │                      │
│      └─ (1:Many) ──→  │                   │ ←──── (1:Many)       │
│            │          │                   │                      │
│            ▼          ▼                   ▼                      │
│      MonthlyReportEntry   SemesterReportEntry   YearlyReportEntry│
│      ├─ ReportId (FK)     ├─ ReportId (FK)      ├─ ReportId (FK) │
│      ├─ StudentId (FK)    ├─ StudentId (FK)     ├─ StudentId (FK)│
│      ├─ Summary           ├─ Summary            ├─ Summary       │
│      ├─ AverageScore      ├─ AverageScore       ├─ AverageScore  │
│      └─ Ranking           └─ Ranking            └─ Ranking       │
│            │                   │                     │            │
│            └─────────┬─────────┴─────────┬──────────┘            │
│                      ▼                   ▼                        │
│              All linked to Feedback                              │
│              ├─ ReportType (enum)                               │
│              ├─ ReportId (conditional FK)                       │
│              ├─ ParentUserId (FK)                               │
│              └─ Content                                          │
│                                                                   │
└──────────────────────────────────────────────────────────────────┘
```

### 5.2 Core Models

#### 5.2.1 User Model
```
User
├─ Id: int (PK)
├─ Name: string (unique username)
├─ Sex: SexType (Male/Female)
├─ Dob: DateTime?
├─ Contact: string?
├─ PasswordHash: string (BCrypt hashed)
├─ IsActive: bool
├─ CreatedAt: DateTime
│
└─ Relations:
   ├─ UserRoles → Role (Many-to-Many)
   ├─ HomeroomClasses → Class (1:Many)
   ├─ TeachingClassSubjects → ClassSubject (1:Many)
   ├─ SubmittedMonthlyScores → MonthlyScore (1:Many)
   ├─ SubmittedMonthlyReports → MonthlyReport (1:Many)
   ├─ SubmittedSemesterReports → SemesterReport (1:Many)
   ├─ SubmittedYearlyReports → YearlyReport (1:Many)
   ├─ SubmittedFeedbacks → Feedback (1:Many)
   └─ ParentStudents → ParentStudent (1:Many)
```

**Purpose**: Core user entity representing all system participants

---

#### 5.2.2 Role Model
```
Role
├─ Id: int (PK)
├─ Name: RoleName (enum: SuperAdmin, Teacher, Homeroom, Parent)
│
└─ Relations:
   └─ UserRoles → User (Many-to-Many)
```

**Purpose**: Define system roles with permissions

**Role Hierarchy**:
- SuperAdmin: Full system access
- Teacher: Grade submission for assigned subjects
- Homeroom: Class management and reporting
- Parent: View children's reports and submit feedback

---

#### 5.2.3 Grade Model
```
Grade
├─ Id: int (PK)
├─ Name: string (e.g., "Grade 10", "Form 4")
│
└─ Relations:
   └─ Classes → Class (1:Many)
```

**Purpose**: Group students by academic level

---

#### 5.2.4 Class Model
```
Class
├─ Id: int (PK)
├─ GradeId: int (FK) → Grade
├─ SchoolYear: short (e.g., 2024)
├─ Name: string (e.g., "10A", "10B")
├─ HomeroomUserId: int? (FK) → User
├─ CreatedAt: DateTime
│
└─ Relations:
   ├─ Grade → Grade
   ├─ HomeroomTeacher → User
   ├─ Students → Student (1:Many)
   ├─ ClassSubjects → ClassSubject (1:Many)
   ├─ MonthlyReports → MonthlyReport (1:Many)
   ├─ SemesterReports → SemesterReport (1:Many)
   └─ YearlyReports → YearlyReport (1:Many)
```

**Purpose**: Organize students by grade and year with homeroom assignment

**Unique Constraint**: (GradeId, SchoolYear, Name)

---

#### 5.2.5 Subject Model
```
Subject
├─ Id: int (PK)
├─ Name: string (e.g., "Mathematics", "English")
│
└─ Relations:
   └─ ClassSubjects → ClassSubject (1:Many)
```

**Purpose**: Define available subjects in curriculum

---

#### 5.2.6 ClassSubject Model
```
ClassSubject
├─ Id: int (PK)
├─ ClassId: int (FK) → Class
├─ SubjectId: int (FK) → Subject
├─ TeacherUserId: int? (FK) → User
│
└─ Relations:
   ├─ Class → Class
   ├─ Subject → Subject
   ├─ Teacher → User
   ├─ Attendances → Attendance (1:Many)
   ├─ GradebookEntries → GradebookEntry (1:Many)
   ├─ MonthlyScores → MonthlyScore (1:Many)
   ├─ SemesterScores → SemesterScore (1:Many)
   └─ YearlyScores → YearlyScore (1:Many)
```

**Purpose**: Subject instance with teacher assignment

**Unique Constraint**: (ClassId, SubjectId)

---

#### 5.2.7 Student Model
```
Student
├─ Id: int (PK)
├─ ClassId: int (FK) → Class
├─ Name: string
├─ Sex: SexType
├─ Dob: DateTime?
├─ Contact: string?
├─ EnrolledAt: DateTime
│
└─ Relations:
   ├─ Class → Class
   ├─ ParentStudents → ParentStudent (1:Many)
   ├─ Attendances → Attendance (1:Many)
   ├─ GradebookEntries → GradebookEntry (1:Many)
   ├─ MonthlyScores → MonthlyScore (1:Many)
   ├─ SemesterScores → SemesterScore (1:Many)
   ├─ YearlyScores → YearlyScore (1:Many)
   ├─ MonthlyReportEntries → MonthlyReportEntry (1:Many)
   ├─ SemesterReportEntries → SemesterReportEntry (1:Many)
   └─ YearlyReportEntries → YearlyReportEntry (1:Many)
```

**Purpose**: Represent individual students

---

#### 5.2.8 Supporting Models

**Attendance**
```
├─ StudentId, ClassSubjectId, Date, Status
└─ Index: (StudentId, ClassSubjectId, Date)
```

**GradebookEntry**
```
├─ ClassSubjectId, StudentId, Label, MaxScore, Score, EntryDate
```

**MonthlyScore, SemesterScore, YearlyScore**
```
├─ StudentId, ClassSubjectId, Month/Semester/Year, FinalScore
├─ MonthlyScore: IsLocked, SubmittedBy, SubmittedAt
└─ Unique: (StudentId, ClassSubjectId, Period, Year)
```

---

## 6. Programming Structure

### 6.1 Unified Modeling Language (UML)

#### 6.1.1 Use Case Diagram

```
┌─────────────────────────────────────────────────────────┐
│                      SchoolSystem                        │
├─────────────────────────────────────────────────────────┤
│                                                          │
│  ┌──────────────┐                                       │
│  │  SuperAdmin  │                                       │
│  └──────┬───────┘                                       │
│         │                                               │
│         ├─ Manage Users                                │
│         ├─ Assign Roles                                │
│         ├─ Create Grades/Classes/Subjects              │
│         └─ View System Reports                         │
│                                                          │
│  ┌──────────────┐                                       │
│  │   Teacher    │                                       │
│  └──────┬───────┘                                       │
│         │                                               │
│         ├─ Record Attendance                           │
│         ├─ Enter Gradebook                            │
│         ├─ Submit Monthly Scores                      │
│         └─ View Student Performance                   │
│                                                          │
│  ┌──────────────┐                                       │
│  │   Homeroom   │                                       │
│  └──────┬───────┘                                       │
│         │                                               │
│         ├─ Manage Class                                │
│         ├─ View Class Scores                           │
│         ├─ Lock/Unlock Scores                         │
│         ├─ Generate Reports                           │
│         └─ View Feedback                              │
│                                                          │
│  ┌──────────────┐                                       │
│  │    Parent    │                                       │
│  └──────┬───────┘                                       │
│         │                                               │
│         ├─ View Child Reports                         │
│         ├─ Submit Feedback                            │
│         └─ View Attendance                            │
│                                                          │
└─────────────────────────────────────────────────────────┘
```

#### 6.1.2 Class Diagram (Simplified)

```
┌─────────────────────┐
│      User           │
├─────────────────────┤
│ - Id: int           │
│ - Name: string      │
│ - PasswordHash      │
│ - IsActive: bool    │
├─────────────────────┤
│ + Login()           │
│ + ChangePassword()  │
└──────────┬──────────┘
           │ (1:Many)
           │
           ▼
┌──────────────────────┐      ┌──────────────────┐
│  UserRole            │──────│ Role             │
├──────────────────────┤      ├──────────────────┤
│ - UserId (FK)        │      │ - Id: int        │
│ - RoleId (FK)        │      │ - Name: RoleName │
└──────────────────────┘      └──────────────────┘

┌──────────────────┐          ┌──────────────────┐
│    Student       │          │    Subject       │
├──────────────────┤          ├──────────────────┤
│ - Id: int        │          │ - Id: int        │
│ - Name: string   │          │ - Name: string   │
│ - ClassId (FK)   │          └──────────────────┘
└────────┬─────────┘                  │
         │ (1:Many)                   │ (1:Many)
         │                            │
         ▼                            ▼
    ┌────────────┐          ┌──────────────────┐
    │ Class      │──────────│ ClassSubject     │
    ├────────────┤          ├──────────────────┤
    │ - Id       │          │ - ClassId (FK)   │
    │ - Name     │          │ - SubjectId (FK) │
    │ - GradeId  │          │ - TeacherId (FK) │
    └────────────┘          └────────┬─────────┘
                                     │ (1:Many)
                  ┌──────────────────┼──────────────────┐
                  │                  │                  │
                  ▼                  ▼                  ▼
          ┌───────────────┐  ┌───────────────┐  ┌─────────────┐
          │ Attendance    │  │ GradebookEntry│  │ MonthlyScore│
          ├───────────────┤  ├───────────────┤  ├─────────────┤
          │ - StudentId   │  │ - StudentId   │  │ - StudentId │
          │ - Date        │  │ - Score       │  │ - Score     │
          │ - Status      │  │ - MaxScore    │  │ - Month     │
          └───────────────┘  └───────────────┘  └─────────────┘
```

#### 6.1.3 Sequence Diagram - Submit Monthly Score

```
Teacher          API              Service           Database
   │              │                  │                  │
   │─ Submit ─────>│                  │                  │
   │ Score Form    │ ─ Validate ─────>│                  │
   │               │                  │ ─ Query ────────>│
   │               │                  │ Gradebook       │
   │               │                  │<──── Return ────│
   │               │                  │ Entries          │
   │               │<─ Calculate ──────│ Average          │
   │               │                  │                  │
   │               │ ─ Save ──────────>│ ─ Insert ──────>│
   │               │                  │ MonthlyScore    │
   │               │                  │<─── Return ID ──│
   │               │<──── Return ──────│ Saved Record    │
   │<──── 201 ─────│ Created Response  │                  │
   │ Success       │                  │                  │
   │               │                  │                  │
```

---

### 6.2 Architecture

#### 6.2.1 Project Structure

```
SchoolSystem/
│
├── SchoolSystem.Core/                    # Shared Library
│   ├── Models/                           # Entity models
│   │   ├── User.cs
│   │   ├── Role.cs
│   │   ├── Grade.cs
│   │   ├── Class.cs
│   │   ├── Student.cs
│   │   ├── Subject.cs
│   │   ├── ClassSubject.cs
│   │   ├── Attendance.cs
│   │   ├── GradebookEntry.cs
│   │   ├── MonthlyScore.cs
│   │   ├── SemesterScore.cs
│   │   ├── YearlyScore.cs
│   │   ├── MonthlyReport.cs
│   │   ├── SemesterReport.cs
│   │   ├── YearlyReport.cs
│   │   ├── *ReportEntry.cs
│   │   ├── ParentStudent.cs
│   │   └── Feedback.cs
│   │
│   ├── DTOs/                             # Data Transfer Objects
│   │   ├── User/
│   │   │   ├── LoginRequest.cs
│   │   │   ├── UserDto.cs
│   │   │   └── CreateUserRequest.cs
│   │   ├── Score/
│   │   ├── Report/
│   │   └── ...
│   │
│   ├── Interfaces/                       # Service contracts
│   │   ├── IUserService.cs
│   │   ├── IGradeService.cs
│   │   ├── IStudentService.cs
│   │   ├── IAttendanceService.cs
│   │   ├── IGradebookService.cs
│   │   ├── IMonthlyScoreService.cs
│   │   ├── IReportService.cs
│   │   └── IFeedbackService.cs
│   │
│   ├── Enums/                            # System enumerations
│   │   ├── RoleName.cs (SuperAdmin, Teacher, Homeroom, Parent)
│   │   ├── AttendanceStatus.cs (Present, InformedAbsent, UninformedAbsent)
│   │   ├── ReportType.cs (Monthly, Semester, Yearly)
│   │   └── SexType.cs (Male, Female)
│   │
│   ├── Exceptions/                       # Custom exceptions
│   │   ├── UnauthorizedException.cs
│   │   ├── ResourceNotFoundException.cs
│   │   └── InvalidOperationException.cs
│   │
│   └── Extensions/                       # Helper extensions
│       ├── StringExtensions.cs
│       └── ...
│
├── SchoolSystem.Api/                     # REST API
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── UsersController.cs
│   │   ├── GradesController.cs
│   │   ├── ClassesController.cs
│   │   ├── SubjectsController.cs
│   │   ├── ClassSubjectsController.cs
│   │   ├── StudentsController.cs
│   │   ├── AttendanceController.cs
│   │   ├── GradebookController.cs
│   │   ├── ScoresController.cs
│   │   ├── ReportsController.cs
│   │   └── FeedbackController.cs
│   │
│   ├── Services/                         # Business logic
│   │   ├── AuthService.cs
│   │   ├── UserService.cs
│   │   ├── AcademicService.cs
│   │   ├── StudentService.cs
│   │   ├── AttendanceService.cs
│   │   ├── GradebookService.cs
│   │   ├── MonthlyScoreService.cs
│   │   ├── SemesterScoreService.cs
│   │   ├── YearlyScoreService.cs
│   │   ├── MonthlyReportService.cs
│   │   ├── SemesterReportService.cs
│   │   ├── YearlyReportService.cs
│   │   └── FeedbackService.cs
│   │
│   ├── Data/
│   │   ├── SchoolSystemDbContext.cs      # Entity Framework DbContext
│   │   └── Migrations/                    # Database migrations
│   │       ├── 001_InitialCreate.cs
│   │       ├── 002_AddUserRoles.cs
│   │       └─ ...
│   │
│   ├── Handlers/                         # Authorization handlers
│   │   ├── RoleAuthorizationHandler.cs
│   │   └── SuperAdminRequirement.cs
│   │
│   ├── Middleware/                       # Custom middleware
│   │   ├── ErrorHandlingMiddleware.cs
│   │   └── LoggingMiddleware.cs
│   │
│   ├── Program.cs                        # Configuration & DI
│   └── appsettings.json                  # Settings
│
├── SchoolSystem.Web/                     # Razor Pages (Phase III)
│   ├── Pages/
│   │   ├── Auth/
│   │   ├── Dashboard/
│   │   ├── Reports/
│   │   ├── Feedback/
│   │   └── Shared/
│   ├── Models/
│   ├── appsettings.json
│   └── Program.cs
│
├── SchoolSystem.Desktop/                 # WinForms (Phase II)
│   ├── Forms/
│   ├── Services/
│   ├── Models/
│   └── Program.cs
│
└── SchoolSystem.Tests/                   # Unit & Integration Tests
    ├── UnitTests/
    ├── IntegrationTests/
    └── Fixtures/
```

#### 6.2.2 Three-Layer Architecture

```
┌─────────────────────────────────────────────────────────┐
│              PRESENTATION LAYER                         │
│   (Razor Pages Web + Desktop WinForms + REST API)       │
├─────────────────────────────────────────────────────────┤
│                                                          │
│   Web UI          Desktop UI           REST API         │
│   (Razor Pages)   (WinForms)           (Controllers)    │
│        │               │                    │            │
│        └───────────────┴────────────────────┘            │
│                      │                                  │
├─────────────────────────────────────────────────────────┤
│              BUSINESS LOGIC LAYER                        │
│         (Services - All Business Rules)                 │
├─────────────────────────────────────────────────────────┤
│                                                          │
│  ┌────────────────┐  ┌────────────────┐                │
│  │   AuthService  │  │  UserService   │                │
│  ├────────────────┤  ├────────────────┤                │
│  │ + Login()      │  │ + GetUser()    │                │
│  │ + Validate()   │  │ + CreateUser() │                │
│  │ + GenerateJWT()│  │ + UpdateUser() │                │
│  └────────────────┘  └────────────────┘                │
│                                                          │
│  ┌──────────────────────────────────────────────────┐   │
│  │  AcademicService (Grades, Classes, Subjects)    │   │
│  │  StudentService                                  │   │
│  │  AttendanceService                              │   │
│  │  GradebookService                               │   │
│  │  MonthlyScoreService, SemesterScoreService, etc.│   │
│  │  ReportServices (Monthly, Semester, Yearly)    │   │
│  │  FeedbackService                                │   │
│  └──────────────────────────────────────────────────┘   │
│                      │                                  │
├─────────────────────────────────────────────────────────┤
│              DATA ACCESS LAYER                          │
│     (Entity Framework Core + PostgreSQL)                │
├─────────────────────────────────────────────────────────┤
│                                                          │
│  ┌─────────────────────────────────────────────────┐   │
│  │     SchoolSystemDbContext                       │   │
│  │  (Entity models + DbSets + Configuration)       │   │
│  └──────────────┬──────────────────────────────────┘   │
│                 │                                       │
│                 ▼                                       │
│         PostgreSQL Database                            │
│         (school_system)                                │
│                                                          │
└─────────────────────────────────────────────────────────┘
```

#### 6.2.3 Dependency Injection & Service Registration

```csharp
// Program.cs configuration example

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAcademicService, AcademicService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<IGradebookService, GradebookService>();
builder.Services.AddScoped<IMonthlyScoreService, MonthlyScoreService>();
builder.Services.AddScoped<ISemesterScoreService, SemesterScoreService>();
builder.Services.AddScoped<IYearlyScoreService, YearlyScoreService>();
builder.Services.AddScoped<IMonthlyReportService, MonthlyReportService>();
builder.Services.AddScoped<ISemesterReportService, SemesterReportService>();
builder.Services.AddScoped<IYearlyReportService, YearlyReportService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
```

---

## 7. User Interface and User Experience (UI/UX)

### 7.1 Desktop User Interfaces (Concepts)

#### 7.1.1 SuperAdmin Dashboard
```
┌─────────────────────────────────────────────────────┐
│  SchoolSystem - Administrator Dashboard             │
├─────────────────────────────────────────────────────┤
│                                                      │
│  Welcome, Admin | Logout                           │
│  ─────────────────────────────────────────────────│
│                                                      │
│  Quick Stats:                                       │
│  ├─ Total Users: 145                                │
│  ├─ Active Classes: 12                              │
│  ├─ Students Enrolled: 480                          │
│  └─ Reports Generated: 1,250                        │
│                                                      │
│  ┌─ Main Menu ─┐                                   │
│  │ Users       │  [Create] [View] [Edit] [Delete]  │
│  │ Roles       │  [Manage Roles]                   │
│  │ Grades      │  [View Grades]                    │
│  │ Classes     │  [Create] [Manage]                │
│  │ Subjects    │  [Create] [Manage]                │
│  │ Reports     │  [System Reports]                 │
│  └─────────────┘                                   │
│                                                      │
│  Recent Activity:                                   │
│  ├─ New user created: teacher1 (10:30 AM)          │
│  ├─ Class "10A" scores locked (9:45 AM)            │
│  ├─ Monthly report generated for Grade 10 (9:00 AM)│
│  └─ 2 new parent feedbacks submitted               │
│                                                      │
└─────────────────────────────────────────────────────┘
```

#### 7.1.2 Teacher Interface - Gradebook Entry
```
┌──────────────────────────────────────────────────┐
│  Gradebook Entry - Mathematics (Class 10A)       │
├──────────────────────────────────────────────────┤
│  Teacher: John Smith | Period: January 2024      │
│                                                   │
│  ┌─ Student Selection ──────────────────────────┐│
│  │ Class: 10A  [▼] | Select Student: [▼]       ││
│  └─────────────────────────────────────────────┘│
│                                                   │
│  Current Entries for: Ahmed Mohamed              │
│  ─────────────────────────────────────────────  │
│  │ Label      │ Max Score │ Score │ Date     │  │
│  ├────────────┼───────────┼───────┼──────────┤  │
│  │ Quiz 1     │    10     │   8   │ Jan 5   │  │
│  │ Quiz 2     │    10     │   9   │ Jan 12  │  │
│  │ Assignment │    20     │  19   │ Jan 15  │  │
│  │ Midterm    │   100     │  78   │ Jan 25  │  │
│  └────────────┴───────────┴───────┴──────────┘  │
│                                                   │
│  Add New Entry:                                  │
│  ├─ Label: [_______________]                    │
│  ├─ Max Score: [_____] Score: [_____]          │
│  ├─ Entry Date: [2024-01-25] [Pick Date]      │
│  └─ [Save Entry] [Reset]                        │
│                                                   │
│  Monthly Score Summary (Jan):                    │
│  Total Points: 114 / 140 = 81.43%               │
│  [Submit Monthly Score] [Print]                 │
│                                                   │
└──────────────────────────────────────────────────┘
```

#### 7.1.3 Homeroom Teacher - Report Management
```
┌──────────────────────────────────────────────────┐
│  Homeroom Dashboard - Class 10A                   │
├──────────────────────────────────────────────────┤
│  Homeroom Teacher: Mrs. Sophia | School Year 2024│
│                                                   │
│  Class Overview:                                 │
│  ├─ Total Students: 50                           │
│  ├─ Average Score: 78.2                          │
│  ├─ Attendance Rate: 94.3%                       │
│  └─ Reports Status: 2 Pending, 8 Locked         │
│                                                   │
│  Actions:                                        │
│  ┌─────────────────────────────────────────────┐│
│  │ [Generate Monthly Report]                  ││
│  │ [Generate Semester Report]                 ││
│  │ [Lock All Monthly Scores]                  ││
│  │ [View Student Performance]                 ││
│  │ [Review Parent Feedback]                   ││
│  └─────────────────────────────────────────────┘│
│                                                   │
│  Score Locking Status:                           │
│  ├─ January: LOCKED (by Mrs. Sophia - 31 Jan)  │
│  ├─ February: LOCKED (by Mrs. Sophia - 28 Feb) │
│  ├─ March: LOCKED (by Mrs. Sophia - 31 Mar)    │
│  ├─ April: UNLOCKED [Lock] [Review]             │
│  └─ May: UNLOCKED [Lock] [Review]               │
│                                                   │
│  Recent Feedback: 5 parent comments this month  │
│  [View All Feedback]                             │
│                                                   │
└──────────────────────────────────────────────────┘
```

---

### 7.2 Web User Interfaces (Implementation)

#### 7.2.1 Parent Dashboard (Razor Pages)

**Responsive Bootstrap Layout**:
```
┌─────────────────────────────────────────┐
│  SchoolSystem - Parent Portal           │
├─────────────────────────────────────────┤
│  Welcome, Ahmed's Parent                │
│  [Profile] [Settings] [Logout]          │
├─────────────────────────────────────────┤
│                                          │
│  My Children:                           │
│                                          │
│  ┌──────────────────────────────────┐   │
│  │ Student: Ahmed Mohamed           │   │
│  │ Class: 10A | Grade: 10           │   │
│  │ School Year: 2024                │   │
│  │                                  │   │
│  │ Latest Report: January (Monthly) │   │
│  │ Average Score: 82.5/100          │   │
│  │ Ranking: 3rd out of 50           │   │
│  │                                  │   │
│  │ [View Monthly] [View Semester]   │   │
│  │ [View Yearly]  [Submit Feedback] │   │
│  └──────────────────────────────────┘   │
│                                          │
│  ┌──────────────────────────────────┐   │
│  │ Student: Sofia Mohamed           │   │
│  │ Class: 10B | Grade: 10           │   │
│  │ School Year: 2024                │   │
│  │                                  │   │
│  │ Latest Report: January (Monthly) │   │
│  │ Average Score: 84.0/100          │   │
│  │ Ranking: 2nd out of 48           │   │
│  │                                  │   │
│  │ [View Monthly] [View Semester]   │   │
│  │ [View Yearly]  [Submit Feedback] │   │
│  └──────────────────────────────────┘   │
│                                          │
└─────────────────────────────────────────┘
```

#### 7.2.2 Report Viewing Page

```
┌──────────────────────────────────────────┐
│  Student Report - Ahmed Mohamed          │
├──────────────────────────────────────────┤
│  Class: 10A | School Year: 2024          │
│  Report Type: Monthly | Month: January   │
│  Submitted: 31 Jan 2024 by Mrs. Sophia   │
├──────────────────────────────────────────┤
│                                           │
│  Subject Scores:                         │
│                                           │
│  ┌─ Mathematics ─────────────────────┐  │
│  │ Score: 82.5                       │  │
│  │ Grade: A (Excellent)              │  │
│  │ Status: ████████░░ 82.5%          │  │
│  └───────────────────────────────────┘  │
│                                           │
│  ┌─ English ──────────────────────────┐  │
│  │ Score: 78.0                       │  │
│  │ Grade: B (Good)                   │  │
│  │ Status: ███████░░░ 78.0%          │  │
│  └───────────────────────────────────┘  │
│                                           │
│  ┌─ Physics ──────────────────────────┐  │
│  │ Score: 85.0                       │  │
│  │ Grade: A (Excellent)              │  │
│  │ Status: ████████░░ 85.0%          │  │
│  └───────────────────────────────────┘  │
│                                           │
│  Class Performance:                      │
│  ├─ Student Ranking: 3rd out of 50     │
│  ├─ Class Average: 80.2                 │
│  ├─ Attendance: 95% (19/20 days)        │
│  └─ Overall Average: 81.83              │
│                                           │
│  Report Summary:                         │
│  "Ahmed demonstrates strong performance │
│   in Mathematics and Physics with solid │
│   improvement in English. Attendance is  │
│   excellent. Keep up the good work!"    │
│                                           │
│  ┌─ Parent Feedback ──────────────────┐  │
│  │ [Submit Feedback]                  │  │
│  │ [Print Report]  [Download PDF]     │  │
│  └────────────────────────────────────┘  │
│                                           │
└──────────────────────────────────────────┘
```

#### 7.2.3 Feedback Submission Form

```
┌──────────────────────────────────────────┐
│  Submit Feedback                         │
├──────────────────────────────────────────┤
│                                           │
│  Report Selection:                       │
│  ├─ Report Type: [Monthly ▼]            │
│  ├─ Student: [Ahmed Mohamed ▼]          │
│  ├─ Report: [January 2024 ▼]            │
│  │  (Last report submission: 31 Jan)    │
│  │                                       │
│  │  Current Report Preview:              │
│  │  Average Score: 82.5                  │
│  │  Ranking: 3/50                        │
│  │  Status: Locked                       │
│  │                                       │
│  └─────────────────────────────────────┘  │
│                                           │
│  Feedback Content:                       │
│  ┌─────────────────────────────────────┐ │
│  │ Great improvement this month! Ahmed│ │
│  │ is showing excellent progress in   │ │
│  │ Mathematics. We're proud of his    │ │
│  │ commitment to learning.            │ │
│  │                                   │ │
│  │ Character limit: 254/500          │ │
│  └─────────────────────────────────────┘ │
│                                           │
│  [Submit Feedback]  [Clear]  [Cancel]   │
│                                           │
│  ✓ Feedback submitted successfully!      │
│                                           │
└──────────────────────────────────────────┘
```

---

## 8. Database Implementation

### 8.1 PostgreSQL Schema Overview

#### 8.1.1 Core Tables

**users** table:
```sql
CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE,
    sex VARCHAR(10) NOT NULL,
    dob DATE,
    contact VARCHAR(255),
    password_hash VARCHAR(255) NOT NULL,
    is_active BOOLEAN DEFAULT true,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

**roles** table:
```sql
CREATE TABLE roles (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE
    -- Values: SuperAdmin, Teacher, Homeroom, Parent
);
```

**user_roles** table (Many-to-Many):
```sql
CREATE TABLE user_roles (
    user_id INTEGER NOT NULL,
    role_id INTEGER NOT NULL,
    PRIMARY KEY (user_id, role_id),
    FOREIGN KEY (user_id) REFERENCES users(id),
    FOREIGN KEY (role_id) REFERENCES roles(id)
);
```

---

#### 8.1.2 Academic Structure Tables

**grades** table:
```sql
CREATE TABLE grades (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE
);
```

**classes** table:
```sql
CREATE TABLE classes (
    id SERIAL PRIMARY KEY,
    grade_id INTEGER NOT NULL,
    school_year SMALLINT NOT NULL,
    name VARCHAR(50) NOT NULL,
    homeroom_user_id INTEGER,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE (grade_id, school_year, name),
    FOREIGN KEY (grade_id) REFERENCES grades(id),
    FOREIGN KEY (homeroom_user_id) REFERENCES users(id)
);
```

**subjects** table:
```sql
CREATE TABLE subjects (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE
);
```

**class_subjects** table:
```sql
CREATE TABLE class_subjects (
    id SERIAL PRIMARY KEY,
    class_id INTEGER NOT NULL,
    subject_id INTEGER NOT NULL,
    teacher_user_id INTEGER,
    UNIQUE (class_id, subject_id),
    FOREIGN KEY (class_id) REFERENCES classes(id),
    FOREIGN KEY (subject_id) REFERENCES subjects(id),
    FOREIGN KEY (teacher_user_id) REFERENCES users(id)
);
```

---

#### 8.1.3 Student & Performance Tables

**students** table:
```sql
CREATE TABLE students (
    id SERIAL PRIMARY KEY,
    class_id INTEGER NOT NULL,
    name VARCHAR(100) NOT NULL,
    sex VARCHAR(10) NOT NULL,
    dob DATE,
    contact VARCHAR(255),
    enrolled_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (class_id) REFERENCES classes(id)
);
```

**attendance** table:
```sql
CREATE TABLE attendance (
    id SERIAL PRIMARY KEY,
    student_id INTEGER NOT NULL,
    class_subject_id INTEGER NOT NULL,
    date DATE NOT NULL,
    status VARCHAR(20) NOT NULL,
    INDEX idx_student_subject_date (student_id, class_subject_id, date),
    FOREIGN KEY (student_id) REFERENCES students(id),
    FOREIGN KEY (class_subject_id) REFERENCES class_subjects(id)
);
```

**gradebook_entries** table:
```sql
CREATE TABLE gradebook_entries (
    id SERIAL PRIMARY KEY,
    class_subject_id INTEGER NOT NULL,
    student_id INTEGER NOT NULL,
    label VARCHAR(100) NOT NULL,
    max_score DECIMAL(10,2) NOT NULL,
    score DECIMAL(10,2) NOT NULL,
    entry_date DATE NOT NULL,
    FOREIGN KEY (class_subject_id) REFERENCES class_subjects(id),
    FOREIGN KEY (student_id) REFERENCES students(id)
);
```

---

#### 8.1.4 Scoring Tables

**monthly_scores** table:
```sql
CREATE TABLE monthly_scores (
    id SERIAL PRIMARY KEY,
    student_id INTEGER NOT NULL,
    class_subject_id INTEGER NOT NULL,
    month SMALLINT NOT NULL,
    school_year SMALLINT NOT NULL,
    final_score DECIMAL(10,2) NOT NULL,
    submitted_at TIMESTAMP,
    submitted_by INTEGER,
    is_locked BOOLEAN DEFAULT false,
    UNIQUE (student_id, class_subject_id, month, school_year),
    FOREIGN KEY (student_id) REFERENCES students(id),
    FOREIGN KEY (class_subject_id) REFERENCES class_subjects(id),
    FOREIGN KEY (submitted_by) REFERENCES users(id)
);
```

**semester_scores** table:
```sql
CREATE TABLE semester_scores (
    id SERIAL PRIMARY KEY,
    student_id INTEGER NOT NULL,
    class_subject_id INTEGER NOT NULL,
    semester SMALLINT NOT NULL,
    school_year SMALLINT NOT NULL,
    final_score DECIMAL(10,2) NOT NULL,
    UNIQUE (student_id, class_subject_id, semester, school_year),
    FOREIGN KEY (student_id) REFERENCES students(id),
    FOREIGN KEY (class_subject_id) REFERENCES class_subjects(id)
);
```

**yearly_scores** table:
```sql
CREATE TABLE yearly_scores (
    id SERIAL PRIMARY KEY,
    student_id INTEGER NOT NULL,
    class_subject_id INTEGER NOT NULL,
    school_year SMALLINT NOT NULL,
    final_score DECIMAL(10,2) NOT NULL,
    UNIQUE (student_id, class_subject_id, school_year),
    FOREIGN KEY (student_id) REFERENCES students(id),
    FOREIGN KEY (class_subject_id) REFERENCES class_subjects(id)
);
```

---

#### 8.1.5 Report Tables

**monthly_reports** table:
```sql
CREATE TABLE monthly_reports (
    id SERIAL PRIMARY KEY,
    class_id INTEGER NOT NULL,
    month SMALLINT NOT NULL,
    school_year SMALLINT NOT NULL,
    submitted_by INTEGER,
    submitted_at TIMESTAMP,
    FOREIGN KEY (class_id) REFERENCES classes(id),
    FOREIGN KEY (submitted_by) REFERENCES users(id)
);
```

**monthly_report_entries** table:
```sql
CREATE TABLE monthly_report_entries (
    id SERIAL PRIMARY KEY,
    report_id INTEGER NOT NULL,
    student_id INTEGER NOT NULL,
    summary TEXT,
    average_score DECIMAL(10,2),
    ranking INTEGER,
    FOREIGN KEY (report_id) REFERENCES monthly_reports(id),
    FOREIGN KEY (student_id) REFERENCES students(id)
);
```

Similar structures for `semester_reports`, `semester_report_entries`, `yearly_reports`, `yearly_report_entries`.

---

#### 8.1.6 Support Tables

**parent_students** table:
```sql
CREATE TABLE parent_students (
    id SERIAL PRIMARY KEY,
    parent_user_id INTEGER NOT NULL,
    student_id INTEGER NOT NULL,
    UNIQUE (parent_user_id, student_id),
    FOREIGN KEY (parent_user_id) REFERENCES users(id),
    FOREIGN KEY (student_id) REFERENCES students(id)
);
```

**feedback** table:
```sql
CREATE TABLE feedback (
    id SERIAL PRIMARY KEY,
    report_type VARCHAR(20) NOT NULL,
    monthly_report_id INTEGER,
    semester_report_id INTEGER,
    yearly_report_id INTEGER,
    parent_user_id INTEGER NOT NULL,
    content TEXT NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (monthly_report_id) REFERENCES monthly_reports(id),
    FOREIGN KEY (semester_report_id) REFERENCES semester_reports(id),
    FOREIGN KEY (yearly_report_id) REFERENCES yearly_reports(id),
    FOREIGN KEY (parent_user_id) REFERENCES users(id)
);
```

---

### 8.2 Indexes for Performance

```sql
-- User lookups
CREATE INDEX idx_users_name ON users(name);
CREATE INDEX idx_users_is_active ON users(is_active);

-- Student lookups
CREATE INDEX idx_students_class_id ON students(class_id);

-- Attendance lookups
CREATE INDEX idx_attendance_student_classsubject_date 
    ON attendance(student_id, class_subject_id, date);

-- Score lookups
CREATE INDEX idx_monthly_scores_student_month 
    ON monthly_scores(student_id, month, school_year);
CREATE INDEX idx_semester_scores_student_semester 
    ON semester_scores(student_id, semester, school_year);
CREATE INDEX idx_yearly_scores_student_year 
    ON yearly_scores(student_id, school_year);

-- Report lookups
CREATE INDEX idx_monthly_reports_class_month 
    ON monthly_reports(class_id, month, school_year);
```

---

### 8.3 Data Integrity Constraints

```
CHECK Constraints:
- Users.Dob <= CURRENT_DATE (no future dates)
- Users.PasswordHash must not be NULL or empty
- Attendance.Status IN ('Present', 'InformedAbsent', 'UninformedAbsent')
- Scores >= 0 AND Scores <= MaxScore
- MonthlyScore.Month BETWEEN 1 AND 12
- SemesterScore.Semester IN (1, 2)
- Grade values are non-negative decimals with 2 decimal places

Foreign Key Constraints:
- All ForeignKey references have appropriate CASCADE/RESTRICT rules
- Orphan deletion prevented where needed
- Referential integrity enforced
```

---

## Summary

This project documentation provides:

1. **Clear Problem Definition**: Identifies pain points in current school management
2. **Comprehensive Development Plan**: Three phases with clear deliverables
3. **Detailed Database Design**: Entity relationships and schema implementation
4. **Programming Structure**: Architecture patterns and project organization
5. **User Interface Concepts**: Desktop and web interfaces with layouts
6. **Implementation Details**: PostgreSQL schema with indexes and constraints

The SchoolSystem is designed as a scalable, secure, and user-friendly solution for comprehensive school management with role-based access control and automated reporting.

---

**Document Version**: 1.0  
**Created**: [Current Date]  
**Target Audience**: Project Evaluators, Development Team, Stakeholders  
**Status**: Complete (Phase I Implementation)

