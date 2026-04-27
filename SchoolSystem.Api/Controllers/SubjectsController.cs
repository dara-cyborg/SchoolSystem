using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Core.DTOs.Subject;
using SchoolSystem.Core.Interfaces;

namespace SchoolSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "SuperAdminOnly")]
public class SubjectsController : ControllerBase {
    private readonly IAcademicService _academicService;

    public SubjectsController(IAcademicService academicService) {
        _academicService = academicService;
    }

    /// <summary>
    /// Get paginated list of subjects
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetSubjects([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default) {
        if (page < 1 || pageSize < 1) {
            return BadRequest(new { message = "Page and PageSize must be greater than 0" });
        }

        var result = await _academicService.GetSubjectsAsync(page, pageSize, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get a specific subject by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSubject(int id, CancellationToken cancellationToken = default) {
        var subject = await _academicService.GetSubjectByIdAsync(id, cancellationToken);

        if (subject == null) {
            return NotFound(new { message = "Subject not found" });
        }

        return Ok(subject);
    }

    /// <summary>
    /// Create a new subject
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateSubject([FromBody] CreateSubjectDto createSubjectDto, CancellationToken cancellationToken = default) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        if (string.IsNullOrWhiteSpace(createSubjectDto.Name)) {
            return BadRequest(new { message = "Subject name is required" });
        }

        try {
            var subject = await _academicService.CreateSubjectAsync(createSubjectDto, cancellationToken);
            return CreatedAtAction(nameof(GetSubject), new { id = subject.Id }, subject);
        } catch (InvalidOperationException ex) {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing subject
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSubject(int id, [FromBody] UpdateSubjectDto updateSubjectDto, CancellationToken cancellationToken = default) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        if (string.IsNullOrWhiteSpace(updateSubjectDto.Name)) {
            return BadRequest(new { message = "Subject name is required" });
        }

        try {
            var success = await _academicService.UpdateSubjectAsync(id, updateSubjectDto, cancellationToken);

            if (!success) {
                return NotFound(new { message = "Subject not found" });
            }

            var subject = await _academicService.GetSubjectByIdAsync(id, cancellationToken);
            return Ok(subject);
        } catch (InvalidOperationException ex) {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Delete a subject
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubject(int id, CancellationToken cancellationToken = default) {
        try {
            var success = await _academicService.DeleteSubjectAsync(id, cancellationToken);

            if (!success) {
                return NotFound(new { message = "Subject not found" });
            }

            return NoContent();
        } catch (InvalidOperationException ex) {
            return BadRequest(new { message = ex.Message });
        }
    }
}
