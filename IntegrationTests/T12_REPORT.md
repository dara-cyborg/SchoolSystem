# T12 Feedback API - Implementation Report

## Files Created
- **SchoolSystem.Core/DTOs/Feedback/FeedbackDto.cs** - Response DTO with Id, ParentUserId, ReportType, FK columns, Message, CreatedAt
- **SchoolSystem.Core/DTOs/Feedback/CreateFeedbackDto.cs** - Request DTO with ReportType, ReportId, Message
- **SchoolSystem.Core/Interfaces/IFeedbackService.cs** - Service interface
- **SchoolSystem.Api/Services/FeedbackService.cs** - Service implementation with AppDbContext injection
- **SchoolSystem.Api/Controllers/FeedbackController.cs** - API controller with 3 endpoints
- **SchoolSystem.Api/Program.cs** - Registered IFeedbackService as scoped

## Endpoints Implemented
1. **POST /api/feedback** [ParentOnly]
   - Submits feedback for a specific report (monthly, semester, or yearly)
   - Validates report exists in correct table based on ReportType
   - Validates parent ownership via parent_student relationship
   - Sets exactly one FK column; leaves others null
   - Returns 201 Created with location header
   
2. **GET /api/feedback/report/{reportType}/{reportId}** [SuperAdmin, Homeroom, Parent]
   - Retrieves all feedback for a specific report
   - Parent access validated via parent_student relationship
   - SuperAdmin and Homeroom can view all feedback for any report
   - Returns 200 OK with empty list if no feedback exists (never null)
   
3. **GET /api/feedback/class/{classId}** [SuperAdmin, Homeroom]
   - Retrieves all feedback across all report types for a class
   - Only accessible to SuperAdmin and Homeroom
   - Returns 200 OK with empty list if no feedback exists (never null)

## Key Design Decisions

### Feedback Creation
- **Report Validation**: Checks existence in correct table (monthly_reports, semester_reports, or yearly_reports) based on ReportType
- **Parent Authorization**: Validates parent has linked student(s) in the report's class via parent_student table
- **FK Management**: Sets exactly one of three FK columns based on ReportType; leaves others null
- **Error Handling**: Clear InvalidOperationException for missing reports or authorization failures

### Report-based Query
- **Type-Safe Lookup**: Queries correct table and FK column based on ReportType enum
- **Parent Access Control**: If requesting parent, validates they have students in that report's class
- **Empty Results**: Returns empty list (not null) if no feedback exists
- **Ordering**: Results ordered by Id

### Class-based Query
- **Aggregation**: Collects all monthly, semester, and yearly reports for the class
- **Cross-Type Lookup**: Queries feedback across all three FK columns
- **Admin Only**: SuperAdmin and Homeroom can view class feedback
- **Empty Results**: Returns empty list (not null) if no feedback exists

### Data Mapping
- Static MapToDto() prevents repeated projections
- Content field from model mapped to Message in DTO
- All FK columns preserved in DTO for transparency

### Error Handling
- **Report Not Found**: Returns 400 Bad Request with "Report not found." message
- **Unauthorized Parent**: Returns 400 Bad Request with authorization message
- **Empty Results**: Returns 200 OK with empty list (not 404)
- **Invalid ReportType**: Returns 400 Bad Request with enum guidance

### Authorization
- **ParentOnly**: Can only create feedback; can view feedback for their reports only
- **SuperAdmin/Homeroom**: Can view feedback by report (any class) or by class
- **Empty Access**: Parent accessing unauthorized report returns empty list (not 403)

## Transactions & Consistency
- Each feedback creation is atomic (single SaveChangesAsync)
- Read operations are consistent snapshots
- Parent validation performed each time (not cached) for security
