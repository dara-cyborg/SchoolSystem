## TASK PLAN: SCHOOL INFORMATION SYSTEM

### Stack & Convention
- .NET 10 across all projects
- Target frameworks: `net10.0` (Api, Web, Core), `net10.0-windows` (Desktop)
- ORM: Entity Framework Core (latest compatible with .NET 10)
- Database: PostgreSQL via Npgsql.EntityFrameworkCore.PostgreSQL
- Auth: JWT (Desktop/Api), Cookie session (Web portal only)
- No Repository pattern — use AppDbContext directly in services

### WinForms Component Naming Convention (Desktop Project)

All WinForms controls in `SchoolSystem.Desktop` MUST follow these naming conventions:

| Component Type | Prefix | Example |
|---|---|---|
| Button | `btn` | `btnSubmit`, `btnCancel`, `btnRefresh` |
| TextBox | `txt` | `txtUsername`, `txtEmail`, `txtSearch` |
| Label | `lbl` | `lblName`, `lblError`, `lblStatus` |
| CheckBox | `chk` | `chkIsActive`, `chkRemember` |
| Panel | `pnl` | `pnlHeader`, `pnlContent`, `pnlFooter` |
| DataGridView | `dgv` | `dgvUsers`, `dgvStudents`, `dgvAttendance` |
| ComboBox | `cbo` | `cboGrade`, `cboClass`, `cboTeacher` |
| TabControl | `tab` | `tabMain`, `tabReports` |
| StatusStrip | `ss` | `ssStatus` |
| MenuStrip | `ms` | `msMain` |
| ToolStripMenuItem | `tsmi` | `tsmiFile`, `tsmiLogout`, `tsmiUsers` |
| ToolStripStatusLabel | `tssl` | `tsslStatus`, `tsslUserInfo` |
| ToolStripButton | `tsb` | `tsbRefresh`, `tsbAdd` |
| DateTimePicker | `dtp` | `dtpStartDate`, `dtpBirthDate` |
| NumericUpDown | `nud` | `nudScore`, `nudMonth`, `nudYear` |
| GroupBox | `grp` | `grpCredentials`, `grpFilters` |
| ListBox | `lst` | `lstItems`, `lstRoles` |
| CheckedListBox | `clb` | `clbRoles`, `clbSubjects` |
| ProgressBar | `pgb` | `pgbProgress`, `pgbLoading` |
| TabPage | `tp` | `tpRoster`, `tpAttendance` |
| Form | `frm` OR form name | `frmLogin`, `frmUserDialog` (optional prefix) |
| BindingSource | `bs` | `bsUsers`, `bsStudents` |

**Rules**:
1. Always use camelCase after the prefix (first letter lowercase after prefix)
2. Use descriptive names that indicate the control's purpose
3. Do NOT use underscores or numbers at the start of the name
4. If a form is a dialog, use descriptive dialog naming with `frm` prefix (e.g., `frmUserDialog`, `frmLogin`)
5. For grid columns, use PascalCase without prefix (e.g., `StudentName`, `AttendanceStatus`)
6. All event handlers should follow the pattern: `ControlName_EventName` (e.g., `btnSubmit_Click`, `txtSearch_TextChanged`)

**Examples of Correct Naming**:
```
// Bad:
btn_submit, Button1, submitButton, SUBMIT_BTN

// Good:
btnSubmit, btnCancel, btnRefresh

// Bad:
textbox_username, UserNameTextBox, 1txtName

// Good:
txtUsername, txtEmail, txtSearch

// Bad:
DGV_Students, dataGrid_Users

// Good:
dgvStudents, dgvUsers, dgvAttendance
```

---

### Phase 1: Foundation

**T01 - Project Solution Setup and Core Library Architecture**
- Assigned to: Member A (lead)
- Phase: 1-Foundation
- Depends on: None
- Description: Create the .NET 10 solution with 4 projects (Core, Api, Web, Desktop). Set up folder structure, NuGet packages (EF Core, PostgreSQL provider, JWT, BCrypt, Serilog). Define shared constants and enums matching database (role_name, attendance_status, report_type, sex_type).
- Files to create:
  - `SchoolSystem.sln`
  - `SchoolSystem.Core/SchoolSystem.Core.csproj` — target net10.0
  - `SchoolSystem.Api/SchoolSystem.Api.csproj` — target net10.0
  - `SchoolSystem.Web/SchoolSystem.Web.csproj` — target net10.0
  - `SchoolSystem.Desktop/SchoolSystem.Desktop.csproj` — target net10.0-windows
  - `SchoolSystem.Core/Constants/Enums.cs` — RoleName, AttendanceStatus, ReportType, SexType enums
