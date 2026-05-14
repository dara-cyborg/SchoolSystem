using SchoolSystem.Core.DTOs;
using SchoolSystem.Core.DTOs.User;
using SchoolSystem.Core.Models;

public interface IUserService {
    Task<PagedResult<UserDto>> GetUsersAsync(
     int page,
     int pageSize,
     string? search = null,
     CancellationToken cancellationToken = default);
    Task<UserDto?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<User?> CreateUserAsync(CreateUserDto createUserDto, CancellationToken cancellationToken = default);
    Task<bool> UpdateUserAsync(int id, UpdateUserDto updateUserDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteUserAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> AssignRolesToUserAsync(int userId, List<int> roleIds, CancellationToken cancellationToken = default);
}
