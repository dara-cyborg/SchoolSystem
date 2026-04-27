using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Core.DTOs.Class;
using SchoolSystem.Core.Interfaces;

namespace SchoolSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClassesController : ControllerBase {
    private readonly IAcademicService _academicService;

    public ClassesController(IAcademicService academicService) {
        _academicService = academicService;
    }

    /// <summary>
    /// Get paginated list of classes with optional filtering by gradeId and schoolYear
    /// [super_admin, homeroom]
    /// </summary>
    [HttpGet]
    [Authorize(Policy = "SuperAdminOrHomeroom")]
    public async Task<IActionResult> GetClasses([FromQuery] int? gradeId, [FromQuery] short? schoolYear, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default) {
        if (page < 1 || pageSize < 1) {
            return BadRequest(new { message = "Page and PageSize must be greater than 0" });
        }

        var result = await _academicService.GetClassesAsync(gradeId, schoolYear, page, pageSize, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get a specific class by ID
    /// [super_admin]
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Policy = "SuperAdminOnly")]
    public async Task<IActionResult> GetClass(int id, CancellationToken cancellationToken = default) {
        var cls = await _academicService.GetClassByIdAsync(id, cancellationToken);

        if (cls == null) {
            return NotFound(new { message = "Class not found" });
        }

        return Ok(cls);
    }

    /// <summary>
    /// Get all students in a class
    /// [super_admin, homeroom]
    /// </summary>
    [HttpGet("{id}/students")]
    [Authorize(Policy = "SuperAdminOrHomeroom")]
    public async Task<IActionResult> GetClassStudents(int id, CancellationToken cancellationToken = default) {
        // Verify the class exists
        var cls = await _academicService.GetClassByIdAsync(id, cancellationToken);
        if (cls == null) {
            return NotFound(new { message = "Class not found" });
        }

        var students = await _academicService.GetClassStudentsAsync(id, cancellationToken);
        return Ok(students);
    }

    /// <summary>
    /// Create a new class
    /// [super_admin]
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "SuperAdminOnly")]
    public async Task<IActionResult> CreateClass([FromBody] CreateClassDto createClassDto, CancellationToken cancellationToken = default) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        if (string.IsNullOrWhiteSpace(createClassDto.Name)) {
            return BadRequest(new { message = "Class name is required" });
        }

        try {
            var cls = await _academicService.CreateClassAsync(createClassDto, cancellationToken);
            return CreatedAtAction(nameof(GetClass), new { id = cls.Id }, cls);
        } catch (InvalidOperationException ex) {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing class
    /// [super_admin]
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Policy = "SuperAdminOnly")]
    public async Task<IActionResult> UpdateClass(int id, [FromBody] UpdateClassDto updateClassDto, CancellationToken cancellationToken = default) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        if (string.IsNullOrWhiteSpace(updateClassDto.Name)) {
            return BadRequest(new { message = "Class name is required" });
        }

        try {
            var cls = await _academicService.UpdateClassAsync(id, updateClassDto, cancellationToken);

            if (cls == null) {
                return NotFound(new { message = "Class not found" });
            }

            return Ok(cls);
        } catch (InvalidOperationException ex) {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Delete a class
    /// [super_admin]
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Policy = "SuperAdminOnly")]
    public async Task<IActionResult> DeleteClass(int id, CancellationToken cancellationToken = default) {
        try {
            var success = await _academicService.DeleteClassAsync(id, cancellationToken);

            if (!success) {
                return NotFound(new { message = "Class not found" });
            }

            return NoContent();
        } catch (InvalidOperationException ex) {
            return BadRequest(new { message = ex.Message });
        }
    }
}
