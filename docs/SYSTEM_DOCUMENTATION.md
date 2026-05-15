# SchoolSystem - Complete System Documentation

## Table of Contents
1. [System Overview](#system-overview)
2. [Architecture](#architecture)
3. [Database Design & Data Models](#database-design--data-models)
4. [Core Features & Logic](#core-features--logic)
5. [User Roles & Access Control](#user-roles--access-control)
6. [API Endpoints](#api-endpoints)
7. [Web Interface (Razor Pages)](#web-interface-razor-pages)
8. [Authentication & Authorization](#authentication--authorization)
9. [Workflow Examples](#workflow-examples)
10. [Technical Stack](#technical-stack)

---

## System Overview

The SchoolSystem is a comprehensive school management platform designed to manage academic operations including:
- **Student & Class Management**: Create and manage students, organize them into classes with grades
- **Subject & Curriculum**: Manage subjects and link them to classes with teacher assignments
- **Scoring System**: Three-tier scoring system (monthly, semester, yearly) for tracking academic performance
- **Attendance Tracking**: Record student attendance for each class-subject combination
- **Gradebook Management**: Teachers record individual scores via gradebook entries
- **Reporting System**: Automated generation of monthly, semester, and yearly reports
- **Feedback System**: Parents can provide feedback on reports
- **User & Access Management**: Role-based access control with multiple user types

### Core Principles
- **Role-Based Access Control**: SuperAdmin, Teacher, Homeroom, Parent
- **Three-Level Academic Structure**: Grades → Classes → Class-Subjects
- **Multi-tiered Reporting**: Individual scores → Monthly summaries → Semester summaries → Yearly summaries
- **Data Integrity**: Scores can be locked to prevent modifications
- **Separation of Concerns**: Core library, REST API, and Razor Pages web interface

---

## Architecture

### Project Structure

```
SchoolSystem/
├── SchoolSystem.Core/          # Core business logic & models (Class Library)
│   ├── Models/                 # Entity models
│   ├── DTOs/                   # Data Transfer Objects
│   ├── Interfaces/             # Service contracts
│   ├── Enums/                  # System enumerations
│   ├── Exceptions/             # Custom exceptions
│   └── Extensions/             # Helper extensions
│
├── SchoolSystem.Api/           # REST API (ASP.NET Core)
│   ├── Controllers/            # API endpoints
│   ├── Services/               # Business logic implementation
│   ├── Data/                   # DbContext & migrations
│   ├── Handlers/               # Authorization handlers
│   ├── Middleware/             # Custom middleware
│   └── Program.cs              # Configuration
│
├── SchoolSystem.Web/           # Web Interface (Razor Pages)
│   ├── Pages/                  # Razor page models & views
│   ├── Models/                 # Page-specific models
│   └── appsettings.json        # Configuration
│
└── SchoolSystem.Desktop/       # Desktop App (Future implementation)
```

### Three-Layer Architecture

1. **Data Layer**: Entity Framework Core with PostgreSQL
2. **Business Layer**: Services implementing core logic
3. **Presentation Layers**: 
   - REST API for mobile/third-party integration
   - Razor Pages for web interface

---

## Database Design & Data Models

### Entity Relationship Diagram Concept

```
User ◄─────────────► Role (Many-to-Many via UserRole)
  │
  ├─► Class (HomeroomTeacher)
  ├─► ClassSubject (Teacher)
  ├─► MonthlyScore (SubmittedByUser)
  ├─► MonthlyReport (SubmittedByUser)
  ├─► SemesterReport (SubmittedByUser)
  ├─► YearlyReport (SubmittedByUser)
  └─► Feedback (ParentUser)

Grade ◄─────── Class (1:Many)
              │
              ├─► Student (1:Many)
              │     │
              │     ├─► Attendance
              │     ├─► GradebookEntry
              │     ├─► MonthlyScore
              │     ├─► SemesterScore
              │     ├─► YearlyScore
              │     ├─► MonthlyReportEntry
              │     ├─► SemesterReportEntry
              │     ├─► YearlyReportEntry
              │     └─► ParentStudent
              │
              └─► ClassSubject (1:Many)
                    │
                    ├─► Subject (Many:1)
                    ├─► User/Teacher (Many:1)
                    ├─► Attendance (1:Many)
                    ├─► GradebookEntry (1:Many)
                    ├─► MonthlyScore (1:Many)
                    ├─► SemesterScore (1:Many)
                    └─► YearlyScore (1:Many)
```

### Core Models

#### 1. **User Model**
```
- Id: int (PK)
- Name: string (username)
- Sex: SexType (Male/Female)
- Dob: DateTime?
- Contact: string?
- PasswordHash: string (BCrypt)
- IsActive: bool

Relations:
- UserRoles: User can have multiple roles
- HomeroomClasses: User is homeroom teacher for classes
- TeachingClassSubjects: User teaches class-subjects
- MonthlyScores: User submitted scores
- MonthlyReports: User submitted reports
- ParentFeedbacks: User (parent) submitted feedbacks
- ParentStudents: User is parent of students
```

**Purpose**: Represents all system users - teachers, admins, homeroom teachers, parents

---

#### 2. **Role Model**
```
- Id: int (PK)
- Name: RoleName enum (SuperAdmin, Teacher, Homeroom, Parent)

Relations:
- UserRoles: Role assigned to users
```

**Purpose**: Define system roles and permissions

**Role Hierarchy**:
- **SuperAdmin**: Full system access, can manage users, roles, grades, classes
- **Teacher**: Can submit gradebook entries and scores for assigned subjects
- **Homeroom**: Can manage their homeroom class and submit class reports
- **Parent**: Can view their children's reports and submit feedback

---

#### 3. **Grade Model**
```
- Id: int (PK)
- Name: string (e.g., "Grade 10", "Form 4")

Relations:
- Classes: Multiple classes within a grade
```

**Purpose**: Group students by academic level

---

#### 4. **Class Model**
```
- Id: int (PK)
- GradeId: int (FK)
- SchoolYear: short (e.g., 2024)
- Name: string (e.g., "10A", "10B")
- HomeroomUserId: int? (FK) - optional teacher

Relations:
- Grade: Parent grade
- HomeroomTeacher: Teacher responsible for the class
- Students: All students in the class
- ClassSubjects: Subjects taught in this class
- MonthlyReports: Class-level monthly reports
- SemesterReports: Class-level semester reports
- YearlyReports: Class-level yearly reports
```

**Purpose**: Organize students and subjects within a grade level for a specific school year

**Example**: Class "10A" in Grade 10 for School Year 2024, with Mr. Smith as homeroom teacher

---

#### 5. **Subject Model**
```
- Id: int (PK)
- Name: string (e.g., "Mathematics", "English", "Physics")

Relations:
- ClassSubjects: Instances where subject is taught
```

**Purpose**: Catalog of all subjects available in the school

---

#### 6. **ClassSubject Model**
```
- Id: int (PK)
- ClassId: int (FK)
- SubjectId: int (FK)
- TeacherUserId: int? (FK) - optional teacher assignment

Relations:
- Class: The class this subject is taught in
- Subject: The subject
- Teacher: Teacher assigned to teach this subject
- Attendances: All attendance records
- GradebookEntries: Individual score entries
- MonthlyScores: Calculated monthly scores
- SemesterScores: Calculated semester scores
- YearlyScores: Calculated yearly scores

Unique Constraint: (ClassId, SubjectId) - no duplicate subject assignments
```

**Purpose**: Represents a subject being taught in a specific class by a specific teacher

**Example**: Mathematics taught in class 10A by Mr. Smith (ClassSubject#1)

---

#### 7. **Student Model**
```
- Id: int (PK)
- ClassId: int (FK)
- Name: string
- Sex: SexType
- Dob: DateTime?
- Contact: string?

Relations:
- Class: The class the student belongs to
- ParentStudents: Parent-student relationships
- Attendances: Attendance records
- GradebookEntries: Individual scores in gradebook
- MonthlyScores: Monthly calculated scores
- SemesterScores: Semester calculated scores
- YearlyScores: Yearly calculated scores
- MonthlyReportEntries: Entries in monthly reports
- SemesterReportEntries: Entries in semester reports
- YearlyReportEntries: Entries in yearly reports
```

**Purpose**: Represent individual students in the system

---

#### 8. **Attendance Model**
```
- Id: int (PK)
- StudentId: int (FK)
- ClassSubjectId: int (FK)
- Date: DateTime
- Status: AttendanceStatus (Present, InformedAbsent, UninformedAbsent)

Relations:
- Student: The student
- ClassSubject: The subject class where attendance is recorded

Composite Index: (StudentId, ClassSubjectId, Date)
```

**Purpose**: Track student attendance for each class-subject

**Usage**: Teachers record attendance when taking class for a specific subject

---

#### 9. **GradebookEntry Model**
```
- Id: int (PK)
- ClassSubjectId: int (FK)
- StudentId: int (FK)
- Label: string (e.g., "Quiz 1", "Midterm", "Assignment")
- MaxScore: decimal (e.g., 10, 100)
- Score: decimal (the actual score)
- EntryDate: DateTime

Relations:
- ClassSubject: The subject class
- Student: The student
```

**Purpose**: Individual score entries that feed into monthly scores

**Workflow**: 
1. Teacher records gradebook entries for students (e.g., Quiz 1 = 8/10)
2. These are aggregated into monthly scores

---

#### 10. **MonthlyScore Model**
```
- Id: int (PK)
- ClassSubjectId: int (FK)
- StudentId: int (FK)
- Month: short (1-12)
- SchoolYear: short
- FinalScore: decimal
- SubmittedAt: DateTime?
- SubmittedBy: int? (FK to User)
- IsLocked: bool (prevents editing after lock)

Relations:
- ClassSubject: The subject
- Student: The student
- SubmittedByUser: Teacher who submitted

Unique Constraint: (StudentId, ClassSubjectId, Month, SchoolYear)
```

**Purpose**: Aggregated monthly score for a student in a subject

**Logic**:
- Calculated from gradebook entries for that month
- Teachers can submit/update monthly scores
- Once locked, cannot be edited
- Used as basis for semester scores

---

#### 11. **SemesterScore Model**
```
- Id: int (PK)
- ClassSubjectId: int (FK)
- StudentId: int (FK)
- Semester: short (1 or 2)
- SchoolYear: short
- FinalScore: decimal

Relations:
- ClassSubject: The subject
- Student: The student
```

**Purpose**: Aggregated score for a semester (combination of months)

**Logic**:
- Calculated from monthly scores
- Typically: months 1-5 = Semester 1, months 6-12 = Semester 2
- Average of constituent monthly scores

---

#### 12. **YearlyScore Model**
```
- Id: int (PK)
- ClassSubjectId: int (FK)
- StudentId: int (FK)
- SchoolYear: short
- FinalScore: decimal

Relations:
- ClassSubject: The subject
- Student: The student
```

**Purpose**: Aggregated yearly score for a student in a subject

**Logic**: Average of semester 1 and semester 2 scores

---

#### 13-15. **Report Models** (MonthlyReport, SemesterReport, YearlyReport)

Each report has:
```
- Id: int (PK)
- ClassId: int (FK)
- [Period]: short (Month for monthly, Semester for semester)
- SchoolYear: short
- SubmittedBy: int? (FK to User)
- SubmittedAt: DateTime?

Relations:
- Class: The class the report is for
- SubmittedByUser: Who submitted it
- Entries: Individual student entries
- Feedbacks: Parent feedback on the report
```

**Purpose**: Class-level reports containing individual student performance data

**Hierarchy**:
```
MonthlyReportEntry (Student-specific data in monthly report)
    ↓
SemesterReportEntry (Aggregated from monthly entries)
    ↓
YearlyReportEntry (Aggregated from semester entries)
```

---

#### 16. **Report Entry Models** (MonthlyReportEntry, SemesterReportEntry, YearlyReportEntry)

```
- Id: int (PK)
- [Report]Id: int (FK)
- StudentId: int (FK)
- Summary: string (text summary of performance)
- AverageScore: decimal
- Ranking: int (rank within the class)

Relations:
- Student: The student
- Report: The parent report
```

**Purpose**: Individual entries for each student within a report, including ranking

---

#### 17. **ParentStudent Model**
```
- Id: int (PK)
- ParentUserId: int (FK)
- StudentId: int (FK)

Relations:
- Parent: The parent user
- Student: The student

Unique Constraint: (ParentUserId, StudentId)
```

**Purpose**: Define which parents are linked to which students

**Use Case**: A parent can view all their linked children's reports

---

#### 18. **Feedback Model**
```
- Id: int (PK)
- ReportType: ReportType (Monthly, Semester, Yearly)
- MonthlyReportId: int? (FK)
- SemesterReportId: int? (FK)
- YearlyReportId: int? (FK)
- ParentUserId: int (FK)
- Content: string (feedback text)

Relations:
- MonthlyReport: If feedback is on monthly report
- SemesterReport: If feedback is on semester report
- YearlyReport: If feedback is on yearly report
- Parent: The parent who submitted feedback
```

**Purpose**: Allow parents to provide feedback on reports

**Logic**: Only one report reference is populated based on ReportType

---

#### 19. **UserRole Model** (Junction Table)
```
- UserId: int (PK, FK)
- RoleId: int (PK, FK)

Relations:
- User: The user
- Role: The role

Composite Key: (UserId, RoleId)
```

**Purpose**: Implement many-to-many relationship between users and roles

---

### Database Enumerations

```csharp
public enum RoleName {
    SuperAdmin,    // Full system access
    Teacher,       // Can submit scores for assigned subjects
    Homeroom,      // Can manage homeroom class and submit reports
    Parent         // Can view children's reports and submit feedback
}

public enum AttendanceStatus {
    Present,           // Student present
    InformedAbsent,    // Student absent with advance notice
    UninformedAbsent   // Student absent without notice
}

public enum ReportType {
    Monthly,   // Report submitted for a specific month
    Semester,  // Report submitted for a semester
    Yearly     // Report submitted for the entire year
}

public enum SexType {
    Male,      // Male student/user
    Female     // Female student/user
}
```

---

## Core Features & Logic

### 1. User Management System

**Key Services**: `UserService`, `AuthService`

#### User Lifecycle
1. **Creation**: SuperAdmin creates users with name, sex, DOB, contact
2. **Role Assignment**: SuperAdmin assigns roles (Teacher, Homeroom, Parent)
3. **Status Management**: Active/Inactive toggle for enabling/disabling access
4. **Authentication**: BCrypt password hashing with JWT tokens

#### Authentication Flow
```
1. User enters credentials (Name, Password)
2. AuthService validates against database
3. BCrypt.Verify checks password
4. If valid:
   - Fetch user roles
   - Generate JWT token (7-day expiration)
   - Return token + user info
5. If invalid: Return null (unauthorized)
```

---

### 2. Academic Structure Management

**Key Services**: `AcademicService`, `StudentService`

#### Hierarchy Setup
```
Grade (e.g., "Grade 10")
  └─ Class (e.g., "10A", "10B") [for a specific school year]
      ├─ ClassSubject (Math taught by Mr. Smith)
      ├─ ClassSubject (English taught by Mrs. Jones)
      └─ Students (50 students in 10A)
```

#### Grade Management
- SuperAdmin creates grades
- Grades are immutable once classes are created

#### Class Management
- SuperAdmin creates classes for a specific grade and school year
- Assign homeroom teacher (optional)
- Classes can have multiple subjects with different teachers

#### ClassSubject Assignment
- SuperAdmin assigns subjects to classes
- Assign teacher for each subject
- Can be changed mid-year if necessary
- Creates the foundation for grading and attendance

---

### 3. Scoring & Grading System

**Key Services**: `GradebookService`, `MonthlyScoreService`, `SemesterReportService`, `YearlyReportService`

#### Three-Tier Scoring Architecture

```
LEVEL 1: GRADEBOOK ENTRIES (Individual Assessments)
  │
  ├─ Quiz 1: 8/10 (Date: Jan 5)
  ├─ Quiz 2: 9/10 (Date: Jan 12)
  ├─ Assignment: 19/20 (Date: Jan 15)
  └─ Midterm: 78/100 (Date: Jan 25)
  │
  └──► AGGREGATED → MONTHLY SCORE (January)
              │
              ├─ Total points: 114
              ├─ Max points: 140
              └─ Final Score: 81.4% (stored as decimal 81.4)
              │
              └──► SEMESTER SCORES
                      │
                      ├─ Jan Average: 81.4
                      ├─ Feb Average: 84.2
                      ├─ Mar Average: 79.8
                      ├─ Apr Average: 82.1
                      ├─ May Average: 85.3
                      │
                      └──► Semester 1 Final: 82.56 (average of Jan-May)
                      │
                      └──► YEARLY SCORE
                              │
                              ├─ Semester 1: 82.56
                              ├─ Semester 2: 84.12
                              │
                              └──► Yearly Final: 83.34 (average of both semesters)
```

#### Monthly Score Submission Workflow

```
Teacher View:
1. Navigate to subject's students
2. For each student, review gradebook entries for the month
3. Click "Submit Monthly Score"
4. System calculates score from gradebook entries
5. Teacher can override if needed
6. Submit → Score saved to database
7. Can edit if not locked
8. Homeroom teacher locks scores for the month (prevents changes)

Flow:
Teacher → GradebookEntry (month) → Aggregate → MonthlyScore.Submit() 
        → Database save
        → Lock/Unlock available
```

**Key Rules**:
- Monthly scores are calculated at month boundaries
- Once locked, cannot be edited (prevents mid-year changes)
- Only assigned teacher can submit scores
- If no gradebook entries, monthly score defaults to 0

---

#### Semester Score Calculation

```
Semester 1: Average of Months 1-5
Semester 2: Average of Months 6-12

Formula: Sum of monthly scores / number of months with scores

Example:
- January: 81.4
- February: 84.2
- March: 79.8
- April: 82.1
- May: 85.3
Semester 1 = (81.4 + 84.2 + 79.8 + 82.1 + 85.3) / 5 = 82.56
```

**Service Logic**:
```
SemesterReportService.GenerateSemesterScores()
  For each ClassSubject:
    For each Student:
      Get all MonthlyScores for this student in this subject for the semester
      Calculate average
      Create/Update SemesterScore record
      Create SemesterReportEntry with score and ranking
```

---

#### Yearly Score Calculation

```
Yearly Average = (Semester 1 Score + Semester 2 Score) / 2

Example:
- Semester 1: 82.56
- Semester 2: 84.12
Yearly = (82.56 + 84.12) / 2 = 83.34
```

---

### 4. Attendance Tracking System

**Key Services**: `AttendanceService`

#### Attendance Recording

```
Where: Each ClassSubject
When: After each class session
Who: Teacher or Homeroom teacher
What: Mark students as Present, InformedAbsent, or UninformedAbsent

Data Structure:
- StudentId
- ClassSubjectId
- Date
- Status

Example:
Student "John" in "Math (ClassSubject#1)" on 2024-01-15: Present
Student "Jane" in "Math (ClassSubject#1)" on 2024-01-15: InformedAbsent
Student "Bob" in "Math (ClassSubject#1)" on 2024-01-15: UninformedAbsent
```

#### Attendance Statistics
- Not directly calculated in scoring
- Used for reporting and analysis by homeroom teachers
- Can identify patterns of absenteeism

---

### 5. Reporting System

**Key Services**: `MonthlyReportService`, `SemesterReportService`, `YearlyReportService`

#### Report Generation Workflow

**Monthly Report Workflow**:
```
1. At end of month, homeroom teacher initiates report
2. System fetches:
   - All students in the class
   - Their monthly scores for this month (all subjects)
   - Their average attendance
3. Generate ranking (student ranking by total score)
4. Create MonthlyReport record
5. Create MonthlyReportEntry for each student with:
   - Summary text
   - Average score across subjects
   - Ranking within class
6. Mark as "SubmittedBy" homeroom teacher
```

**Semester Report Workflow**:
```
1. After end of semester, generate semester scores first
2. Create SemesterReport record
3. For each student:
   - Get their semester scores in each subject
   - Calculate average across all subjects
   - Rank students by average score
   - Create SemesterReportEntry with summary and ranking
4. Mark as submitted
```

**Yearly Report Workflow**:
```
1. After all semesters complete, generate yearly scores
2. Create YearlyReport record
3. For each student:
   - Get their yearly score in each subject
   - Calculate average across all subjects
   - Rank students by average score
   - Create YearlyReportEntry with summary and ranking
4. Mark as submitted
```

#### Report Entries
Each entry includes:
- **Student Info**: Name, ID
- **Summary**: Text description of performance (e.g., "Good progress in Mathematics")
- **Average Score**: Calculated from subject scores
- **Ranking**: Position in class (1st, 2nd, 3rd, etc.)

---

### 6. Feedback System

**Key Services**: `FeedbackService`

#### Feedback Workflow

**Parent Perspective**:
```
1. Parent logs in
2. Views dashboard with their children
3. Selects a child and views available reports
4. Clicks on a report (monthly/semester/yearly)
5. Reads report details
6. If feedback button available, clicks to add feedback
7. Enters feedback text (comments, concerns, questions)
8. Submits feedback
9. Feedback saved to database with timestamp
```

**Storage Structure**:
```
Feedback
├─ ReportType: Monthly (determines which report FK is used)
├─ MonthlyReportId: 42
├─ ParentUserId: 15 (parent who submitted)
└─ Content: "Great improvement this month! Keep up the hard work."
```

**Retrieval for Homeroom/Admin**:
```
Homeroom can view:
- All feedback on their class's reports
- See which parents commented and when
- Use for parent-teacher communication
```

---

### 7. Role-Based Access Control

**Key Files**: `RoleAuthorizationHandler`, `RoleAuthorizationMiddleware`

#### Permission Matrix

| Feature | SuperAdmin | Teacher | Homeroom | Parent |
|---------|-----------|---------|----------|--------|
| Manage Users | ✅ | ❌ | ❌ | ❌ |
| Manage Roles | ✅ | ❌ | ❌ | ❌ |
| Manage Grades/Classes | ✅ | ❌ | ❌ | ❌ |
| Manage Subjects | ✅ | ❌ | ❌ | ❌ |
| Record Gradebook | Teacher for subject | ✅ | ❌ | ❌ |
| Record Attendance | Teacher for subject | ✅ | ❌ | ❌ |
| Submit Monthly Scores | Teacher for subject | ✅ | ❌ | ❌ |
| Submit Reports | Homeroom teacher | ❌ | ✅ | ❌ |
| View Class Reports | Homeroom teacher | ❌ | ✅ | ❌ |
| View Child Reports | N/A | ❌ | ❌ | ✅ |
| Submit Feedback | Parents only | ❌ | ❌ | ✅ |
| Manage Parents | ✅ | ❌ | ❌ | ❌ |

#### Authorization Implementation

**JWT Token Claims**:
```
{
  "nameid": "5",           // User ID
  "unique_name": "john",   // Username
  "email": "john@school",  // Contact
  "role": ["Teacher", "Parent"]  // Multiple roles allowed
}
```

**Policy-Based Authorization**:
```
.AddPolicy("SuperAdminOnly", policy =>
  policy.Requirements.Add(new SuperAdminRequirement()))

.AddPolicy("TeacherOnly", policy =>
  policy.Requirements.Add(new TeacherRequirement()))

Implemented via: RoleAuthorizationHandler
```

---

## User Roles & Access Control

### Role Definitions

#### 1. SuperAdmin (Administrator)
**Responsibilities**:
- System setup and configuration
- User and role management
- Grade and class creation
- Subject management
- Teacher assignment to classes

**Capabilities**:
- Create/Edit/Delete users
- Assign and revoke roles
- Create academic structure (Grades, Classes, ClassSubjects)
- Manage system settings
- View all reports

**Web Interfaces**: 
- Users management dashboard
- Classes/Subjects management
- System administration panel

---

#### 2. Teacher (Subject Teacher)
**Responsibilities**:
- Record student attendance
- Create gradebook entries
- Submit monthly scores
- Track student progress

**Capabilities**:
- Record attendance for assigned subjects
- Enter scores in gradebook
- Submit calculated monthly scores
- View students in assigned subjects
- Edit non-locked scores

**Web Interfaces** (Desktop):
- Attendance recording interface
- Gradebook entry interface
- Score submission interface
- Student performance dashboard

**Limitations**:
- Can only access assigned classes and subjects
- Cannot access admin functions
- Cannot see reports
- Cannot submit class-level reports

---

#### 3. Homeroom (Class Homeroom Teacher)
**Responsibilities**:
- Manage the overall class
- Submit monthly/semester/yearly reports
- Coordinate with subject teachers
- Monitor class attendance patterns

**Capabilities**:
- View all students in homeroom class
- View all subject scores for the class
- Create and submit reports
- Lock/unlock monthly scores
- View parent feedback
- See attendance summary

**Web Interfaces** (Desktop):
- Class management dashboard
- Report submission interface
- Score management and locking
- Attendance overview
- Feedback review interface

**Limitations**:
- Can only access their assigned class
- Cannot modify gradebook entries
- Cannot change class structure

---

#### 4. Parent
**Responsibilities**:
- Monitor children's academic progress
- Provide feedback to teachers
- Stay informed about school activities

**Capabilities**:
- View linked children's information
- View available reports (monthly/semester/yearly)
- Read detailed report entries
- Submit feedback on reports
- See attendance summary

**Web Interfaces** (Razor Pages - Public):
- Dashboard: View linked children
- Reports page: View reports by type
- Feedback page: Submit feedback
- Report details: Read full report with rankings

**Limitations**:
- Can only see their own children's data
- Cannot modify any data
- Cannot access gradebook or scores
- Can only submit feedback, not edit others' data

---

## API Endpoints

### Architecture
- **Base URL**: `http://localhost:5041` (configured in appsettings.json)
- **Authentication**: JWT Bearer token in Authorization header
- **Response Format**: JSON
- **Pagination**: Skip/Take parameters with MaxPageSize = 100

### Authentication Endpoints

#### Login
```
POST /api/auth/login
Content-Type: application/json

Body:
{
  "name": "teacher1",
  "password": "password123"
}

Response (200):
{
  "token": "eyJhbGc...",
  "user": {
    "id": 5,
    "name": "John Smith",
    "sex": "Male",
    "dob": "1980-05-15",
    "contact": "john@school.edu",
    "isActive": true,
    "createdAt": "2024-01-01",
    "roles": ["Teacher", "Parent"]
  }
}
```

### User Management Endpoints

#### Get Users (Paginated)
```
GET /api/users?page=1&pageSize=10
Authorization: Bearer {token}

Response (200):
{
  "items": [...],
  "totalCount": 45,
  "page": 1,
  "pageSize": 10
}
```

#### Create User
```
POST /api/users
Authorization: Bearer {token}
Content-Type: application/json

Body:
{
  "name": "teacher2",
  "sex": "Female",
  "dob": "1985-03-20",
  "contact": "jane@school.edu",
  "password": "newpassword123"
}

Response (201):
{
  "id": 45,
  "name": "teacher2",
  ...
}
```

#### Assign Role to User
```
POST /api/users/{userId}/assign-role
Authorization: Bearer {token}
Content-Type: application/json

Body:
{
  "roleId": 2,  // Teacher role
  "roleName": "Teacher"  // OR use roleName instead
}

Response (200):
```

---

### Class & Subject Endpoints

#### Get Grades
```
GET /api/classes/grades
Authorization: Bearer {token}

Response (200):
[
  { "id": 1, "name": "Grade 10" },
  { "id": 2, "name": "Grade 11" }
]
```

#### Create Class
```
POST /api/classes
Authorization: Bearer {token}
Content-Type: application/json

Body:
{
  "gradeId": 1,
  "schoolYear": 2024,
  "name": "10A",
  "homeroomUserId": 5  // Teacher ID
}

Response (201):
{
  "id": 12,
  "gradeId": 1,
  "schoolYear": 2024,
  "name": "10A",
  "homeroomUserId": 5
}
```

#### Get Class Subjects
```
GET /api/class-subjects/class/{classId}
Authorization: Bearer {token}

Response (200):
[
  {
    "id": 1,
    "classId": 12,
    "subjectId": 1,
    "subjectName": "Mathematics",
    "teacherUserId": 5,
    "teacherName": "John Smith"
  },
  ...
]
```

#### Assign Subject to Class
```
POST /api/class-subjects
Authorization: Bearer {token}
Content-Type: application/json

Body:
{
  "classId": 12,
  "subjectId": 1,
  "teacherUserId": 5
}

Response (201):
```

---

### Student Endpoints

#### Get Students (Paginated)
```
GET /api/students?page=1&pageSize=20
Authorization: Bearer {token}

Response (200):
{
  "items": [
    {
      "id": 101,
      "name": "Ahmed",
      "sex": "Male",
      "classId": 12,
      "className": "10A",
      "dob": "2008-05-10",
      "contact": "ahmed@student.com"
    },
    ...
  ],
  "totalCount": 50,
  "page": 1,
  "pageSize": 20
}
```

#### Get Students by Class
```
GET /api/students/class/{classId}
Authorization: Bearer {token}

Response (200):
[
  { id: 101, name: "Ahmed", ... },
  { id: 102, name: "Sofia", ... },
  ...
]
```

#### Create Student
```
POST /api/students
Authorization: Bearer {token}
Content-Type: application/json

Body:
{
  "name": "Ahmed",
  "sex": "Male",
  "dob": "2008-05-10",
  "contact": "ahmed@student.com",
  "classId": 12
}

Response (201):
{
  "id": 101,
  ...
}
```

---

### Attendance Endpoints

#### Record Attendance
```
POST /api/attendance
Authorization: Bearer {token}
Content-Type: application/json

Body:
{
  "studentId": 101,
  "classSubjectId": 1,
  "date": "2024-01-15",
  "status": "Present"  // or "InformedAbsent", "UninformedAbsent"
}

Response (201):
```

#### Get Attendance for Student
```
GET /api/attendance/student/{studentId}
Authorization: Bearer {token}

Response (200):
[
  {
    "id": 1,
    "studentId": 101,
    "classSubjectId": 1,
    "date": "2024-01-15",
    "status": "Present"
  },
  ...
]
```

---

### Gradebook Endpoints

#### Record Gradebook Entry
```
POST /api/gradebook
Authorization: Bearer {token}
Content-Type: application/json

Body:
{
  "classSubjectId": 1,
  "studentId": 101,
  "label": "Quiz 1",
  "maxScore": 10,
  "score": 8.5,
  "entryDate": "2024-01-15"
}

Response (201):
```

#### Get Student's Gradebook
```
GET /api/gradebook/student/{studentId}
Authorization: Bearer {token}

Response (200):
[
  {
    "id": 1,
    "classSubjectId": 1,
    "label": "Quiz 1",
    "maxScore": 10,
    "score": 8.5,
    "entryDate": "2024-01-15"
  },
  ...
]
```

---

### Scoring Endpoints

#### Submit Monthly Score
```
POST /api/scores/monthly/submit
Authorization: Bearer {token}
Content-Type: application/json

Body:
{
  "studentId": 101,
  "classSubjectId": 1,
  "month": 1,        // January
  "schoolYear": 2024,
  "finalScore": 82.5
}

Response (201):
{
  "id": 1,
  "studentId": 101,
  "classSubjectId": 1,
  "month": 1,
  "schoolYear": 2024,
  "finalScore": 82.5,
  "submittedAt": "2024-01-31T10:30:00Z",
  "submittedBy": 5,
  "isLocked": false
}
```

#### Update Monthly Score
```
PUT /api/scores/monthly/{monthlyScoreId}
Authorization: Bearer {token}
Content-Type: application/json

Body:
{
  "finalScore": 84.0
}

Response (200):
```

#### Lock Monthly Score
```
POST /api/scores/monthly/{monthlyScoreId}/lock
Authorization: Bearer {token}

Response (200):
```

#### Get Monthly Scores for Student in Class
```
GET /api/scores/monthly/student/{studentId}/class/{classSubjectId}
Authorization: Bearer {token}

Response (200):
[
  { month: 1, finalScore: 82.5, isLocked: true, ... },
  { month: 2, finalScore: 84.0, isLocked: false, ... },
  ...
]
```

---

### Report Endpoints

#### Generate Monthly Reports
```
POST /api/reports/monthly/generate
Authorization: Bearer {token}
Content-Type: application/json

Body:
{
  "classId": 12,
  "month": 1,
  "schoolYear": 2024
}

Response (201):
{
  "id": 1,
  "classId": 12,
  "month": 1,
  "schoolYear": 2024,
  "submittedBy": 5,
  "submittedAt": "2024-01-31T10:30:00Z",
  "entries": [
    {
      "id": 1,
      "studentId": 101,
      "studentName": "Ahmed",
      "summary": "Good performance",
      "averageScore": 82.5,
      "ranking": 1
    },
    ...
  ]
}
```

#### Get Monthly Reports for Class
```
GET /api/reports/monthly/class/{classId}
Authorization: Bearer {token}

Response (200):
[
  { id: 1, month: 1, schoolYear: 2024, ... },
  { id: 2, month: 2, schoolYear: 2024, ... }
]
```

#### Get Semester Report
```
GET /api/reports/semester/{reportId}
Authorization: Bearer {token}

Response (200):
{
  "id": 1,
  "classId": 12,
  "semester": 1,
  "schoolYear": 2024,
  "entries": [...]
}
```

#### Get Yearly Report
```
GET /api/reports/yearly/{reportId}
Authorization: Bearer {token}

Response (200):
{
  "id": 1,
  "classId": 12,
  "schoolYear": 2024,
  "entries": [...]
}
```

---

### Feedback Endpoints

#### Submit Feedback on Report
```
POST /api/feedback
Authorization: Bearer {token}
Content-Type: application/json

Body:
{
  "reportType": "Monthly",  // or "Semester", "Yearly"
  "monthlyReportId": 1,     // Conditionally set based on reportType
  "content": "Great progress this month!"
}

Response (201):
{
  "id": 1,
  "reportType": "Monthly",
  "monthlyReportId": 1,
  "parentUserId": 25,
  "content": "Great progress this month!"
}
```

#### Get Feedback on Report
```
GET /api/feedback/report/{reportId}
Authorization: Bearer {token}

Response (200):
[
  {
    "id": 1,
    "reportType": "Monthly",
    "parentName": "Mr. Ahmed",
    "content": "Great progress!",
    "createdAt": "2024-01-31"
  },
  ...
]
```

---

## Web Interface (Razor Pages)

### Architecture
- **Framework**: ASP.NET Core Razor Pages
- **Authentication**: Cookie-based (separate from API JWT)
- **Target Users**: Parents
- **Base URL**: `http://localhost:5000` (or configured port)

### Page Structure

```
Pages/
├── Auth/
│   ├── Login.cshtml           # Parent login
│   ├── Logout.cshtml          # Logout confirmation
│   └── AccessDenied.cshtml    # 403 error page
├── Dashboard/
│   └── Index.cshtml           # Parent's linked children view
├── Reports/
│   ├── Monthly.cshtml         # View monthly reports
│   ├── Semester.cshtml        # View semester reports
│   └── Yearly.cshtml          # View yearly reports
├── Feedback/
│   ├── Index.cshtml           # Feedback submission form
│   ├── Submit.cshtml          # Submit feedback on report
│   └── History.cshtml         # View submitted feedback history
├── Shared/
│   ├── _Layout.cshtml         # Master layout
│   └── _ValidationScriptsPartial.cshtml
├── Index.cshtml               # Home/landing page
├── Privacy.cshtml             # Privacy policy
├── Error.cshtml               # Error page
└── _ViewImports.cshtml        # Shared imports
```

### Key Pages

#### 1. Authentication

**Login Page** (`Auth/Login.cshtml`):
- Username and password fields
- "Remember Me" option
- Links to privacy policy
- Redirects to dashboard on success

**Access Denied Page** (`Auth/AccessDenied.cshtml`):
- Shown when unauthorized user tries to access protected page
- Link back to home

---

#### 2. Dashboard (`Dashboard/Index.cshtml`)

**Purpose**: Show parent's linked children

**Data Displayed**:
```
For each child linked to parent:
├─ Student Name
├─ Class Name (e.g., "10A")
├─ School Year (e.g., "2024")
├─ Latest Report Summary
└─ Action Buttons:
    ├─ Monthly Report (if available)
    ├─ Semester Report (if available)
    └─ Yearly Report (if available)
```

**Page Model Logic**:
```
1. Load parent user from session/claims
2. Query ParentStudent relationships
3. For each child student:
   - Get student details
   - Get class information
   - Get latest reports (monthly, semester, yearly)
   - Calculate latest report summary
4. Render dashboard with cards for each child
```

**Example Output**:
```
MY CHILDREN

┌─────────────────────────┐
│ Ahmed (Grade 10A)       │
│ School Year: 2024       │
│ Latest: Jan Report      │
│ [Monthly] [Semester]... │
└─────────────────────────┘

┌─────────────────────────┐
│ Sofia (Grade 10B)       │
│ School Year: 2024       │
│ Latest: Jan Report      │
│ [Monthly] [Semester]... │
└─────────────────────────┘
```

---

#### 3. Reports Pages (`Reports/Monthly.cshtml`, etc.)

**Purpose**: Display academic reports for selected student

**Data Displayed**:
```
Report Header:
├─ Student Name: Ahmed
├─ Class: 10A
├─ Report Type: Monthly
├─ Month: January 2024
└─ Submitted: 31 Jan 2024

Report Entries:
├─ Subject: Mathematics
│  ├─ Score: 82.5
│  └─ Status: A (Excellent)
├─ Subject: English
│  ├─ Score: 78.0
│  └─ Status: B (Good)
└─ Subject: Physics
   ├─ Score: 85.0
   └─ Status: A (Excellent)

Class Performance:
├─ Student Ranking: 3rd out of 50
├─ Class Average: 80.5
└─ Attendance: 95%

Summary: Ahmed shows consistent improvement...
```

**Page Model Logic**:
```
1. Get studentId and reportId from route parameters
2. Verify parent is linked to this student
3. Load report from API
4. Load report entries (scores, rankings)
5. Calculate statistics (class average, attendance, etc.)
6. Render report details
```

---

#### 4. Feedback Pages

**Submit Feedback Page** (`Feedback/Submit.cshtml`):
```
Form:
├─ Report Type: [Select Monthly/Semester/Yearly]
├─ Student: [Selected from dropdown]
├─ Report: [Auto-populated based on type]
├─ Feedback Text: [Large textarea]
└─ [Submit] [Cancel]

On Submit:
1. Validate all fields
2. Call API to save feedback
3. Show success message
4. Redirect to feedback history
```

**Feedback History Page** (`Feedback/History.cshtml`):
```
Display all submitted feedbacks:
├─ Date Submitted
├─ Report Type
├─ Student Name
├─ Feedback Text
└─ [Edit] [Delete]

Filters:
├─ By Student
├─ By Date Range
└─ By Report Type
```

---

### API Integration

Each Razor Page calls the ASP.NET Core API using `HttpClient`:

```csharp
// Example from page model
public async Task OnGetAsync(int studentId)
{
    var response = await _httpClient.GetAsync(
        $"/api/students/{studentId}");

    if (response.IsSuccessStatusCode)
    {
        var json = await response.Content.ReadAsStringAsync();
        Student = JsonSerializer.Deserialize<StudentDto>(json);
    }
}
```

**Configuration**:
```json
{
  "ApiBaseUrl": "http://localhost:5041",
  "ConnectionStrings": {
    "DefaultConnection": "Host=100.87.106.38;Port=1445;..."
  }
}
```

---

## Authentication & Authorization

### JWT Token Structure

```
Header:
{
  "alg": "HS256",
  "typ": "JWT"
}

Payload:
{
  "nameid": "5",                    // User ID
  "unique_name": "john",            // Username
  "email": "john@school.edu",       // Contact email
  "role": ["Teacher", "Parent"],    // Role claims
  "iat": 1704067200,                // Issued at
  "exp": 1704672000,                // Expires in 7 days
  "iss": "SchoolSystem",            // Issuer
  "aud": "SchoolSystem"             // Audience
}

Signature: HMACSHA256(base64(header) + "." + base64(payload), secret_key)
```

### Token Configuration

```json
// appsettings.json
{
  "Jwt": {
    "Key": "YourSuperSecretKeyHere12345678901234567890",
    "Issuer": "SchoolSystem",
    "Audience": "SchoolSystem"
  }
}
```

### Authentication Flow

```
1. LOGIN REQUEST
   User → POST /api/auth/login → AuthService.LoginAsync()

2. CREDENTIALS VALIDATION
   ├─ Find user by username in database
   ├─ Use BCrypt.Verify(password, storedHash)
   └─ If invalid → Return null

3. TOKEN GENERATION
   ├─ Fetch user roles
   ├─ Create claims (ID, name, email, roles)
   ├─ Sign JWT token with secret key
   └─ Return token + user info

4. API REQUESTS WITH TOKEN
   Client adds header: Authorization: Bearer {token}
   API validates token:
   ├─ Check signature
   ├─ Verify issuer and audience
   ├─ Check expiration
   ├─ Extract claims (user ID, roles)
   └─ Allow/Deny request

5. TOKEN EXPIRATION
   └─ After 7 days → Must login again
```

### Authorization Policies

**Policy-Based Authorization**:
```csharp
// Program.cs configuration
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SuperAdminOnly", policy =>
        policy.Requirements.Add(new SuperAdminRequirement()));

    options.AddPolicy("TeacherOnly", policy =>
        policy.Requirements.Add(new TeacherRequirement()));

    // ... more policies
});

// Usage in controllers
[Authorize(Policy = "SuperAdminOnly")]
public IActionResult ManageUsers() { ... }
```

**Role-Based Authorization**:
```csharp
// In API controllers
[Authorize(Roles = "Teacher")]
public async Task<IActionResult> SubmitMonthlyScore(...) { ... }

[Authorize(Roles = "Parent")]
public async Task<IActionResult> SubmitFeedback(...) { ... }
```

---

## Workflow Examples

### Workflow 1: Recording and Submitting Student Scores

```
START: Teacher wants to record January scores for Math class 10A

STEP 1: Teacher logs in
├─ Username: john_teacher
├─ Password: ****
└─ API returns JWT token with role "Teacher"

STEP 2: Teacher opens gradebook for Math (ClassSubject #1)
├─ API: GET /api/gradebook/student/{studentId}?classSubjectId=1
└─ Shows all entries for this subject for this student

STEP 3: Teacher records gradebook entries
├─ Quiz 1 (Jan 5): 8/10
├─ Quiz 2 (Jan 12): 9/10
├─ Assignment (Jan 15): 19/20
└─ Midterm (Jan 25): 78/100
   Each entry: POST /api/gradebook
   {
     "classSubjectId": 1,
     "studentId": 101,
     "label": "Quiz 1",
     "maxScore": 10,
     "score": 8,
     "entryDate": "2024-01-05"
   }

STEP 4: Teacher submits monthly score for January
├─ System calculates: 
│  ├─ Total points: 8+9+19+78 = 114
│  ├─ Max points: 10+10+20+100 = 140
│  └─ Percentage: (114/140) * 100 = 81.43%
├─ API: POST /api/scores/monthly/submit
│  {
│    "studentId": 101,
│    "classSubjectId": 1,
│    "month": 1,
│    "schoolYear": 2024,
│    "finalScore": 81.43
│  }
└─ Response: MonthlyScore record saved, isLocked: false

STEP 5: Homeroom teacher reviews and locks scores
├─ API: POST /api/scores/monthly/{monthlyScoreId}/lock
├─ MonthlyScore.IsLocked = true
└─ No more edits allowed for this month

RESULT: 
- Monthly score stored in database
- Cannot be changed after lock
- Used for semester report calculation
```

---

### Workflow 2: Generating and Viewing Monthly Report

```
START: Homeroom teacher wants to generate January report for class 10A

STEP 1: Homeroom teacher logs in
├─ Username: homeroomA
├─ Password: ****
└─ API returns JWT token with role "Homeroom"

STEP 2: Homeroom selects class and month
├─ Class: 10A (ClassId: 12)
├─ Month: January (Month: 1)
└─ School Year: 2024

STEP 3: System generates report
├─ API: POST /api/reports/monthly/generate
│  {
│    "classId": 12,
│    "month": 1,
│    "schoolYear": 2024
│  }
├─ Creates MonthlyReport record
├─ For each student (1-50):
│  ├─ Fetch all subject scores for this month
│  ├─ Calculate average across subjects
│  ├─ Calculate ranking (sort by average)
│  └─ Create MonthlyReportEntry
└─ Example ranking:
   ├─ 1st: Ahmed (Avg 82.5)
   ├─ 2nd: Sofia (Avg 81.0)
   ├─ 3rd: Hassan (Avg 80.2)
   └─ ...

STEP 4: Report submitted
├─ MonthlyReport.SubmittedBy: 3 (Homeroom teacher ID)
├─ MonthlyReport.SubmittedAt: 2024-01-31 10:30 UTC
└─ Report now locked for view

RESULT: Report is now available for parents to view

---

LATER: Parent views report

STEP 1: Parent logs in via web interface
├─ Username: parent_ahmed
├─ Password: ****
└─ Cookie session created

STEP 2: Parent navigates to dashboard
├─ Page loads linked children
├─ Shows: Ahmed (10A) with Latest Report: January
└─ Action buttons: [Monthly] [Semester] [Yearly]

STEP 3: Parent clicks "Monthly" button
├─ Navigates to: /Reports/Monthly?studentId=101&reportId=1
├─ Page fetches report from API
│  GET /api/reports/monthly/1
└─ Displays:
   ├─ Student: Ahmed
   ├─ Class: 10A
   ├─ Month: January 2024
   ├─ Subject Scores:
   │  ├─ Math: 82.5
   │  ├─ English: 78.0
   │  └─ Physics: 85.0
   ├─ Class Average: 80.5
   ├─ Ahmed's Ranking: 3rd/50
   ├─ Attendance: 95%
   ├─ Summary: "Ahmed is showing improvement..."
   └─ [Submit Feedback] [Print] [Download]

STEP 4: Parent submits feedback (optional)
├─ API: POST /api/feedback
│  {
│    "reportType": "Monthly",
│    "monthlyReportId": 1,
│    "content": "Great progress! Keep up the good work!"
│  }
└─ Feedback stored with parent ID and timestamp

RESULT: Parent can view report and provide feedback to school
```

---

### Workflow 3: Semester Report Generation

```
TIMELINE:
├─ Months 1-5: Teachers submit monthly scores
├─ End of May: All monthly scores locked
└─ June 1: Homeroom generates semester report

START: System generates semester 1 report

STEP 1: Homeroom triggers semester report generation
├─ API: POST /api/reports/semester/generate
│  {
│    "classId": 12,
│    "semester": 1,
│    "schoolYear": 2024
│  }

STEP 2: System processes each student
For Ahmed (StudentId: 101):
├─ Get all monthly scores (Jan-May)
│  ├─ January: 81.43
│  ├─ February: 84.20
│  ├─ March: 79.80
│  ├─ April: 82.10
│  └─ May: 85.30
├─ Calculate averages per subject
│  ├─ Math: (82.5 + 83.0 + 80.0 + 82.5 + 84.0) / 5 = 82.4
│  ├─ English: (78.0 + 79.5 + 77.5 + 81.0 + 82.0) / 5 = 79.6
│  └─ Physics: (85.0 + 86.0 + 84.5 + 85.5 + 87.0) / 5 = 85.6
├─ Create SemesterScore for each subject
│  ├─ SemesterScore(Math, Sem1): 82.4
│  ├─ SemesterScore(English, Sem1): 79.6
│  └─ SemesterScore(Physics, Sem1): 85.6
├─ Calculate class average
│  └─ Ahmed's average: (82.4 + 79.6 + 85.6) / 3 = 82.53
└─ Determine ranking (Ahmed: 2nd in class)

STEP 3: Create report entries for all students
├─ SemesterReportEntry(Ahmed):
│  ├─ Summary: "Ahmed shows consistent performance..."
│  ├─ AverageScore: 82.53
│  └─ Ranking: 2
├─ SemesterReportEntry(Sofia):
│  ├─ Summary: "Sofia excels in all subjects..."
│  ├─ AverageScore: 83.50
│  └─ Ranking: 1
└─ ... (48 more students)

STEP 4: Report locked and available
├─ SemesterReport.SubmittedAt: June 1, 2024
├─ SemesterReport.SubmittedBy: 3 (Homeroom ID)
└─ Status: Locked for viewing

RESULT: Semester report ready for parent viewing
```

---

### Workflow 4: Yearly Report and Ranking

```
TIMELINE:
├─ Months 1-5: Semester 1 complete
├─ Months 6-12: Semester 2 complete
└─ January 1 (next year): Yearly report generated

START: Generate yearly report

STEP 1: Trigger yearly report generation
├─ API: POST /api/reports/yearly/generate
│  {
│    "classId": 12,
│    "schoolYear": 2024
│  }

STEP 2: For each student, calculate yearly scores
For Ahmed:
├─ Get semester 1 scores
│  ├─ Math: 82.4
│  ├─ English: 79.6
│  └─ Physics: 85.6
│  └─ Semester 1 Average: 82.53
│
├─ Get semester 2 scores
│  ├─ Math: 83.0
│  ├─ English: 81.0
│  └─ Physics: 86.0
│  └─ Semester 2 Average: 83.33
│
├─ Create YearlyScore for each subject
│  ├─ YearlyScore(Math): (82.4 + 83.0) / 2 = 82.7
│  ├─ YearlyScore(English): (79.6 + 81.0) / 2 = 80.3
│  └─ YearlyScore(Physics): (85.6 + 86.0) / 2 = 85.8
│
├─ Calculate yearly average
│  └─ (82.7 + 80.3 + 85.8) / 3 = 82.93
│
└─ Determine final ranking among all 50 students

STEP 3: Create report entries
├─ YearlyReportEntry(Ahmed):
│  ├─ Summary: "Excellent year! Ahmed improved..."
│  ├─ AverageScore: 82.93
│  └─ Ranking: 2 (out of 50)
└─ ... (49 more students)

STEP 4: Report saved and locked
└─ Status: Final report ready for archival

RESULT: Complete yearly performance record for all students
```

---

## Technical Stack

### Backend (API)
- **Framework**: ASP.NET Core 10
- **Database**: PostgreSQL with Entity Framework Core
- **Authentication**: JWT Bearer Tokens
- **API Documentation**: Swagger/OpenAPI
- **Architecture Pattern**: Service-Repository pattern

### Frontend (Web)
- **Framework**: ASP.NET Core Razor Pages
- **Authentication**: Cookie-based sessions
- **Styling**: Bootstrap 5
- **HTTP Client**: HttpClientFactory

### Desktop (Future)
- **Framework**: Windows Forms or WPF
- **Target Users**: Teachers, Homeroom, Admin
- **Features**: Gradebook entry, attendance, report viewing

### Database
- **Engine**: PostgreSQL
- **ORM**: Entity Framework Core
- **Connection**: Remote server (Host: 100.87.106.38:1445)
- **Database Name**: school_system

### Development Tools
- **Language**: C# 13
- **IDE**: Visual Studio 2026
- **Version Control**: Git
- **Package Manager**: NuGet

### Key Dependencies
- `Microsoft.EntityFrameworkCore.Npgsql`: PostgreSQL driver
- `System.IdentityModel.Tokens.Jwt`: JWT token generation
- `BCrypt.Net-Next`: Password hashing
- `Microsoft.AspNetCore.Authentication.JwtBearer`: JWT validation
- `Microsoft.OpenApi`: Swagger documentation

---

## Summary

The SchoolSystem is a comprehensive academic management platform with three distinct layers:

1. **Core Layer** (SchoolSystem.Core): 
   - Shared models, enums, DTOs, interfaces
   - No dependencies on other projects
   - Reusable across API and Desktop

2. **API Layer** (SchoolSystem.Api):
   - REST endpoints for all operations
   - Business logic services
   - Database access via EF Core
   - JWT authentication

3. **Web Layer** (SchoolSystem.Web):
   - Parent portal using Razor Pages
   - View reports and submit feedback
   - Session-based authentication
   - Calls API for data

**Key Flows**:
- Teachers record scores → Homeroom submits reports → Parents view reports → Parents submit feedback
- Scores flow: Gradebook → Monthly → Semester → Yearly
- Rankings calculated at each report level
- Complete audit trail of who submitted what and when

This architecture allows for easy addition of the Desktop interfaces for Teachers, Admins, and Homeroom teachers without changing the core logic or API.

---

## Document History

- **Created**: [Current Date]
- **Version**: 1.0
- **Project**: SchoolSystem (Final Year Project)
- **Target Audience**: Lecturers, Project Evaluators
- **Scope**: Complete system logic (excluding Desktop interfaces)

---

**END OF DOCUMENTATION**
