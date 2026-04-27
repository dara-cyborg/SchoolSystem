# T10 Monthly Scores and Reports API - Implementation Report

## Files Created
- **SchoolSystem.Core/DTOs/Score/MonthlyScoreDto.cs** - Response DTO with Id, StudentId, ClassSubjectId, Month, SchoolYear, FinalScore, IsLocked
- **SchoolSystem.Core/DTOs/Score/SubmitMonthlyScoreDto.cs** - Request DTO for score submission
- **SchoolSystem.Core/DTOs/Report/MonthlyReportDto.cs** - Response DTO with Id, ClassId, Month, SchoolYear, CreatedAt, Entries
- **SchoolSystem.Core/DTOs/Report/MonthlyReportEntryDto.cs** - Nested entry DTO with StudentId, TotalScore, Rank
- **SchoolSystem.Core/DTOs/Report/SubmitMonthlyReportDto.cs** - Request DTO for report submission
- **SchoolSystem.Core/Interfaces/IMonthlyScoreService.cs** - Service interface
- **SchoolSystem.Core/Interfaces/IMonthlyReportService.cs** - Service interface
- **SchoolSystem.Api/Services/MonthlyScoreService.cs** - Service implementation with AppDbContext injection
- **SchoolSystem.Api/Services/MonthlyReportService.cs** - Service implementation with rank computation logic
- **SchoolSystem.Api/Controllers/MonthlyScoresController.cs** - API controller with 2 endpoints
- **SchoolSystem.Api/Controllers/MonthlyReportsController.cs** - API controller with 2 endpoints
- **SchoolSystem.Api/Program.cs** - Registered both services as scoped

## Endpoints Implemented

### Monthly Scores
1. **POST /api/monthly-scores/submit** [TeacherOnly]
   - Submits or updates a monthly score for a student
   - Upsert logic: updates existing score or creates new one
   - Returns 201 Created with location header
   
2. **PUT /api/monthly-scores/{id}** [HomeroomOnly]
   - Updates a monthly score (FinalScore only)
   - Returns 400 if score is locked
   - Returns 200 OK or 404 Not Found

### Monthly Reports
1. **POST /api/monthly-reports/submit** [HomeroomOnly]
   - Submits a monthly report for a class
   - Locks all monthly scores for that class+month+schoolyear
   - Computes and assigns ranks based on total scores per student
   - Verifies no duplicate report exists
   - Returns 201 Created with location header
   
2. **GET /api/monthly-reports/{id}** [SuperAdmin, Homeroom, Parent]
   - Retrieves a monthly report with entries
   - Parent access validated via parent_student table
   - Returns 200 OK or 404 Not Found or 403 Forbidden

## Key Design Decisions

### Score Submission
- **Teacher Authorization**: Teachers can only submit scores for their assigned ClassSubjects
- **Upsert Logic**: If (StudentId, ClassSubjectId, Month, SchoolYear) exists, update FinalScore; otherwise insert
- **Validation**: Both StudentId and ClassSubjectId must exist
- **Tracking**: SubmittedAt and SubmittedBy recorded on submit

### Homeroom Editing
- **Locked Check**: Homeroom cannot edit scores that have IsLocked = true
- **Field Restriction**: Only FinalScore can be updated; other fields immutable
- **Error Handling**: Returns 400 Bad Request if score is locked

### Report Submission & Ranking
- **Duplicate Check**: Prevents duplicate reports for same (ClassId, Month, SchoolYear)
- **Score Locking**: Sets IsLocked = true on all scores in the class for that month/year
- **Rank Computation**: 
  - Sums FinalScore per student across all ClassSubjects in the class
  - Sorts descending by total score
  - Assigns ranks 1..N sequentially
- **Atomicity**: All operations within a single transaction (SaveChangesAsync)

### Parent Access Control
- **Validation**: Parents can only view reports for classes containing their linked students
- **Lookup**: Verified via ParentStudent → Student → Class relationship
- **Authorization**: Returns 403 Forbidden if parent has no access

### Error Handling
- **InvalidOperationException**: Caught in controller → 400 Bad Request with message
- **FK Violations**: Detected before insert/update; clear error messages
- **Authorization Violations**: Caught and returned as 400 Bad Request

### Data Mapping
- Static MapToDto() in each service prevents repeated projections
- Entries ordered by Rank in report DTOs
- Timestamps (CreatedAt) included in response DTOs

## Transactions & Consistency
- Report submission is atomic: all scores locked and entries created in single transaction
- Score updates are immediate; locked state prevents further homeroom edits
- Parent validation performed each time (not cached) for security
