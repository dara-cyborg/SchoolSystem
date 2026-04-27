using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Core.DTOs.Auth;

namespace SchoolSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase {
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto loginDto) {
        var result = await _authService.LoginAsync(loginDto);

        if (result == null) {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        return Ok(result);
    }
}