using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Api.Services;
using SchoolSystem.Core.DTOs.User;
using SchoolSystem.Core.Exceptions;

namespace SchoolSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "SuperAdminOnly")]
public class UsersController : ControllerBase {
    private readonly IUserService _userService;

    public UsersController(IUserService userService) {
        _userService = userService;
    }

    /// <summary>
    /// Get paginated list of users with their roles
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default) {
        if (page < 1 || pageSize < 1) {
            return BadRequest(new { message = "Page and PageSize must be greater than 0" });
        }

        var result = await _userService.GetUsersAsync(page, pageSize, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get a specific user by ID with their roles
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(string? search,int id, CancellationToken cancellationToken = default) {
        var user = await _userService.GetUserByIdAsync(id, cancellationToken);

        if (user == null) {
            return NotFound(new { message = "User not found" });
        }

        return Ok(user);
    }

    /// <summary>
    /// Create a new user with password hashing and role assignment
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto, CancellationToken cancellationToken = default) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        if (string.IsNullOrWhiteSpace(createUserDto.Name) || string.IsNullOrWhiteSpace(createUserDto.Password)) {
            return BadRequest(new { message = "Name and Password are required" });
        }

        try {
            var user = await _userService.CreateUserAsync(createUserDto, cancellationToken);

            if (user == null) {
                return BadRequest(new { message = "Failed to create user" });
            }

            var userDto = await _userService.GetUserByIdAsync(user.Id, cancellationToken);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, userDto);
        } catch (InvalidRoleException ex) {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Update a user (does not update password)
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto, CancellationToken cancellationToken = default) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        var success = await _userService.UpdateUserAsync(id, updateUserDto, cancellationToken);

        if (!success) {
            return NotFound(new { message = "User not found" });
        }

        var userDto = await _userService.GetUserByIdAsync(id, cancellationToken);
        return Ok(userDto);
    }

    /// <summary>
    /// Delete a user (super_admin only)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id, CancellationToken cancellationToken = default) {
        var success = await _userService.DeleteUserAsync(id, cancellationToken);

        if (!success) {
            return NotFound(new { message = "User not found" });
        }

        return NoContent();
    }

    /// <summary>
    /// Assign roles to a user
    /// </summary>
    [HttpPost("{id}/roles")]
    public async Task<IActionResult> AssignRoles(int id, [FromBody] AssignRoleDto assignRoleDto, CancellationToken cancellationToken = default) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        if (assignRoleDto.RoleIds == null || assignRoleDto.RoleIds.Count == 0) {
            return BadRequest(new { message = "At least one role is required" });
        }

        try {
            var success = await _userService.AssignRolesToUserAsync(id, assignRoleDto.RoleIds, cancellationToken);

            if (!success) {
                return NotFound(new { message = "User not found" });
            }

            var userDto = await _userService.GetUserByIdAsync(id, cancellationToken);
            return Ok(userDto);
        } catch (InvalidRoleException ex) {
            return BadRequest(new { message = ex.Message });
        }
    }
}
