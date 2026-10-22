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

    public async Task<List<UserDto>> GetUsersAsync()
    {
        var response = await client.GetAsync("users");
        if(!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());

        return (await response.Content.ReadFromJsonAsync<List<UserDto>>())!;
    }

    public async Task UpdateUserAsync(int id, UpdateUserDto request)
    {
        var response = await client.PutAsJsonAsync($"users/{id}", request);
        if(!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());
    }

    public async Task DeleteUserAsync(int id)
    {
        var response = await client.DeleteAsync($"users/{id}");
        if(!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());
    }
}