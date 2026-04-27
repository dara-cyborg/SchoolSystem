# T11 Semester and Yearly Reports API - Implementation Report

## Files Created
- **SchoolSystem.Core/DTOs/Score/SemesterScoreDto.cs** - Response DTO with Id, StudentId, ClassSubjectId, Semester, SchoolYear, AverageScore
- **SchoolSystem.Core/DTOs/Score/YearlyScoreDto.cs** - Response DTO with Id, StudentId, ClassSubjectId, SchoolYear, AverageScore
- **SchoolSystem.Core/DTOs/Report/SemesterReportDto.cs** - Response DTO with Id, ClassId, Semester, SchoolYear, CreatedAt, Entries
- **SchoolSystem.Core/DTOs/Report/SemesterReportEntryDto.cs** - Nested entry DTO with StudentId, TotalScore, Rank
- **SchoolSystem.Core/DTOs/Report/SubmitSemesterReportDto.cs** - Request DTO for report submission
- **SchoolSystem.Core/DTOs/Report/YearlyReportDto.cs** - Response DTO with Id, ClassId, SchoolYear, CreatedAt, Entries
- **SchoolSystem.Core/DTOs/Report/YearlyReportEntryDto.cs** - Nested entry DTO with StudentId, TotalScore, Rank
- **SchoolSystem.Core/DTOs/Report/SubmitYearlyReportDto.cs** - Request DTO for report submission
- **SchoolSystem.Core/Interfaces/ISemesterReportService.cs** - Service interface
- **SchoolSystem.Core/Interfaces/IYearlyReportService.cs** - Service interface
- **SchoolSystem.Api/Services/SemesterReportService.cs** - Service implementation with aggregation and rank computation
- **SchoolSystem.Api/Services/YearlyReportService.cs** - Service implementation with aggregation and rank computation
- **SchoolSystem.Api/Controllers/SemesterReportsController.cs** - API controller with 2 endpoints
- **SchoolSystem.Api/Controllers/YearlyReportsController.cs** - API controller with 2 endpoints
- **SchoolSystem.Api/Program.cs** - Registered both services as scoped

## Endpoints Implemented

### Semester Reports
1. **POST /api/semester-reports/submit** [HomeroomOnly]
   - Submits a semester report for a class
   - Aggregates locked monthly scores by semester (Months 1-6 for Semester 1, Months 7-12 for Semester 2)
   - Computes and assigns ranks based on total averaged scores per student
   - Verifies all 6 months of scores exist and are locked
   - Returns 201 Created with location header
   
2. **GET /api/semester-reports/{id}** [SuperAdmin, Homeroom, Parent]
   - Retrieves a semester report with entries
   - Parent access validated via parent_student table
   - Returns 200 OK or 404 Not Found or 403 Forbidden

### Yearly Reports
1. **POST /api/yearly-reports/submit** [HomeroomOnly]
   - Submits a yearly report for a class
   - Aggregates semester scores (Semester 1 and 2) into yearly scores
   - Computes and assigns ranks based on total averaged scores per student
   - Verifies both semesters' scores exist
   - Returns 201 Created with location header
   
2. **GET /api/yearly-reports/{id}** [SuperAdmin, Homeroom, Parent]
   - Retrieves a yearly report with entries
   - Parent access validated via parent_student table
   - Returns 200 OK or 404 Not Found or 403 Forbidden

## Key Design Decisions

### Semester Report Aggregation
- **Month Mapping**: Semester 1 = Months 1-6, Semester 2 = Months 7-12
- **Score Requirements**: All 6 months of monthly_scores must exist and be IsLocked = true
- **Averaging**: Calculates average FinalScore per student per ClassSubject across the 6 months
- **Rank Computation**: Sums averaged scores across all ClassSubjects, ranks DESC 1..N

### Yearly Report Aggregation
- **Score Requirements**: Both Semester 1 and Semester 2 scores must exist for each student
- **Averaging**: Averages the 2 semester scores per student per ClassSubject
- **Rank Computation**: Sums averaged scores across all ClassSubjects, ranks DESC 1..N

### Validation & Error Handling
- **Duplicate Prevention**: Returns error if report already exists for (ClassId, Semester/Year, SchoolYear)
- **Completeness Check**: Throws InvalidOperationException if required monthly/semester scores are missing
- **Lock Verification**: Monthly scores must be locked before semester aggregation
- **Clear Messaging**: Specific error messages for each validation failure

### Parent Access Control
- **Validation**: Parents can only view reports for classes containing their linked students
- **Lookup**: Verified via ParentStudent → Student → Class relationship
- **Authorization**: Returns 403 Forbidden if parent has no access
- **Per-Request**: Access validation performed each time for security

### Rank Computation (Identical to T10)
- Groups students by ID
- Sums averaged scores across all ClassSubjects
- Orders DESC by total score
- Assigns sequential ranks 1..N
- Entries ordered by rank in DTO response

### Transaction & Consistency
- Semester report: Semester scores created, then report and entries in separate SaveChangesAsync calls
- Yearly report: Yearly scores created, then report and entries in separate SaveChangesAsync calls
- Each aggregation validates data completeness before proceeding

### Error Handling
- **InvalidOperationException**: Caught in controller → 400 Bad Request with message
- **Not Found**: Service returns null → Controller returns 404
- **Forbidden**: Parent access check fails → 403 Forbidden response
- **Server Error**: Catch-all exception handler returns 500 with diagnostic message

### Data Mapping
- Static MapToDto() in each service prevents repeated projections
- Entries ordered by Rank in report DTOs
- Timestamps (CreatedAt) included in response DTOs
- Nullable decimal fields for average scores
