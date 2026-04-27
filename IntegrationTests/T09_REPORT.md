# T09 Gradebook API - Implementation Report

## Files Created
- **SchoolSystem.Core/DTOs/Gradebook/GradebookEntryDto.cs** - Response DTO with Id, StudentId, ClassSubjectId, Label, Score, MaxScore, CreatedAt
- **SchoolSystem.Core/DTOs/Gradebook/CreateGradebookEntryDto.cs** - Request DTO for creation
- **SchoolSystem.Core/DTOs/Gradebook/UpdateGradebookEntryDto.cs** - Request DTO for updates (Label, Score, MaxScore only)
- **SchoolSystem.Core/Interfaces/IGradebookService.cs** - Service interface
- **SchoolSystem.Api/Services/GradebookService.cs** - Service implementation with AppDbContext injection
- **SchoolSystem.Api/Controllers/GradebookController.cs** - API controller with 6 endpoints
- **SchoolSystem.Api/Program.cs** - Registered IGradebookService as scoped

## Endpoints Implemented
1. **POST /api/gradebook** [TeacherOnly]
   - Creates a new gradebook entry
   - Returns 201 Created with location header
   
2. **GET /api/gradebook/{id}** [TeacherOnly, Homeroom]
   - Retrieves a single gradebook entry by ID
   - Returns 200 OK or 404 Not Found
   
3. **PUT /api/gradebook/{id}** [TeacherOnly]
   - Updates Label, Score, MaxScore only
   - Returns 200 OK or 404 Not Found
   
4. **DELETE /api/gradebook/{id}** [TeacherOnly]
   - Deletes a gradebook entry
   - Returns 204 No Content or 404 Not Found
   
5. **GET /api/gradebook/class-subject/{classSubjectId}** [TeacherOnly, Homeroom]
   - Retrieves all entries for a class-subject (ordered by Id)
   - Returns 200 OK or 404 if ClassSubject doesn't exist
   
6. **GET /api/gradebook/student/{studentId}/class-subject/{classSubjectId}** [TeacherOnly, Homeroom]
   - Retrieves all entries for a specific student in a class-subject
   - Returns 200 OK or 404 if ClassSubject doesn't exist

## Key Design Decisions
- **Authorization**: Teachers can only modify entries for their assigned ClassSubjects (verified via TeacherUserId)
- **Homeroom Access**: Homeroom staff can read all entries but cannot create/update/delete
- **FK Validation**: Both StudentId and ClassSubjectId are validated before insert; ClassSubject ownership verified for modifications
- **Updates**: UpdateAsync returns the tracked entity without a second query
- **Deletions**: DeleteAsync returns bool (false if not found)
- **Queries**: GetByClassSubject and GetByStudentAndClassSubject return null if ClassSubject doesn't exist
- **Ordering**: All list queries ordered by Id
- **Error Handling**: InvalidOperationException caught in controller → 400 Bad Request with message
- **DTO Mapping**: Static MapToDto() method to prevent repeated projections
- **Timestamps**: EntryDate set to UtcNow on creation; UpdatedAt set on modifications
