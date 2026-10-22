using DTOs;

namespace BlazorApp.Services;

public interface IUserService
{
    Task<UserDto> AddUserAsync(CreateUserDto request);
    Task<UserDto> GetUserAsync(int id);
    Task<List<UserDto>> GetUsersAsync();
    Task UpdateUserAsync(int id, UpdateUserDto request);
    Task DeleteUserAsync(int id);
}