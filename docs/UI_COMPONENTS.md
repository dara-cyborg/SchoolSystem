# UI Components Requirement Document

This document specifies the UI components required for each task in the SchoolSystem project, with correct naming conventions and implementation details.

---

## Table of Contents

1. [Desktop (WinForms) Components](#desktop-winforms-components)
2. [Web (Razor Pages) Components](#web-razor-pages-components)
3. [Component Naming Reference](#component-naming-reference)

---

## Desktop (WinForms) Components

### Phase 3: Desktop (Tasks T13-T18)

> **Architecture Note**: This project uses **MenuStrip + dynamic TabControl**. Features are organized as UserControls (uc prefix) loaded on-demand into a dynamic TabControl (tcMain) when menu items are clicked. The Dashboard tab (tpDashboard) is permanent. Role visibility is controlled via MenuStrip show/hide, not tab visibility.

#### T13 - Desktop: ApiClient Service and Authentication

| Component | Type | File Path | Description |
|-----------|------|-----------|-------------|
| `ApiClient` | Service | `SchoolSystem.Desktop/Services/ApiClient.cs` | Singleton service wrapping HttpClient. Stores JWT token in memory, injects Authorization header on every request. |
| `frmLogin` | Form | `SchoolSystem.Desktop/Forms/Auth/frmLogin.cs` + `.Designer.cs` | Login form with username/password fields. Calls POST /api/auth/login on submit. |

**frmLogin Controls:**
| Control | Type | Naming | Purpose |
|---------|------|--------|---------|
| `txtUsername` | TextBox | `txtUsername` | Username input field |
| `txtPassword` | TextBox | `txtPassword` | Password input field (PasswordChar = *) |
| `btnLogin` | Button | `btnLogin` | Submit login credentials |
| `lblError` | Label | `lblError` | Display error messages |
| `pnlMain` | Panel | `pnlMain` | Container panel |

---

#### T14 - Desktop: Main Dashboard and Navigation (MenuStrip + Dynamic TabControl)

| Component | Type | File Path | Description |
|-----------|------|-----------|-------------|
| `frmMain` | Form (MenuStrip + dynamic TabControl) | `SchoolSystem.Desktop/Forms/frmMain.cs` + `.Designer.cs` | Main form with MenuStrip navigation and dynamic TabControl. Tabs opened on demand when menu item clicked. Permanent Dashboard tab. Role visibility via MenuStrip show/hide. |

**frmMain Controls:**
| Control | Type | Naming | Purpose |
|---------|------|--------|---------|
| `tcMain` | TabControl | `tcMain` | Main dynamic tab control (Dock=Fill, DrawMode=OwnerDrawFixed) |
| `tpDashboard` | TabPage | `tpDashboard` | Permanent dashboard tab (always present) |
| `pnlDashboard` | Panel | `pnlDashboard` | Inside tpDashboard, Dock=Fill |
| `msMain` | MenuStrip | `msMain` | Main menu strip |
| `tsmiAdmin` | ToolStripMenuItem | `tsmiAdmin` | Top-level: Administration |
| `tsmiUsers` | ToolStripMenuItem | `tsmiUsers` | Child of tsmiAdmin |
| `tsmiClasses` | ToolStripMenuItem | `tsmiClasses` | Child of tsmiAdmin |
| `tsmiSubjects` | ToolStripMenuItem | `tsmiSubjects` | Child of tsmiAdmin |
| `tsmiTeacher` | ToolStripMenuItem | `tsmiTeacher` | Top-level: Teacher |
| `tsmiGradebook` | ToolStripMenuItem | `tsmiGradebook` | Child of tsmiTeacher |
| `tsmiAttendance` | ToolStripMenuItem | `tsmiAttendance` | Child of tsmiTeacher |
| `tsmiScoreSubmit` | ToolStripMenuItem | `tsmiScoreSubmit` | Child of tsmiTeacher |
| `tsmiHomeroom` | ToolStripMenuItem | `tsmiHomeroom` | Top-level: Homeroom |
| `tsmiClassOverview` | ToolStripMenuItem | `tsmiClassOverview` | Child of tsmiHomeroom |
| `tsmiReportSubmit` | ToolStripMenuItem | `tsmiReportSubmit` | Child of tsmiHomeroom |
| `tsmiAccount` | ToolStripMenuItem | `tsmiAccount` | Top-level: Account |
| `tsmiLogout` | ToolStripMenuItem | `tsmiLogout` | Child of tsmiAccount |
| `ssStatus` | StatusStrip | `ssStatus` | Status strip |
| `tsslUserInfo` | ToolStripStatusLabel | `tsslUserInfo` | Display logged-in user info |
| `tsslStatus` | ToolStripStatusLabel | `tsslStatus` | Status message |

**Role visibility is via MenuStrip, not tabs:**
- `super_admin`: tsmiAdmin + tsmiTeacher + tsmiHomeroom + tsmiAccount visible
- `teacher`: tsmiTeacher + tsmiAccount visible
- `homeroom`: tsmiTeacher + tsmiHomeroom + tsmiAccount visible
- `parent`: (Not used in Desktop)

**Dashboard Tab (tpDashboard) Controls:**
| Control | Type | Naming | Purpose |
|---------|------|--------|---------|
| `lblStatUsersLabel` | Label | `lblStatUsersLabel` | "Total Users" label |
| `lblStatUsersValue` | Label | `lblStatUsersValue` | Total users count |
| `lblStatClassesLabel` | Label | `lblStatClassesLabel` | "Total Classes" label |
| `lblStatClassesValue` | Label | `lblStatClassesValue` | Total classes count |
| `lblStatStudentsLabel` | Label | `lblStatStudentsLabel` | "Total Students" label |
| `lblStatStudentsValue` | Label | `lblStatStudentsValue` | Total students count |
| `lblStatSubjectsLabel` | Label | `lblStatSubjectsLabel` | "Total Subjects" label |
| `lblStatSubjectsValue` | Label | `lblStatSubjectsValue` | Total subjects count |

**Stat visibility by role:**
- SuperAdmin: Users, Classes, Students, Subjects
- Homeroom: Classes, Students only
- Teacher: Dashboard tab hidden (no stats shown)

---

#### T15 - Desktop: User and Academic Structure (UserControls)

All T15 features are implemented as UserControls loaded dynamically into tcMain.

##### ucUserManagement (Admin)

| Component | Type | File Path | Description |
|-----------|------|-----------|-------------|
| `ucUserManagement` | UserControl | `SchoolSystem.Desktop/Controls/Admin/ucUserManagement.cs` + `.Designer.cs` | User list, CRUD, search |
| `frmUserDialog` | Form (Dialog) | `SchoolSystem.Desktop/Forms/Admin/frmUserDialog.cs` + `.Designer.cs` | User create/edit dialog |

**ucUserManagement Controls:**
| Control | Type | Naming | Purpose |
|---------|------|--------|---------|
| `dgvUsers` | DataGridView | `dgvUsers` | User list display |
| `bsUsers` | BindingSource | `bsUsers` | Data binding source |
| `btnAddUser` | Button | `btnAddUser` | Add new user |
| `btnEditUser` | Button | `btnEditUser` | Edit selected user |
| `btnDeleteUser` | Button | `btnDeleteUser` | Delete selected user |
| `btnRefreshUsers` | Button | `btnRefreshUsers` | Refresh user list |
| `txtSearch` | TextBox | `txtSearch` | Search filter |
| `pnlToolbar` | Panel | `pnlToolbar` | Button toolbar panel |

**frmUserDialog Controls:**
| Control | Type | Naming | Purpose |
|---------|------|--------|---------|
| `txtName` | TextBox | `txtName` | User name input |
| `cboSex` | ComboBox | `cboSex` | Sex selection (male/female) |
| `dtpDob` | DateTimePicker | `dtpDob` | Date of birth |
| `txtContact` | TextBox | `txtContact` | Contact info |
| `txtPassword` | TextBox | `txtPassword` | Password (visible only on create) |
| `clbRoles` | CheckedListBox | `clbRoles` | Role selection |
| `chkIsActive` | CheckBox | `chkIsActive` | Active status |
| `btnSave` | Button | `btnSave` | Save user |
| `btnCancel` | Button | `btnCancel` | Cancel and close |

##### ucClassManagement (Admin)

| Component | Type | File Path | Description |
|-----------|------|-----------|-------------|
| `ucClassManagement` | UserControl | `SchoolSystem.Desktop/Controls/Admin/ucClassManagement.cs` + `.Designer.cs` | Class list, CRUD, filters |
| `frmClassDialog` | Form (Dialog) | `SchoolSystem.Desktop/Forms/Admin/frmClassDialog.cs` + `.Designer.cs` | Class create/edit dialog |

**ucClassManagement Controls:**
| Control | Type | Naming | Purpose |
|---------|------|--------|---------|
| `dgvClasses` | DataGridView | `dgvClasses` | Class list |
| `cboGradeFilter` | ComboBox | `cboGradeFilter` | Filter by grade |
| `nudSchoolYear` | NumericUpDown | `nudSchoolYear` | Filter by school year |
| `btnAddClass` | Button | `btnAddClass` | Add class |
| `btnEditClass` | Button | `btnEditClass` | Edit class |
| `btnDeleteClass` | Button | `btnDeleteClass` | Delete class |
| `btnViewClassStudents` | Button | `btnViewClassStudents` | View class roster |

**frmClassDialog Controls:**
| Control | Type | Naming | Purpose |
|---------|------|--------|---------|
| `txtName` | TextBox | `txtName` | Class name input |
| `cboGrade` | ComboBox | `cboGrade` | Grade selection |
| `nudSchoolYear` | NumericUpDown | `nudSchoolYear` | School year |
| `cboHomeroom` | ComboBox | `cboHomeroom` | Homeroom teacher selection |
| `btnSave` | Button | `btnSave` | Save class |
| `btnCancel` | Button | `btnCancel` | Cancel and close |

##### ucSubjectManagement (Admin)

| Component | Type | File Path | Description |
|-----------|------|-----------|-------------|
| `ucSubjectManagement` | UserControl | `SchoolSystem.Desktop/Controls/Admin/ucSubjectManagement.cs` + `.Designer.cs` | Subject list, CRUD |
| `frmSubjectDialog` | Form (Dialog) | `SchoolSystem.Desktop/Forms/Admin/frmSubjectDialog.cs` + `.Designer.cs` | Subject create/edit dialog |

**ucSubjectManagement Controls:**
| Control | Type | Naming | Purpose |
|---------|------|--------|---------|
| `dgvSubjects` | DataGridView | `dgvSubjects` | Subject list |
| `btnAddSubject` | Button | `btnAddSubject` | Add subject |
| `btnEditSubject` | Button | `btnEditSubject` | Edit subject |
| `btnDeleteSubject` | Button | `btnDeleteSubject` | Delete subject |

**frmSubjectDialog Controls:**
| Control | Type | Naming | Purpose |
|---------|------|--------|---------|
| `txtName` | TextBox | `txtName` | Subject name input |
| `btnSave` | Button | `btnSave` | Save subject |
| `btnCancel` | Button | `btnCancel` | Cancel and close |

##### Grade Dialog (shared)

| Component | Type | File Path | Description |
|-----------|------|-----------|-------------|
| `frmGradeDialog` | Form (Dialog) | `SchoolSystem.Desktop/Forms/Admin/frmGradeDialog.cs` + `.Designer.cs` | Grade create/edit dialog |

**frmGradeDialog Controls:**
| Control | Type | Naming | Purpose |
|---------|------|--------|---------|
| `txtName` | TextBox | `txtName` | Grade name input |
| `btnSave` | Button | `btnSave` | Save grade |
| `btnCancel` | Button | `btnCancel` | Cancel and close |

---

#### T16 - Desktop: Student and Attendance (UserControls)

All T16 features are implemented as UserControls loaded dynamically into tcMain.

##### ucClassOverview (Homeroom)

| Component | Type | File Path | Description |
|-----------|------|-----------|-------------|
| `ucClassOverview` | UserControl | `SchoolSystem.Desktop/Controls/Homeroom/ucClassOverview.cs` + `.Designer.cs` | Class roster and student overview |
| `frmStudentDialog` | Form (Dialog) | `SchoolSystem.Desktop/Forms/Admin/frmStudentDialog.cs` + `.Designer.cs` | Student create/edit dialog |

**ucClassOverview Controls:**
| Control | Type | Naming | Purpose |
|---------|------|--------|---------|
| `cboClass` | ComboBox | `cboClass` | Select class |
| `dgvStudents` | DataGridView | `dgvStudents` | Student list |
| `btnAddStudent` | Button | `btnAddStudent` | Add student |
| `btnEditStudent` | Button | `btnEditStudent` | Edit student |
| `btnDeleteStudent` | Button | `btnDeleteStudent` | Delete student |
| `btnLinkParent` | Button | `btnLinkParent` | Link parent to student |

**frmStudentDialog Controls:**
| Control | Type | Naming | Purpose |
|---------|------|--------|---------|
| `txtName` | TextBox | `txtName` | Student name |
| `cboSex` | ComboBox | `cboSex` | Sex selection |
| `dtpDob` | DateTimePicker | `dtpDob` | Date of birth |
| `cboClass` | ComboBox | `cboClass` | Class assignment |
| `btnSave` | Button | `btnSave` | Save student |
| `btnCancel` | Button | `btnCancel` | Cancel |

##### ucAttendance (Teacher)

| Component | Type | File Path | Description |
|-----------|------|-----------|-------------|
| `ucAttendance` | UserControl | `SchoolSystem.Desktop/Controls/Teacher/ucAttendance.cs` + `.Designer.cs` | Attendance marking per ClassSubject + date |

**ucAttendance Controls:**
| Control | Type | Naming | Purpose |
|---------|------|--------|---------|
| `cboClass` | ComboBox | `cboClass` | Select class |
| `cboClassSubject` | ComboBox | `cboClassSubject` | Select class-subject |
| `dtpDate` | DateTimePicker | `dtpDate` | Attendance date |
| `dgvAttendance` | DataGridView | `dgvAttendance` | Student attendance grid |
| `btnSubmitAttendance` | Button | `btnSubmitAttendance` | Submit attendance |
| `btnBulkSubmit` | Button | `btnBulkSubmit` | Bulk submit all |

**Attendance Status Indicators:**
| Status | Color |
|--------|-------|
| `present` | Green (#90EE90) |
| `informed_absence` | Yellow (#FFFFE0) |
| `uninformed_absence` | Red (#FFB6C1) |

---

#### T17 - Desktop: Gradebook and Score (UserControls)

All T17 features are implemented as UserControls loaded dynamically into tcMain.

##### ucGradebook (Teacher)

| Component | Type | File Path | Description |
|-----------|------|-----------|-------------|
| `ucGradebook` | UserControl | `SchoolSystem.Desktop/Controls/Teacher/ucGradebook.cs` + `.Designer.cs` | Gradebook entries per ClassSubject |

**ucGradebook Controls:**
| Control | Type | Naming | Purpose |
|---------|------|--------|---------|
| `cboClassSubject` | ComboBox | `cboClassSubject` | Select class-subject |
| `dgvGradebook` | DataGridView | `dgvGradebook` | Editable gradebook entries |
| `txtEntryLabel` | TextBox | `txtEntryLabel` | Entry label (quiz, test, etc.) |
| `nudEntryScore` | NumericUpDown | `nudEntryScore` | Score value |
| `nudEntryMaxScore` | NumericUpDown | `nudEntryMaxScore` | Maximum score |
| `btnAddEntry` | Button | `btnAddEntry` | Add entry |
| `btnEditEntry` | Button | `btnEditEntry` | Edit entry |
| `btnDeleteEntry` | Button | `btnDeleteEntry` | Delete entry |

##### ucScoreSubmit (Teacher)

| Component | Type | File Path | Description |
|-----------|------|-----------|-------------|
| `ucScoreSubmit` | UserControl | `SchoolSystem.Desktop/Controls/Teacher/ucScoreSubmit.cs` + `.Designer.cs` | Monthly score submission |

**ucScoreSubmit Controls:**
| Control | Type | Naming | Purpose |
|---------|------|--------|---------|
| `cboClassSubject` | ComboBox | `cboClassSubject` | Select class-subject |
| `nudMonth` | NumericUpDown | `nudMonth` | Month (1-12) |
| `nudYear` | NumericUpDown | `nudYear` | School year |
| `dgvScores` | DataGridView | `dgvScores` | Student scores grid |
| `btnSubmitScores` | Button | `btnSubmitScores` | Submit all scores |
| `pgbScoreProgress` | ProgressBar | `pgbScoreProgress` | Progress indicator |

**Locked Score Visual:**
- Background color: Gray (#D3D3D3)
- Non-editable
- Lock icon displayed in row

---

#### T18 - Desktop: Report Forms (UserControls)

All T18 features are implemented as UserControls loaded dynamically into tcMain.

##### ucReportSubmit (Homeroom)

| Component | Type | File Path | Description |
|-----------|------|-----------|-------------|
| `ucReportSubmit` | UserControl | `SchoolSystem.Desktop/Controls/Homeroom/ucReportSubmit.cs` + `.Designer.cs` | Monthly/Semester/Yearly report submission |

**ucReportSubmit Controls:**
| Control | Type | Naming | Purpose |
|---------|------|--------|---------|
| `cboClass` | ComboBox | `cboClass` | Select class |
| `nudMonth` | NumericUpDown | `nudMonth` | Month (1-12) for monthly report |
| `nudYear` | NumericUpDown | `nudYear` | School year |
| `cboSemester` | ComboBox | `cboSemester` | Semester (1-2) for semester report |
| `cboReportType` | ComboBox | `cboReportType` | Report type (Monthly/Semester/Yearly) |
| `btnSubmitReport` | Button | `btnSubmitReport` | Submit report |
| `dgvReportResult` | DataGridView | `dgvReportResult` | Rank table result |

**Rank Table Columns:**
| Column | Purpose |
|--------|---------|
| `StudentName` | Student name |
| `TotalScore` | Sum of scores across subjects |
| `Rank` | Class rank (1, 2, 3, ...) |

---

## Web (Razor Pages) Components

### Phase 4: Web (Tasks T19-T22)

#### T19 - Web: Authentication and Session Management

| Component | Type | File Path | Description |
|-----------|------|-----------|-------------|
| `Login.cshtml` | Razor Page | `SchoolSystem.Web/Pages/Auth/Login.cshtml` | Login form (Cookie-based, not JWT) |
| `Login.cshtml.cs` | Page Model | `SchoolSystem.Web/Pages/Auth/Login.cshtml.cs` | Login logic |
| `Logout.cshtml` | Razor Page | `SchoolSystem.Web/Pages/Auth/Logout.cshtml` | Logout action |
| `Logout.cshtml.cs` | Page Model | `SchoolSystem.Web/Pages/Auth/Logout.cshtml.cs` | Logout logic |
| `WebAuthService` | Service | `SchoolSystem.Web/Services/WebAuthService.cs` | Validates credentials, issues cookie |

**Login.cshtml Controls:**
| Control | Tag | Purpose |
|--------|-----|---------|
| `<input type="text" name="Username">` | Input | Username field |
| `<input type="password" name="Password">` | Input | Password field |
| `<button type="submit">` | Button | Submit login |
| `<div class="error">` | Div | Error message display |

---

#### T20 - Web: Parent Portal Dashboard

| Component | Type | File Path | Description |
|-----------|------|-----------|-------------|
| `Index.cshtml` | Razor Page | `SchoolSystem.Web/Pages/Dashboard/Index.cshtml` | Parent dashboard |
| `Index.cshtml.cs` | Page Model | `SchoolSystem.Web/Pages/Dashboard/Index.cshtml.cs` | Dashboard logic |

**Index.cshtml Structure:**
| Section | Purpose |
|---------|---------|
| Child cards grid | Display linked children |
| Quick links | Navigation to reports, feedback |
| Latest report summary | Most recent report per child |

---

#### T21 - Web: Student Reports View

| Component | Type | File Path | Description |
|-----------|------|-----------|-------------|
| `Monthly.cshtml` | Razor Page | `SchoolSystem.Web/Pages/Reports/Monthly.cshtml` | Monthly report view |
| `Monthly.cshtml.cs` | Page Model | `SchoolSystem.Web/Pages/Reports/Monthly.cshtml.cs` | Monthly report logic |
| `Semester.cshtml` | Razor Page | `SchoolSystem.Web/Pages/Reports/Semester.cshtml` | Semester report view |
| `Semester.cshtml.cs` | Page Model | `SchoolSystem.Web/Pages/Reports/Semester.cshtml.cs` | Semester report logic |
| `Yearly.cshtml` | Razor Page | `SchoolSystem.Web/Pages/Reports/Yearly.cshtml` | Yearly report view |
| `Yearly.cshtml.cs` | Page Model | `SchoolSystem.Web/Pages/Reports/Yearly.cshtml.cs` | Yearly report logic |

**Report Page Structure:**
| Section | Purpose |
|---------|---------|
| Student info header | Name, class, school year |
| Subject scores table | Per-subject scores |
| Total score | Sum across subjects |
| Class rank | Rank within class |
| Absence summary | Informed/uninformed counts |
| Navigation | Back to dashboard |

**Read-only Indicators:**
- No edit controls
- No submit buttons
- Gray background for scores

---

#### T22 - Web: Feedback Submission

| Component | Type | File Path | Description |
|-----------|------|-----------|-------------|
| `Submit.cshtml` | Razor Page | `SchoolSystem.Web/Pages/Feedback/Submit.cshtml` | Feedback submission form |
| `Submit.cshtml.cs` | Page Model | `SchoolSystem.Web/Pages/Feedback/Submit.cshtml.cs` | Submit logic |
| `History.cshtml` | Razor Page | `SchoolSystem.Web/Pages/Feedback/History.cshtml` | Feedback history |
| `History.cshtml.cs` | Page Model | `SchoolSystem.Web/Pages/Feedback/History.cshtml.cs` | History logic |

**Submit.cshtml Controls:**
| Control | Tag | Purpose |
|--------|-----|---------|
| `<select name="ReportType">` | Select | Report type (monthly/semester/yearly) |
| `<input type="hidden" name="ReportId">` | Input | Report ID (from query string) |
| `<textarea name="Message">` | Textarea | Feedback message |
| `<button type="submit">` | Button | Submit feedback |

**History.cshtml Structure:**
| Section | Purpose |
|---------|---------|
| Feedback list | All submitted feedback |
| Report link | Link to related report |
| Timestamp | Submission date |
| Status | Read/unread (optional) |

---

## Component Naming Reference

### WinForms Control Prefixes

| Component Type | Prefix | Example |
|----------------|--------|---------|
| Button | `btn` | `btnSubmit`, `btnCancel`, `btnRefresh` |
| TextBox | `txt` | `txtUsername`, `txtEmail`, `txtSearch` |
| Label | `lbl` | `lblName`, `lblError`, `lblStatus` |
| CheckBox | `chk` | `chkIsActive`, `chkRemember` |
| Panel | `pnl` | `pnlHeader`, `pnlContent`, `pnlFooter` |
| DataGridView | `dgv` | `dgvUsers`, `dgvStudents`, `dgvAttendance` |
| ComboBox | `cbo` | `cboGrade`, `cboClass`, `cboTeacher` |
| TabControl | `tc` | `tcMain`, `tcAcademic` |
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
| Form | `frm` | `frmLogin`, `frmUserDialog` |
| BindingSource | `bs` | `bsUsers`, `bsStudents` |
| UserControl | `uc` | `ucUserManagement`, `ucClassOverview` |

### Naming Rules

1. **Always use camelCase** after the prefix (first letter lowercase after prefix)
2. **Use descriptive names** that indicate the control's purpose
3. **Do NOT use underscores** or numbers at the start of the name
4. **For grid columns**, use PascalCase without prefix (e.g., `StudentName`, `AttendanceStatus`)
5. **Event handlers** should follow the pattern: `ControlName_EventName` (e.g., `btnSubmit_Click`, `txtSearch_TextChanged`)

### Correct vs Incorrect Naming

```csharp
// Bad examples:
btn_submit, Button1, submitButton, SUBMIT_BTN
textbox_username, UserNameTextBox, 1txtName
DGV_Students, dataGrid_Users

// Good examples:
btnSubmit, btnCancel, btnRefresh
txtUsername, txtEmail, txtSearch
dgvStudents, dgvUsers, dgvAttendance
```

---

## Task to Component Mapping

| Task | Components |
|------|-------------|
| T13 | `ApiClient`, `frmLogin` |
| T14 | `frmMain` (MenuStrip + dynamic tcMain with tpDashboard) |
| T15 | `ucUserManagement`, `ucClassManagement`, `ucSubjectManagement`, dialog forms |
| T16 | `ucClassOverview`, `ucAttendance`, `frmStudentDialog` (dialog) |
| T17 | `ucGradebook`, `ucScoreSubmit` |
| T18 | `ucReportSubmit` |
| T19 | `Login`, `Logout`, `WebAuthService` |
| T20 | `Dashboard/Index` |
| T21 | `Reports/Monthly`, `Reports/Semester`, `Reports/Yearly` |
| T22 | `Feedback/Submit`, `Feedback/History` |

---

## TabControl Architecture Summary

### Main Form Structure (frmMain)

```
┌─────────────────────────────────────────────────────────┐
│  MenuStrip: msMain                                       │
│  [Admin ▼] [Teacher ▼] [Homeroom ▼] [Account ▼]        │
├─────────────────────────────────────────────────────────┤
│  TabControl: tcMain (dynamic, Dock=Fill)               │
│  ┌─────────────────────────────────────────────────┐   │
│  │ tpDashboard (permanent) │ Other tabs (dynamic) │   │
│  │ - Stat labels           │ - UserControls loaded │   │
│  │                         │   on menu click        │   │
│  └─────────────────────────────────────────────────┘   │
├─────────────────────────────────────────────────────────┤
│  StatusStrip: ssStatus                                  │
│  [User: admin] [Role: super_admin]                     │
└─────────────────────────────────────────────────────────┘
```

### Menu Structure and Tab Loading

| Menu Item | Loads UserControl | Tab Created |
|-----------|-------------------|-------------|
| `tsmiUsers` | `ucUserManagement` | Users |
| `tsmiClasses` | `ucClassManagement` | Classes |
| `tsmiSubjects` | `ucSubjectManagement` | Subjects |
| `tsmiGradebook` | `ucGradebook` | Gradebook |
| `tsmiAttendance` | `ucAttendance` | Attendance |
| `tsmiScoreSubmit` | `ucScoreSubmit` | Score Submit |
| `tsmiClassOverview` | `ucClassOverview` | Class Overview |
| `tsmiReportSubmit` | `ucReportSubmit` | Report Submit |

### Role-Based Menu Visibility

| Role | Visible Menu Items |
|------|--------------------|
| `super_admin` | tsmiAdmin + tsmiTeacher + tsmiHomeroom + tsmiAccount |
| `teacher` | tsmiTeacher + tsmiAccount |
| `homeroom` | tsmiTeacher + tsmiHomeroom + tsmiAccount |

### Dashboard Stat Visibility

| Role | Visible Stats |
|------|---------------|
| `super_admin` | Users, Classes, Students, Subjects |
| `homeroom` | Classes, Students |
| `teacher` | (Dashboard tab hidden) |

---

## Color Scheme Reference

| Purpose | Color | Hex Code |
|---------|-------|----------|
| Present status | Light Green | `#90EE90` |
| Informed absence | Light Yellow | `#FFFFE0` |
| Uninformed absence | Light Pink | `#FFB6C1` |
| Locked score | Light Gray | `#D3D3D3` |
| Primary button | Blue | `#007ACC` |
| Error message | Red | `#DC143C` |
| Success message | Green | `#28A745` |

---

*Document generated for SchoolSystem project. Last updated: April 27, 2026.*