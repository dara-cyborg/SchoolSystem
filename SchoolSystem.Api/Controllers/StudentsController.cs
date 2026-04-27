using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Core.DTOs.Student;
using SchoolSystem.Core.Extensions;
using SchoolSystem.Core.Interfaces;

namespace SchoolSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase {
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService) {
        _studentService = studentService;
    }

    /// <summary>
    /// Get paginated list of all students
    /// [super_admin]
    /// </summary>
    [HttpGet]
    [Authorize(Policy = "SuperAdminOrHomeroom")]
    public async Task<IActionResult> GetStudents([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default) {
        if (page < 1 || pageSize < 1) {
            return BadRequest(new { message = "Page and PageSize must be greater than 0" });
        }

        var result = await _studentService.GetStudentsAsync(page, pageSize, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get a specific student by ID
    /// [super_admin]
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Policy = "SuperAdminOnly")]
    public async Task<IActionResult> GetStudent(int id, CancellationToken cancellationToken = default) {
        var student = await _studentService.GetStudentByIdAsync(id, cancellationToken);

        if (student == null) {
            return NotFound(new { message = "Student not found" });
        }

        return Ok(student);
    }

    /// <summary>
    /// Get paginated list of students in a class
    /// [super_admin, homeroom, teacher]
    /// </summary>
    [HttpGet("class/{classId}")]
    [Authorize(Policy = "SuperAdminOrHomeroomOrTeacher")]
    public async Task<IActionResult> GetStudentsByClass(int classId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default) {
        if (page < 1 || pageSize < 1) {
            return BadRequest(new { message = "Page and PageSize must be greater than 0" });
        }

        var result = await _studentService.GetStudentsByClassAsync(classId, page, pageSize, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get paginated list of students linked to a parent
    /// [super_admin, parent]
    /// </summary>
    [HttpGet("parent/{parentUserId}")]
    [Authorize(Policy = "SuperAdminOrParent")]
    public async Task<IActionResult> GetStudentsByParent(int parentUserId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default) {
        if (page < 1 || pageSize < 1) {
            return BadRequest(new { message = "Page and PageSize must be greater than 0" });
        }

        // Parent can only retrieve their own linked children unless they are super_admin
        var userId = User.GetUserId();
        if (User.HasRole("SuperAdmin")) {
            // Super admin can retrieve any parent's students
        } else if (User.HasRole("Parent")) {
            // Parent can only retrieve their own students
            if (userId != parentUserId) {
                return Forbid();
            }
        } else {
            return Forbid();
        }

        try {
            var result = await _studentService.GetStudentsByParentAsync(parentUserId, page, pageSize, cancellationToken);
            return Ok(result);
        } catch (InvalidOperationException ex) {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Create a new student
    /// [super_admin, homeroom]
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "SuperAdminOrHomeroom")]
    public async Task<IActionResult> CreateStudent([FromBody] CreateStudentDto createStudentDto, CancellationToken cancellationToken = default) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        if (string.IsNullOrWhiteSpace(createStudentDto.Name)) {
            return BadRequest(new { message = "Student name is required" });
        }

        try {
            var student = await _studentService.CreateStudentAsync(createStudentDto, cancellationToken);
            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
        } catch (InvalidOperationException ex) {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing student
    /// [super_admin, homeroom]
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Policy = "SuperAdminOrHomeroom")]
    public async Task<IActionResult> UpdateStudent(int id, [FromBody] UpdateStudentDto updateStudentDto, CancellationToken cancellationToken = default) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        if (string.IsNullOrWhiteSpace(updateStudentDto.Name)) {
            return BadRequest(new { message = "Student name is required" });
        }

        try {
            var student = await _studentService.UpdateStudentAsync(id, updateStudentDto, cancellationToken);

            if (student == null) {
                return NotFound(new { message = "Student not found" });
            }

            return Ok(student);
        } catch (InvalidOperationException ex) {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Delete a student
    /// [super_admin]
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Policy = "SuperAdminOnly")]
    public async Task<IActionResult> DeleteStudent(int id, CancellationToken cancellationToken = default) {
        var success = await _studentService.DeleteStudentAsync(id, cancellationToken);

        if (!success) {
            return NotFound(new { message = "Student not found" });
        }

        return NoContent();
    }

    /// <summary>
    /// Link a parent to a student
    /// [super_admin]
    /// </summary>
    [HttpPost("{id}/link-parent")]
    [Authorize(Policy = "SuperAdminOnly")]
    public async Task<IActionResult> LinkParent(int id, [FromBody] LinkParentDto linkParentDto, CancellationToken cancellationToken = default) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        try {
            await _studentService.LinkParentAsync(id, linkParentDto, cancellationToken);
            return Ok(new { message = "Parent linked successfully" });
        } catch (InvalidOperationException ex) {
            return BadRequest(new { message = ex.Message });
        }
    }
}
