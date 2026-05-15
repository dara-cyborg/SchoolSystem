using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Core.DTOs.ClassSubject;
using SchoolSystem.Core.Extensions;
using SchoolSystem.Core.Interfaces;

namespace SchoolSystem.Api.Controllers;

[ApiController]
[Route("api/teachers")]
public class TeacherController : ControllerBase
{
    private readonly ITeacherService _teacherService;

    public TeacherController(ITeacherService teacherService)
    {
        _teacherService = teacherService;
    }

    [HttpGet("my-class-subjects")]
    [Authorize(Policy = "TeacherOnly")]
    public async Task<ActionResult<List<ClassSubjectWithStudentsDto>>> GetMyClassSubjectsAsync()
    {
        var teacherUserId = User.GetUserId();
        var result = await _teacherService.GetMyClassSubjectsAsync(teacherUserId);
        return Ok(result);
    }
}