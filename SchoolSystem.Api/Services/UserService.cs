using Microsoft.EntityFrameworkCore;
using SchoolSystem.Api.Data;
using SchoolSystem.Core.DTOs;
using SchoolSystem.Core.DTOs.User;
using SchoolSystem.Core.Exceptions;
using SchoolSystem.Core.Models;

namespace SchoolSystem.Api.Services;

public class UserService : IUserService {
    private readonly AppDbContext _context;
    private const int MaxPageSize = 100;

    public UserService(AppDbContext context) {
        _context = context;
    }

    public async Task<PagedResult<UserDto>> GetUsersAsync(
       int page,
       int pageSize,
       string? search = null,
       CancellationToken cancellationToken = default)
    {
        if (pageSize < 1) pageSize = 1;
        if (pageSize > MaxPageSize) pageSize = MaxPageSize;

        var query = _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .AsQueryable();

        // Search filter
        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim().ToLower();

            query = query.Where(u =>
                u.Name.ToLower().Contains(search) ||
                u.Contact.ToLower().Contains(search));
        }

        query = query.OrderBy(u => u.Id);

        var totalCount = await query.CountAsync(cancellationToken);

        var users = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<UserDto>
        {
            Items = users.Select(u => MapUserToDto(u)).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }
    public async Task<UserDto?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default) {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        return user == null ? null : MapUserToDto(user);
    }

    public async Task<User?> CreateUserAsync(CreateUserDto createUserDto, CancellationToken cancellationToken = default) {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);

        // Validate role IDs exist if provided
        if (createUserDto.RoleIds.Count > 0) {
            var validRoleIds = await _context.Roles
                .Where(r => createUserDto.RoleIds.Contains(r.Id))
                .Select(r => r.Id)
                .ToListAsync(cancellationToken);

            if (validRoleIds.Count != createUserDto.RoleIds.Count) {
                throw new InvalidRoleException("One or more role IDs do not exist.");
            }
        }

        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try {
            var user = new User {
                Name = createUserDto.Name,
                Sex = createUserDto.Sex,
                Dob = createUserDto.Dob,
                Contact = createUserDto.Contact,
                PasswordHash = passwordHash,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            if (createUserDto.RoleIds.Count > 0) {
                var userRoles = createUserDto.RoleIds.Select(roleId => new UserRole {
                    UserId = user.Id,
                    RoleId = roleId
                }).ToList();

                _context.UserRoles.AddRange(userRoles);
                await _context.SaveChangesAsync(cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);
            return user;
        } catch {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<bool> UpdateUserAsync(int id, UpdateUserDto updateUserDto, CancellationToken cancellationToken = default) {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        if (user == null) {
            return false;
        }

        user.Name = updateUserDto.Name;
        user.Sex = updateUserDto.Sex;
        user.Dob = updateUserDto.Dob;
        user.Contact = updateUserDto.Contact;
        user.IsActive = updateUserDto.IsActive;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteUserAsync(int id, CancellationToken cancellationToken = default) {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        if (user == null) {
            return false;
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> AssignRolesToUserAsync(int userId, List<int> roleIds, CancellationToken cancellationToken = default) {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user == null) {
            return false;
        }

        // Validate role IDs exist
        if (roleIds.Count > 0) {
            var validRoleIds = await _context.Roles
                .Where(r => roleIds.Contains(r.Id))
                .Select(r => r.Id)
                .ToListAsync(cancellationToken);

            if (validRoleIds.Count != roleIds.Count) {
                throw new InvalidRoleException("One or more role IDs do not exist.");
            }
        }

        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try {
            _context.UserRoles.RemoveRange(user.UserRoles);

            if (roleIds.Count > 0) {
                var userRoles = roleIds.Select(roleId => new UserRole {
                    UserId = userId,
                    RoleId = roleId
                }).ToList();

                _context.UserRoles.AddRange(userRoles);
            }

            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return true;
        } catch {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private static UserDto MapUserToDto(User user) {
        return new UserDto {
            Id = user.Id,
            Name = user.Name,
            Sex = user.Sex,
            Dob = user.Dob,
            Contact = user.Contact,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList()
        };
    }
}