- Acceptance criteria:
  - Solution compiles without errors on .NET 10
  - All 4 projects reference Core project
  - Database enums defined in Core/Constants/Enums.cs
  - All target frameworks correctly set

---

**T02 - Core Domain Models**
- Assigned to: Member A (lead)
- Phase: 1-Foundation
- Depends on: T01
- Description: Create all domain entity classes mapping to PostgreSQL tables. Each class maps 1:1 to a DB table with correct property types, navigation properties, and FK relationships.
- Files to create:
  - `SchoolSystem.Core/Models/User.cs`
  - `SchoolSystem.Core/Models/Role.cs`
  - `SchoolSystem.Core/Models/UserRole.cs`
  - `SchoolSystem.Core/Models/Grade.cs`
  - `SchoolSystem.Core/Models/Class.cs`
  - `SchoolSystem.Core/Models/Subject.cs`
  - `SchoolSystem.Core/Models/ClassSubject.cs`
  - `SchoolSystem.Core/Models/Student.cs`
  - `SchoolSystem.Core/Models/ParentStudent.cs`
  - `SchoolSystem.Core/Models/Attendance.cs`
  - `SchoolSystem.Core/Models/GradebookEntry.cs`
  - `SchoolSystem.Core/Models/MonthlyScore.cs`
  - `SchoolSystem.Core/Models/SemesterScore.cs`
  - `SchoolSystem.Core/Models/YearlyScore.cs`
  - `SchoolSystem.Core/Models/MonthlyReport.cs`
  - `SchoolSystem.Core/Models/MonthlyReportEntry.cs`
  - `SchoolSystem.Core/Models/SemesterReport.cs`
  - `SchoolSystem.Core/Models/SemesterReportEntry.cs`
  - `SchoolSystem.Core/Models/YearlyReport.cs`
  - `SchoolSystem.Core/Models/YearlyReportEntry.cs`
  - `SchoolSystem.Core/Models/Feedback.cs`
