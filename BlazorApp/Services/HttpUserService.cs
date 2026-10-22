using DTOs;

namespace BlazorApp.Services;

public class HttpUserService(HttpClient client) : IUserService
{
    public async Task<UserDto> AddUserAsync(CreateUserDto request)
    {
        var response = await client.PostAsJsonAsync("users", request);
        if(!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());

        return (await response.Content.ReadFromJsonAsync<UserDto>())!;
    }

    public async Task<UserDto> GetUserAsync(int id)
    {
        var response = await client.GetAsync($"users/{id}");
        if(!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());

        return (await response.Content.ReadFromJsonAsync<UserDto>())!;
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