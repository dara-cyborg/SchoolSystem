# SchoolSystem Solution Flow and File Roles

This document explains:
1. How data and requests flow through the system
2. What each key file is responsible for

It is meant as a practical onboarding map for new contributors.

## 1) End-to-End Flow

### 1. API request flow (Desktop/Web -> API -> DB)
1. Client sends HTTP request to `SchoolSystem.Api`.
2. `Program.cs` configures middleware/auth and routes request to the matching controller.
3. Controller validates input and delegates business logic to a service.
4. Service uses `AppDbContext` (EF Core) to query/update PostgreSQL.
5. Service returns DTOs/results to controller.
6. Controller returns JSON response to caller.

### 2. Authentication and authorization flow
1. User logs in via `POST /api/auth/login`.
2. `AuthController` + auth service validate credentials and issue JWT.
3. Client includes JWT in `Authorization: Bearer <token>`.
4. API auth middleware validates token and builds user claims.
5. Role checks and user-id checks protect endpoints.

### 3. Academic/reporting business flow
1. Setup chain: grades -> classes -> subjects -> class-subjects -> students.
2. Teachers submit attendance/gradebook/monthly scores.
3. Homeroom submits monthly reports (and lock behavior for scores).
4. Semester/yearly report endpoints aggregate monthly/semester data.
5. Parents read reports and submit feedback.

## 2) File Roles by Area

## Root
- `SchoolSystem.slnx`: Solution container that ties all projects together.
- `.gitignore`: Ignore rules for generated/build/local files.

## docs/
- `docs/API_REFERENCE.md`: Endpoint-level API contract and usage reference.
- `docs/flow.md`: Presentation and conceptual walkthrough of architecture and lifecycle.
- `docs/tasks.md`: Task plan, ownership, and implementation phases.
- `docs/UI_COMPONENTS.md`: UI components and desktop/web UI references.
- `docs/SOLUTION_FLOW_AND_FILE_ROLES.md`: This architecture + file-role map.

## IntegrationTests/
- `IntegrationTests/seed_schoolsystem_2025.sql`: Seed data script for integration/testing data.
- `IntegrationTests/structure.txt`: Snapshot/notes about DB or project structure for test context.
- `IntegrationTests/T05_T08_SchoolSystem_API.postman_collection.json`: Postman tests/requests for T05-T08 APIs.
- `IntegrationTests/T09_Gradebook_API.postman_collection.json`: Gradebook API test collection.
- `IntegrationTests/T10_MonthlyScoreReport_API.postman_collection.json`: Monthly score/report test collection.
- `IntegrationTests/T11_SemesterYearlyReport_API.postman_collection.json`: Semester/yearly report test collection.
- `IntegrationTests/T12_Feedback_API.postman_collection.json`: Feedback API test collection.
- `IntegrationTests/T09_REPORT.md`: Report/notes for T09.
- `IntegrationTests/T10_REPORT.md`: Report/notes for T10.
- `IntegrationTests/T11_REPORT.md`: Report/notes for T11.
- `IntegrationTests/T12_REPORT.md`: Report/notes for T12.
- `IntegrationTests/T13_REPORT.md`: Report/notes for T13.

## SchoolSystem.Core/

### Core project root
- `SchoolSystem.Core/SchoolSystem.Core.csproj`: Core project definition and package references.
- `SchoolSystem.Core/GlobalUsings.cs`: Shared/global using directives.

### DTOs
- `SchoolSystem.Core/DTOs/PagedResult.cs`: Generic paginated response model (`Items`, `TotalCount`, etc.).
- `SchoolSystem.Core/DTOs/Auth/`: Request/response contracts for authentication.
- `SchoolSystem.Core/DTOs/User/`: User management DTOs.
- `SchoolSystem.Core/DTOs/Grade/`: Grade DTOs.
- `SchoolSystem.Core/DTOs/Class/`: Class DTOs.
- `SchoolSystem.Core/DTOs/ClassSubject/`: Class-subject assignment DTOs.
- `SchoolSystem.Core/DTOs/Student/`: Student and parent-link DTOs.
- `SchoolSystem.Core/DTOs/Attendance/`: Attendance DTOs.
- `SchoolSystem.Core/DTOs/Gradebook/`: Gradebook DTOs.
- `SchoolSystem.Core/DTOs/Score/`: Monthly/semester/yearly score DTOs.
- `SchoolSystem.Core/DTOs/Report/`: Monthly/semester/yearly report DTOs.
- `SchoolSystem.Core/DTOs/Feedback/`: Feedback DTOs.
- `SchoolSystem.Core/DTOs/Parent/`: Parent-facing DTOs.
- `SchoolSystem.Core/DTOs/Subject/`: Subject DTOs.

### Domain and shared contracts
- `SchoolSystem.Core/Models/`: EF entity models representing domain tables.
- `SchoolSystem.Core/Interfaces/`: Service interfaces shared across layers.
- `SchoolSystem.Core/Enums/`: Enum definitions used in entities/DTOs/business rules.
- `SchoolSystem.Core/Extensions/`: Utility extensions (for example claims/helpers).
- `SchoolSystem.Core/Exceptions/`: Custom exception types used by services/controllers.

## SchoolSystem.Api/