- Acceptance criteria:
  - All 21 tables have corresponding model classes
  - Navigation properties and FK relationships properly defined
  - school_year is SMALLINT (short in C#) on all relevant models

---

**T03 - EF Core DbContext**
- Assigned to: Member A (lead)
- Phase: 1-Foundation
- Depends on: T02
- Description: Create AppDbContext with DbSets for all entities. Configure relationships, cascade delete rules, composite unique constraints using Fluent API. No repository pattern — services inject AppDbContext directly.
- Files to create:
  - `SchoolSystem.Api/Data/AppDbContext.cs`
- Files to modify:
  - `SchoolSystem.Api/appsettings.json` — add ConnectionStrings:DefaultConnection
  - `SchoolSystem.Api/Program.cs` — register AppDbContext with Npgsql provider
- Acceptance criteria:
  - AppDbContext compiles and connects to PostgreSQL
  - All 21 DbSets configured
  - Composite unique constraints match DB schema (e.g. UNIQUE(grade_id, name, school_year))
  - Cascade delete rules configured

---

**T04 - Authentication and Authorization Infrastructure**
- Assigned to: Member A (lead)
- Phase: 1-Foundation
- Depends on: T03
- Description: Implement JWT token generation and validation for Api/Desktop. Create Cookie authentication for Web portal only. Build role-based authorization. Implement ClaimsPrincipal extension for multi-role users.
- Files to create:
  - `SchoolSystem.Core/Extensions/ClaimsPrincipalExtensions.cs` — HasRole(), GetUserId(), GetUserName()
  - `SchoolSystem.Api/Services/AuthService.cs` — JWT generation, login logic
  - `SchoolSystem.Api/Controllers/AuthController.cs` — POST /api/auth/login
  - `SchoolSystem.Api/Handlers/RoleAuthorizationHandlers.cs` — custom handlers per role
- Files to modify:
  - `SchoolSystem.Api/Program.cs` — register JWT scheme, authorization policies
  - `SchoolSystem.Api/appsettings.json` — add Jwt:Key, Jwt:Issuer, Jwt:Audience
  - `SchoolSystem.Web/Program.cs` — register Cookie auth scheme only
- Acceptance criteria:
  - JWT tokens issued and validated correctly
  - Cookie auth works for Razor Pages (Web only)
  - [Authorize(Roles = "teacher")] works on Api controllers
  - User.HasRole("teacher") returns correct boolean

---

### Phase 2: API

**T05 - API Controllers: User Management**
- Assigned to: Member C
- Phase: 2-API
- Depends on: T04
- Description: Create UsersController with full CRUD. Implement password hashing with BCrypt. Add role assignment endpoint managing user_roles table. All endpoints restricted to super_admin.
- Files to create:
  - `SchoolSystem.Core/DTOs/User/UserDto.cs` — Id, Name, Sex, Dob, Contact, IsActive, CreatedAt, Roles (List<string>)
  - `SchoolSystem.Core/DTOs/User/CreateUserDto.cs` — Name, Sex, Dob, Contact, Password, RoleIds (List<int>)
  - `SchoolSystem.Core/DTOs/User/UpdateUserDto.cs` — Name, Sex, Dob, Contact, IsActive
  - `SchoolSystem.Core/DTOs/User/AssignRoleDto.cs` — RoleIds (List<int>)
  - `SchoolSystem.Core/DTOs/PagedResult.cs` — generic: Items, TotalCount, Page, PageSize, TotalPages
  - `SchoolSystem.Core/Interfaces/IUserService.cs`
  - `SchoolSystem.Api/Services/UserService.cs` — implements IUserService, injects AppDbContext
  - `SchoolSystem.Api/Controllers/UsersController.cs`
- Files to modify:
  - `SchoolSystem.Api/Program.cs` — register IUserService as scoped
- Endpoints:
  - GET    /api/users?page=1&pageSize=20   [super_admin]
  - GET    /api/users/{id}                 [super_admin]
  - POST   /api/users                      [super_admin]
  - PUT    /api/users/{id}                 [super_admin]
  - DELETE /api/users/{id}                 [super_admin]
  - POST   /api/users/{id}/roles           [super_admin]
- Acceptance criteria:
  - GET /api/users returns paginated user list with roles
  - POST /api/users hashes password before saving
  - PUT /api/users/{id} does not update password_hash
  - DELETE restricted to super_admin only

---

**T06 - API Controllers: Academic Structure (Grades, Classes, Subjects)**
- Assigned to: Member C
- Phase: 2-API
- Depends on: T04
- Description: Create controllers for Grades, Classes, Subjects, ClassSubjects. Implement teacher assignment to class_subjects. Add homeroom assignment to classes. Filter classes by grade and school_year.
- Files to create:
  - `SchoolSystem.Core/DTOs/Grade/GradeDto.cs`, `CreateGradeDto.cs`
  - `SchoolSystem.Core/DTOs/Class/ClassDto.cs`, `CreateClassDto.cs`, `UpdateClassDto.cs`
  - `SchoolSystem.Core/DTOs/Subject/SubjectDto.cs`, `CreateSubjectDto.cs`
  - `SchoolSystem.Core/DTOs/ClassSubject/ClassSubjectDto.cs`, `CreateClassSubjectDto.cs`
  - `SchoolSystem.Core/Interfaces/IAcademicService.cs`
  - `SchoolSystem.Api/Services/AcademicService.cs` — injects AppDbContext
  - `SchoolSystem.Api/Controllers/GradesController.cs`
  - `SchoolSystem.Api/Controllers/ClassesController.cs`
  - `SchoolSystem.Api/Controllers/SubjectsController.cs`
  - `SchoolSystem.Api/Controllers/ClassSubjectsController.cs`
- Files to modify:
  - `SchoolSystem.Api/Program.cs` — register IAcademicService as scoped
- Endpoints:
  - CRUD /api/grades                              [super_admin]
  - CRUD /api/classes                             [super_admin]
  - GET  /api/classes?gradeId=&schoolYear=        [super_admin, homeroom]
  - GET  /api/classes/{id}/students               [super_admin, homeroom]
  - CRUD /api/subjects                            [super_admin]
  - CRUD /api/class-subjects                      [super_admin]
- Acceptance criteria:
  - CRUD operations for all academic structure entities
  - ClassSubjects correctly assigns teacher_user_id
  - GET /api/classes/{id}/students returns correct roster
  - school_year filter works on classes endpoint

---

**T07 - API Controllers: Student Management**
- Assigned to: Member C
- Phase: 2-API
- Depends on: T05, T06
- Description: Create StudentsController for CRUD. Implement parent-student linking. Add endpoints to get students by class, by parent. Parent role can only see their own linked children.
- Files to create:
  - `SchoolSystem.Core/DTOs/Student/StudentDto.cs`, `CreateStudentDto.cs`, `UpdateStudentDto.cs`
  - `SchoolSystem.Core/DTOs/Student/LinkParentDto.cs` — ParentUserId, StudentId
  - `SchoolSystem.Core/Interfaces/IStudentService.cs`
  - `SchoolSystem.Api/Services/StudentService.cs` — injects AppDbContext
  - `SchoolSystem.Api/Controllers/StudentsController.cs`
- Files to modify:
  - `SchoolSystem.Api/Program.cs` — register IStudentService as scoped
- Endpoints:
  - CRUD /api/students                            [super_admin, homeroom]
  - GET  /api/students/class/{classId}            [super_admin, homeroom, teacher]
  - GET  /api/students/parent/{parentUserId}      [super_admin, parent]
  - POST /api/students/{id}/link-parent           [super_admin]
- Acceptance criteria:
  - CRUD for student records with class assignment
  - Parent can only retrieve their own linked children
  - GET /api/students/class/{classId} returns correct roster

---

**T08 - API Controllers: Attendance**
- Assigned to: Member C
- Phase: 2-API
- Depends on: T04
- Description: Create AttendanceController. Teacher marks attendance only for their assigned class_subjects. Bulk entry for full class. Summary endpoint returns informed/uninformed absence counts per student per period.
- Files to create:
  - `SchoolSystem.Core/DTOs/Attendance/AttendanceDto.cs`, `CreateAttendanceDto.cs`, `BulkAttendanceDto.cs`
  - `SchoolSystem.Core/DTOs/Attendance/AttendanceSummaryDto.cs` — StudentId, InformedAbsences, UninformedAbsences
  - `SchoolSystem.Core/Interfaces/IAttendanceService.cs`
  - `SchoolSystem.Api/Services/AttendanceService.cs` — injects AppDbContext
  - `SchoolSystem.Api/Controllers/AttendanceController.cs`
- Files to modify:
  - `SchoolSystem.Api/Program.cs` — register IAttendanceService as scoped
- Endpoints:
  - POST /api/attendance                                           [teacher]
  - POST /api/attendance/bulk                                      [teacher]
  - GET  /api/attendance/student/{studentId}?startDate=&endDate=  [super_admin, homeroom, teacher]
  - GET  /api/attendance/summary/{studentId}?month=&schoolYear=   [all roles]
- Acceptance criteria:
  - Teacher can only mark attendance for their own class_subjects
  - Bulk endpoint accepts list of student attendance records
  - Summary correctly counts informed vs uninformed absences

---

**T09 - API Controllers: Gradebook**
- Assigned to: Member C
- Phase: 2-API
- Depends on: T04
- Description: Create GradebookController for gradebook_entries. Teacher manages free-form entries (label, max_score, score) for their class_subjects. Entries are not the final monthly score — they are the teacher's working notes.
- Files to create:
  - `SchoolSystem.Core/DTOs/Gradebook/GradebookEntryDto.cs`, `CreateGradebookEntryDto.cs`, `UpdateGradebookEntryDto.cs`
  - `SchoolSystem.Core/Interfaces/IGradebookService.cs`
  - `SchoolSystem.Api/Services/GradebookService.cs` — injects AppDbContext
  - `SchoolSystem.Api/Controllers/GradebookController.cs`
- Files to modify:
  - `SchoolSystem.Api/Program.cs` — register IGradebookService as scoped
- Endpoints:
  - CRUD /api/gradebook                                                    [teacher]
  - GET  /api/gradebook/class-subject/{classSubjectId}                     [teacher, homeroom]
  - GET  /api/gradebook/student/{studentId}/class-subject/{classSubjectId} [teacher, homeroom]
- Acceptance criteria:
  - Teacher can only manage entries for their assigned class_subjects
  - CRUD works correctly for gradebook entries
  - Homeroom can view but not create/edit entries

---

**T10 - API Controllers: Monthly Scores and Reports**
- Assigned to: Member C
- Phase: 2-API
- Depends on: T09
- Description: Create MonthlyScoresController and MonthlyReportsController. Teacher submits one final_score per student per class_subject per month. Homeroom can edit scores (is_locked = false) before report submission. Report submission locks all scores and computes rank: sum final_score per student across all class_subjects in the class, rank by total within class.
- Files to create:
  - `SchoolSystem.Core/DTOs/Score/MonthlyScoreDto.cs`, `SubmitMonthlyScoreDto.cs`
  - `SchoolSystem.Core/DTOs/Report/MonthlyReportDto.cs`, `SubmitMonthlyReportDto.cs`
  - `SchoolSystem.Core/Interfaces/IMonthlyScoreService.cs`
  - `SchoolSystem.Core/Interfaces/IMonthlyReportService.cs`
  - `SchoolSystem.Api/Services/MonthlyScoreService.cs` — injects AppDbContext
  - `SchoolSystem.Api/Services/MonthlyReportService.cs` — injects AppDbContext, contains rank computation logic
  - `SchoolSystem.Api/Controllers/MonthlyScoresController.cs`
  - `SchoolSystem.Api/Controllers/MonthlyReportsController.cs`
- Files to modify:
  - `SchoolSystem.Api/Program.cs` — register both services as scoped
- Endpoints:
  - POST /api/monthly-scores/submit           [teacher]
  - PUT  /api/monthly-scores/{id}             [homeroom] — only if is_locked = false
  - POST /api/monthly-reports/submit          [homeroom] — locks scores, computes ranks
  - GET  /api/monthly-reports/{id}            [super_admin, homeroom, parent]
- Acceptance criteria:
  - Teacher submits one score per student per class_subject per month
  - Homeroom can edit before report submission
  - Report submission sets is_locked = true on all monthly_scores for that class+month+school_year
  - Rank computed as: sum of final_score across all subjects, ordered DESC, assigned rank 1..N

---

**T11 - API Controllers: Semester and Yearly Reports**
- Assigned to: Member C
- Phase: 2-API
- Depends on: T10
- Description: Create SemesterReportsController and YearlyReportsController. Semester aggregates 6 monthly scores per subject per student. Yearly aggregates 2 semester scores. Both compute rank within class using same logic as monthly.
- Files to create:
  - `SchoolSystem.Core/DTOs/Score/SemesterScoreDto.cs`
  - `SchoolSystem.Core/DTOs/Score/YearlyScoreDto.cs`
  - `SchoolSystem.Core/DTOs/Report/SemesterReportDto.cs`, `SubmitSemesterReportDto.cs`
  - `SchoolSystem.Core/DTOs/Report/YearlyReportDto.cs`, `SubmitYearlyReportDto.cs`
  - `SchoolSystem.Core/Interfaces/ISemesterReportService.cs`
  - `SchoolSystem.Core/Interfaces/IYearlyReportService.cs`
  - `SchoolSystem.Api/Services/SemesterReportService.cs` — injects AppDbContext
  - `SchoolSystem.Api/Services/YearlyReportService.cs` — injects AppDbContext
  - `SchoolSystem.Api/Controllers/SemesterReportsController.cs`
  - `SchoolSystem.Api/Controllers/YearlyReportsController.cs`
- Files to modify:
  - `SchoolSystem.Api/Program.cs` — register both services as scoped
- Endpoints:
  - POST /api/semester-reports/submit         [homeroom]
  - GET  /api/semester-reports/{id}           [super_admin, homeroom, parent]
  - POST /api/yearly-reports/submit           [homeroom]
  - GET  /api/yearly-reports/{id}             [super_admin, homeroom, parent]
- Acceptance criteria:
  - Semester score = average of 6 monthly final_scores per subject per student
  - Yearly score = average of 2 semester scores per subject per student
  - Rank computation identical to monthly: sum across subjects, rank within class
  - Reports are read-only after submission

---

**T12 - API Controllers: Feedback**
- Assigned to: Member C
- Phase: 2-API
- Depends on: T04
- Description: Create FeedbackController. Parent submits feedback linked to a specific report. The feedback table has nullable FKs for monthly_report_id, semester_report_id, yearly_report_id — only one should be set per record depending on report_type.
- Files to create:
  - `SchoolSystem.Core/DTOs/Feedback/FeedbackDto.cs`, `CreateFeedbackDto.cs`
  - `SchoolSystem.Core/Interfaces/IFeedbackService.cs`
  - `SchoolSystem.Api/Services/FeedbackService.cs` — injects AppDbContext
  - `SchoolSystem.Api/Controllers/FeedbackController.cs`
- Files to modify:
  - `SchoolSystem.Api/Program.cs` — register IFeedbackService as scoped
- Endpoints:
  - POST /api/feedback                                        [parent]
  - GET  /api/feedback/report/{reportType}/{reportId}        [super_admin, homeroom, parent]
  - GET  /api/feedback/class/{classId}                       [super_admin, homeroom]
- Acceptance criteria:
  - Parent can only submit feedback for their linked child's reports
  - report_type enum determines which FK column is populated
  - Homeroom sees all feedback for their class
  - super_admin sees all feedback

---

### Phase 3: Desktop

**T13 - Desktop: ApiClient Service and Authentication**
- Assigned to: Member B
- Phase: 3-Desktop
- Depends on: T04, T05
- Description: Create ApiClient singleton wrapping HttpClient. Store JWT token in memory after login. Inject Authorization header automatically on every request. Create frmLogin connecting to POST /api/auth/login. Program.cs launches frmMain after successful login.
- Files to create:
  - `SchoolSystem.Desktop/Services/ApiClient.cs` — singleton, stores JWT, wraps GET/POST/PUT/DELETE
  - `SchoolSystem.Desktop/Forms/Auth/frmLogin.cs` + `frmLogin.Designer.cs`
- Files to modify:
  - `SchoolSystem.Desktop/Program.cs` — launch frmLogin first, then frmMain on success
- Acceptance criteria:
  - ApiClient singleton with token management
  - frmLogin authenticates against /api/auth/login
  - All API calls include Authorization: Bearer {token} header
  - Logout clears stored token and returns to frmLogin
  - frmMain launched after successful login

---

**T14 - Desktop: Main Dashboard and Navigation**
- Assigned to: Member B
- Phase: 3-Desktop
- Depends on: T13
- Description: Create main form `frmMain` with MenuStrip navigation and dynamic TabControl. Menu items open tabs on demand. Permanent Dashboard tab shows quick stats. Role visibility controlled via MenuStrip show/hide, not tab visibility.
- Files to create:
  - `SchoolSystem.Desktop/Forms/frmMain.cs` + `frmMain.Designer.cs` — MenuStrip + dynamic TabControl shell
- Files to modify:
  - `SchoolSystem.Desktop/Program.cs` — open frmMain after successful login
- Menu structure:
  - `tsmiAdmin` → `tsmiUsers`, `tsmiClasses`, `tsmiSubjects`
  - `tsmiTeacher` → `tsmiGradebook`, `tsmiAttendance`, `tsmiScoreSubmit`
  - `tsmiHomeroom` → `tsmiClassOverview`, `tsmiReportSubmit`
  - `tsmiAccount` → `tsmiLogout`
- Tab structure:
  - `tpDashboard` — Permanent, shows quick stats (always present)
  - Other tabs created dynamically when menu item clicked, closed with X button
- Acceptance criteria:
  - frmMain with MenuStrip and dynamic TabControl (tcMain)
  - Role visibility via MenuStrip show/hide:
    - super_admin: tsmiAdmin + tsmiTeacher + tsmiHomeroom + tsmiAccount
    - teacher: tsmiTeacher + tsmiAccount
    - homeroom: tsmiTeacher + tsmiHomeroom + tsmiAccount
  - Dashboard shows stat labels: Users, Classes, Students, Subjects (visibility varies by role)
  - Dashboard loads basic stats on open

---

**T15 - Desktop: User and Academic Structure (UserControls)**
- Assigned to: Member B
- Phase: 3-Desktop
- Depends on: T14
- Description: Create UserControls for each feature area, loaded dynamically into tcMain TabControl when menu item clicked. Dialog forms still used for Add/Edit operations.
- Files to create:
  - `SchoolSystem.Desktop/Controls/Admin/ucUserManagement.cs` + `.Designer.cs` — User list, CRUD, search
  - `SchoolSystem.Desktop/Controls/Admin/ucClassManagement.cs` + `.Designer.cs` — Class list, CRUD, filters
  - `SchoolSystem.Desktop/Controls/Admin/ucSubjectManagement.cs` + `.Designer.cs` — Subject list, CRUD
  - `SchoolSystem.Desktop/Forms/Admin/frmUserDialog.cs` + `.Designer.cs` — User create/edit dialog
  - `SchoolSystem.Desktop/Forms/Admin/frmGradeDialog.cs` + `.Designer.cs` — Grade create/edit dialog
  - `SchoolSystem.Desktop/Forms/Admin/frmClassDialog.cs` + `.Designer.cs` — Class create/edit dialog
  - `SchoolSystem.Desktop/Forms/Admin/frmSubjectDialog.cs` + `.Designer.cs` — Subject create/edit dialog
- Files to modify:
  - `SchoolSystem.Desktop/Forms/frmMain.cs` — Wire menu items to load UserControls
- UserControl structure:
  - `ucUserManagement` — dgvUsers, btnAddUser, btnEditUser, btnDeleteUser, btnRefreshUsers, txtSearch
  - `ucClassManagement` — dgvClasses, cboGradeFilter, nudSchoolYear, btnAddClass, btnEditClass, btnDeleteClass
  - `ucSubjectManagement` — dgvSubjects, btnAddSubject, btnEditSubject, btnDeleteSubject
- Acceptance criteria:
  - UserControls loaded into tabs on menu click
  - DataGridView with Add/Edit/Delete buttons per UserControl
  - Dialog forms validate required fields before submit
  - Changes reflected in grid after API call succeeds
  - All controls follow naming conventions (btnAddUser, dgvUsers, etc.)

---

**T16 - Desktop: Student and Attendance (UserControls)**
- Assigned to: Member B
- Phase: 3-Desktop
- Depends on: T15
- Description: Create UserControls for class overview and attendance, loaded dynamically into tcMain TabControl when menu item clicked.
- Files to create:
  - `SchoolSystem.Desktop/Controls/Homeroom/ucClassOverview.cs` + `.Designer.cs` — Class roster and student overview
  - `SchoolSystem.Desktop/Controls/Teacher/ucAttendance.cs` + `.Designer.cs` — Attendance marking per ClassSubject + date
  - `SchoolSystem.Desktop/Forms/Admin/frmStudentDialog.cs` + `.Designer.cs` — Student create/edit dialog
- Files to modify:
  - `SchoolSystem.Desktop/Forms/frmMain.cs` — Wire menu items to load UserControls
- UserControl structure:
  - `ucClassOverview` — cboClass, dgvStudents, btnAddStudent, btnEditStudent, btnDeleteStudent, btnLinkParent
  - `ucAttendance` — cboClass, cboClassSubject, dtpDate, dgvAttendance, btnSubmitAttendance, btnBulkSubmit
- Acceptance criteria:
  - UserControls loaded into tabs on menu click
  - Student CRUD with class assignment dropdown
  - Attendance form filters by class and date
  - Bulk grid shows all students with status dropdown per row
  - Visual color indicators: green = present, yellow = informed, red = uninformed

---

**T17 - Desktop: Gradebook and Score (UserControls)**
- Assigned to: Member B
- Phase: 3-Desktop
- Depends on: T16
- Description: Create UserControls for gradebook entries and score submission, loaded dynamically into tcMain TabControl when menu item clicked.
- Files to create:
  - `SchoolSystem.Desktop/Controls/Teacher/ucGradebook.cs` + `.Designer.cs` — Gradebook entries per ClassSubject
  - `SchoolSystem.Desktop/Controls/Teacher/ucScoreSubmit.cs` + `.Designer.cs` — Monthly score submission
- Files to modify:
  - `SchoolSystem.Desktop/Forms/frmMain.cs` — Wire menu items to load UserControls
- UserControl structure:
  - `ucGradebook` — cboClassSubject, dgvGradebook, btnAddEntry, btnEditEntry, btnDeleteEntry
  - `ucScoreSubmit` — cboClassSubject, nudMonth, nudYear, dgvScores, btnSubmitScores
- Acceptance criteria:
  - UserControls loaded into tabs on menu click
  - Gradebook grid is editable for teacher's own class_subjects
  - Monthly score submission shows all students with score input
  - Locked scores are grayed out and non-editable with lock icon

---

**T18 - Desktop: Report Forms (UserControls)**
- Assigned to: Member B
- Phase: 3-Desktop
- Depends on: T17
- Description: Create UserControl for report submission, loaded dynamically into tcMain TabControl when menu item clicked.
- Files to create:
  - `SchoolSystem.Desktop/Controls/Homeroom/ucReportSubmit.cs` + `.Designer.cs` — Monthly/Semester/Yearly report submission
- Files to modify:
  - `SchoolSystem.Desktop/Forms/frmMain.cs` — Wire menu item to load UserControl
- UserControl structure:
  - `ucReportSubmit` — cboClass, nudMonth, nudYear, cboSemester, cboReportType, btnSubmitReport, dgvReportResult
- Acceptance criteria:
  - UserControl loaded into tab on menu click
  - Report submission calls POST /api/monthly-reports/submit (or semester/yearly)
  - Rank table displays after submission with student name, total score, rank
  - Submitted reports open as read-only in same tab

---

### Phase 4: Web

**T19 - Web: Authentication and Session Management**
- Assigned to: Member C
- Phase: 4-Web
- Depends on: T04, T05
- Description: Create Cookie-based login for the Razor Pages web portal. Do NOT use JWT here. The web portal authenticates against the DB directly using AppDbContext (or a shared AuthService from Core). Session persists via Cookie claims.
- Files to create:
  - `SchoolSystem.Web/Pages/Auth/Login.cshtml` + `Login.cshtml.cs`
  - `SchoolSystem.Web/Pages/Auth/Logout.cshtml` + `Logout.cshtml.cs`
  - `SchoolSystem.Web/Services/WebAuthService.cs` — validates credentials, issues cookie
- Files to modify:
  - `SchoolSystem.Web/Program.cs` — register Cookie auth, register WebAuthService, add AppDbContext connection
  - `SchoolSystem.Web/appsettings.json` — add ConnectionStrings:DefaultConnection
- Acceptance criteria:
  - Cookie-based login works with no JWT involved
  - Session persists across page requests
  - Logout clears authentication cookie
  - Non-authenticated users redirected to /Auth/Login

---

**T20 - Web: Parent Portal Dashboard**
- Assigned to: Member C
- Phase: 4-Web
- Depends on: T19
- Description: Create parent dashboard page. Query parent_student to find linked children. Display each child's name, class, and latest report summary.
- Files to create:
  - `SchoolSystem.Web/Pages/Dashboard/Index.cshtml` + `Index.cshtml.cs`
- Files to modify:
  - `SchoolSystem.Web/Pages/_Layout.cshtml` — add nav links for Reports, Feedback
- Acceptance criteria:
  - Dashboard shows all children linked to logged-in parent
  - Each child card shows name, class, school_year
  - Navigation links to reports and feedback per child

---

**T21 - Web: Student Reports View**
- Assigned to: Member C
- Phase: 4-Web
- Depends on: T20
- Description: Create Razor Pages for monthly, semester, and yearly report views. Display per-subject scores, total score, class rank, informed/uninformed absence counts. All read-only.
- Files to create:
  - `SchoolSystem.Web/Pages/Reports/Monthly.cshtml` + `Monthly.cshtml.cs`
  - `SchoolSystem.Web/Pages/Reports/Semester.cshtml` + `Semester.cshtml.cs`
  - `SchoolSystem.Web/Pages/Reports/Yearly.cshtml` + `Yearly.cshtml.cs`
- Acceptance criteria:
  - Parent can view their child's reports only
  - Scores displayed per subject with total and rank
  - Absence summary shown per report period
  - No edit controls on any report page

---

**T22 - Web: Feedback Submission**
- Assigned to: Member C
- Phase: 4-Web
- Depends on: T21
- Description: Create feedback submission form linked to a specific report. Store report_type and the relevant report FK. Display feedback history per report.
- Files to create:
  - `SchoolSystem.Web/Pages/Feedback/Submit.cshtml` + `Submit.cshtml.cs`
  - `SchoolSystem.Web/Pages/Feedback/History.cshtml` + `History.cshtml.cs`
- Acceptance criteria:
  - Feedback form pre-fills report_type and report_id from query string
  - Submitted feedback saved with correct FK (monthly_report_id, semester_report_id, or yearly_report_id)
  - History page shows all feedback submitted by the logged-in parent

---

## SUMMARY

- **Member A (Lead)**: T01–T04 — Foundation (Core models, DbContext, Auth)
- **Member B**: T13–T18 — Desktop (WinForms with TabControl architecture)
- **Member C**: T05–T12 (API), T19–T22 (Web portal)

### Desktop Architecture Note

Starting from T14, the Desktop project uses **MenuStrip + dynamic TabControl**:

- Single `frmMain` with MenuStrip navigation
- Dynamic TabControl (tcMain) — tabs opened on demand per menu click
- Permanent Dashboard tab (tpDashboard)
- Each feature area is a UserControl (uc prefix) loaded into a tab
- Dialog forms (frm prefix) used for Add/Edit operations only
- Role-based visibility via MenuStrip show/hide, not tab show/hide

Total: 22 tasks across 4 phases.
Members B and C can begin their phases as soon as T04 is complete.
All projects target .NET 10.
