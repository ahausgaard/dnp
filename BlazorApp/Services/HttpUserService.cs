using DTOs;

namespace BlazorApp.Services;

public class HttpUserService : IUserService
{
    private readonly HttpClient client;
    
    
    public HttpUserService(HttpClient client)
    {
        this.client = client;
    }
    
    public Task<UserDto> AddUserAsync(CreateUserDto request)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> GetUserAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<UserDto>> GetUsersAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdateUserAsync(int id, UpdateUserDto request)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUserAsync(int id)
    {
        throw new NotImplementedException();
    }
}