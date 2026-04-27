using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Core.DTOs.ClassSubject;
using SchoolSystem.Core.Interfaces;

namespace SchoolSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "SuperAdminOnly")]
public class ClassSubjectsController : ControllerBase {
    private readonly IAcademicService _academicService;

    public ClassSubjectsController(IAcademicService academicService) {
        _academicService = academicService;
    }

    /// <summary>
    /// Get paginated list of class-subject assignments
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetClassSubjects([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default) {
        if (page < 1 || pageSize < 1) {
            return BadRequest(new { message = "Page and PageSize must be greater than 0" });
        }

        var result = await _academicService.GetClassSubjectsAsync(page, pageSize, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get a specific class-subject assignment by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetClassSubject(int id, CancellationToken cancellationToken = default) {
        var classSubject = await _academicService.GetClassSubjectByIdAsync(id, cancellationToken);

        if (classSubject == null) {
            return NotFound(new { message = "Class-Subject not found" });
        }

        return Ok(classSubject);
    }

    /// <summary>
    /// Create a new class-subject assignment with optional teacher assignment
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateClassSubject([FromBody] CreateClassSubjectDto createClassSubjectDto, CancellationToken cancellationToken = default) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        if (createClassSubjectDto.ClassId <= 0 || createClassSubjectDto.SubjectId <= 0) {
            return BadRequest(new { message = "ClassId and SubjectId are required and must be greater than 0" });
        }

        try {
            var classSubject = await _academicService.CreateClassSubjectAsync(createClassSubjectDto, cancellationToken);
            return CreatedAtAction(nameof(GetClassSubject), new { id = classSubject.Id }, classSubject);
        } catch (InvalidOperationException ex) {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Update a class-subject assignment (typically to assign/change teacher)
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClassSubject(int id, [FromBody] UpdateClassSubjectDto updateClassSubjectDto, CancellationToken cancellationToken = default) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        try {
            var success = await _academicService.UpdateClassSubjectAsync(id, updateClassSubjectDto, cancellationToken);

            if (!success) {
                return NotFound(new { message = "Class-Subject not found" });
            }

            var classSubject = await _academicService.GetClassSubjectByIdAsync(id, cancellationToken);
            return Ok(classSubject);
        } catch (InvalidOperationException ex) {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Delete a class-subject assignment
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClassSubject(int id, CancellationToken cancellationToken = default) {
        try {
            var success = await _academicService.DeleteClassSubjectAsync(id, cancellationToken);

            if (!success) {
                return NotFound(new { message = "Class-Subject not found" });
            }

            return NoContent();
        } catch (InvalidOperationException ex) {
            return BadRequest(new { message = ex.Message });
        }
    }
}