### API project root
- `SchoolSystem.Api/SchoolSystem.Api.csproj`: API project definition.
- `SchoolSystem.Api/Program.cs`: App startup; DI, auth, middleware, routing.
- `SchoolSystem.Api/appsettings.json`: Base runtime settings (connection strings, JWT config, etc.).
- `SchoolSystem.Api/appsettings.Development.json`: Development overrides.
- `SchoolSystem.Api/SchoolSystem.Api.http`: Manual HTTP request snippets.

### HTTP layer
- `SchoolSystem.Api/Controllers/AuthController.cs`: Login/auth endpoints.
- `SchoolSystem.Api/Controllers/UsersController.cs`: User CRUD + role assignment endpoints.
- `SchoolSystem.Api/Controllers/GradesController.cs`: Grade endpoints.
- `SchoolSystem.Api/Controllers/ClassesController.cs`: Class endpoints.
- `SchoolSystem.Api/Controllers/SubjectsController.cs`: Subject endpoints.
- `SchoolSystem.Api/Controllers/ClassSubjectsController.cs`: Class-subject mapping endpoints.
- `SchoolSystem.Api/Controllers/StudentsController.cs`: Student and parent-link endpoints.
- `SchoolSystem.Api/Controllers/AttendanceController.cs`: Attendance endpoints.
- `SchoolSystem.Api/Controllers/GradebookController.cs`: Gradebook entry endpoints.
- `SchoolSystem.Api/Controllers/ScoresController.cs`: Generic/combined score endpoints.
- `SchoolSystem.Api/Controllers/MonthlyScoresController.cs`: Monthly score submission/update endpoints.
- `SchoolSystem.Api/Controllers/ReportsController.cs`: Shared/report utility endpoints.
- `SchoolSystem.Api/Controllers/MonthlyReportsController.cs`: Monthly report submission/read endpoints.
- `SchoolSystem.Api/Controllers/SemesterReportsController.cs`: Semester report endpoints.
- `SchoolSystem.Api/Controllers/YearlyReportsController.cs`: Yearly report endpoints.
- `SchoolSystem.Api/Controllers/FeedbackController.cs`: Parent feedback endpoints.

### Data, services, and cross-cutting
- `SchoolSystem.Api/Data/AppDbContext.cs`: EF Core DbContext and DB mappings.
- `SchoolSystem.Api/Services/`: Business logic implementations used by controllers.
- `SchoolSystem.Api/Migrations/`: EF migrations and model snapshot history.
- `SchoolSystem.Api/Handlers/RoleAuthorizationHandler.cs`: Role/authorization policy handler.
- `SchoolSystem.Api/Middleware/RoleAuthorizationMiddleware.cs`: Role authorization middleware pipeline behavior.
- `SchoolSystem.Api/Properties/`: Launch/debug metadata.

## SchoolSystem.Desktop/

### Desktop project root
- `SchoolSystem.Desktop/SchoolSystem.Desktop.csproj`: WinForms project definition.
- `SchoolSystem.Desktop/Program.cs`: Desktop app entry point and startup wiring.
- `SchoolSystem.Desktop/Services/`: HTTP/API client and desktop service helpers.

### Forms
- `SchoolSystem.Desktop/Forms/frmMain.cs`: Main shell form, menu, tab host, dashboard loading, tab context menu logic.
- `SchoolSystem.Desktop/Forms/frmMain.resx`: Designer resources for `frmMain`.
- `SchoolSystem.Desktop/Forms/Auth/`: Login/auth forms and auth UI logic.
- `SchoolSystem.Desktop/Forms/Admin/`: Admin-focused user/class/subject management screens.
- `SchoolSystem.Desktop/Forms/Teacher/`: Teacher workflows (gradebook, attendance, score entry).
- `SchoolSystem.Desktop/Forms/Homeroom/`: Homeroom workflows (overview/report-related screens).

## SchoolSystem.Web/
- `SchoolSystem.Web/SchoolSystem.Web.csproj`: Web project definition.
- `SchoolSystem.Web/Program.cs`: Razor Pages startup, DI, auth/session pipeline.
- `SchoolSystem.Web/appsettings.json`: Base web settings.
- `SchoolSystem.Web/appsettings.Development.json`: Development web settings.
- `SchoolSystem.Web/Pages/`: Razor Pages handlers and page models.
- `SchoolSystem.Web/wwwroot/`: Static assets (css/js/images/libs).
- `SchoolSystem.Web/Properties/`: Launch/debug metadata.

## 3) Fast "Where Do I Change X?" Guide
- Add/modify API endpoint: `SchoolSystem.Api/Controllers/*Controller.cs` + matching service in `SchoolSystem.Api/Services/` + DTO in `SchoolSystem.Core/DTOs/`.
- Add business rule: primarily `SchoolSystem.Api/Services/`.
- Change DB shape: `SchoolSystem.Core/Models/` and `SchoolSystem.Api/Data/AppDbContext.cs`, then migration.
- Change desktop behavior: `SchoolSystem.Desktop/Forms/` and `SchoolSystem.Desktop/Services/`.
- Change web page behavior: `SchoolSystem.Web/Pages/`.

## 4) Notes
- Ignore `bin/` and `obj/` folders during code review; they are build outputs.
- Keep contracts in Core (`DTOs`, `Interfaces`, `Models`) and avoid leaking UI logic into API services.
- Keep controllers thin; business logic belongs in services.
