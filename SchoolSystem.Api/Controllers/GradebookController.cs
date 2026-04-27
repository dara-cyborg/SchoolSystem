using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Core.DTOs.Gradebook;
using SchoolSystem.Core.Extensions;
using SchoolSystem.Core.Interfaces;

namespace SchoolSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GradebookController : ControllerBase {
    private readonly IGradebookService _gradebookService;

    public GradebookController(IGradebookService gradebookService) {
        _gradebookService = gradebookService;
    }

    /// <summary>
    /// Create a gradebook entry
    /// [teacher]
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "TeacherOnly")]
    public async Task<IActionResult> CreateGradebookEntry([FromBody] CreateGradebookEntryDto createDto, CancellationToken cancellationToken = default) {
        try {
            var teacherUserId = User.GetUserId();
            var result = await _gradebookService.CreateAsync(createDto, teacherUserId, cancellationToken);
            return CreatedAtAction(nameof(GetGradebookEntry), new { id = result.Id }, result);
        } catch (InvalidOperationException ex) {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get a gradebook entry by ID
    /// [teacher, homeroom]
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Policy = "TeacherOrHomeroom")]
    public async Task<IActionResult> GetGradebookEntry(int id, CancellationToken cancellationToken = default) {
        var result = await _gradebookService.GetByIdAsync(id, cancellationToken);
        if (result == null) {
            return NotFound(new { message = $"Gradebook entry with ID {id} does not exist." });
        }
        return Ok(result);
    }

    /// <summary>
    /// Update a gradebook entry
    /// [teacher]
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Policy = "TeacherOnly")]
    public async Task<IActionResult> UpdateGradebookEntry(int id, [FromBody] UpdateGradebookEntryDto updateDto, CancellationToken cancellationToken = default) {
        try {
            var teacherUserId = User.GetUserId();
            var result = await _gradebookService.UpdateAsync(id, updateDto, teacherUserId, cancellationToken);
            if (result == null) {
                return NotFound(new { message = $"Gradebook entry with ID {id} does not exist." });
            }
            return Ok(result);
        } catch (InvalidOperationException ex) {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Delete a gradebook entry
    /// [teacher]
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Policy = "TeacherOnly")]
    public async Task<IActionResult> DeleteGradebookEntry(int id, CancellationToken cancellationToken = default) {
        try {
            var teacherUserId = User.GetUserId();
            var deleted = await _gradebookService.DeleteAsync(id, teacherUserId, cancellationToken);
            if (!deleted) {
                return NotFound(new { message = $"Gradebook entry with ID {id} does not exist." });
            }
            return NoContent();
        } catch (InvalidOperationException ex) {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get all gradebook entries for a class-subject
    /// [teacher, homeroom]
    /// </summary>
    [HttpGet("class-subject/{classSubjectId}")]
    [Authorize(Policy = "TeacherOrHomeroom")]
    public async Task<IActionResult> GetGradebookByClassSubject(int classSubjectId, CancellationToken cancellationToken = default) {
        var result = await _gradebookService.GetByClassSubjectAsync(classSubjectId, cancellationToken);
        if (result == null) {
            return NotFound(new { message = $"ClassSubject with ID {classSubjectId} does not exist." });
        }
        return Ok(result);
    }

    /// <summary>
    /// Get gradebook entries for a specific student in a class-subject
    /// [teacher, homeroom]
    /// </summary>
    [HttpGet("student/{studentId}/class-subject/{classSubjectId}")]
    [Authorize(Policy = "TeacherOrHomeroom")]
    public async Task<IActionResult> GetGradebookByStudentAndClassSubject(int studentId, int classSubjectId, CancellationToken cancellationToken = default) {
        var result = await _gradebookService.GetByStudentAndClassSubjectAsync(studentId, classSubjectId, cancellationToken);
        if (result == null) {
            return NotFound(new { message = $"ClassSubject with ID {classSubjectId} does not exist." });
        }
        return Ok(result);
    }
}
