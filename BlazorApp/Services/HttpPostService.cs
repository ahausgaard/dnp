using System.Text.Json;
using DTOs;
using Microsoft.AspNetCore.Http.Json;

namespace BlazorApp.Services;

public class HttpPostService(HttpClient client) : IPostService
{
    public async Task<PostDto> AddPostAsync(CreatePostDto request)
    {
        var response = await client.PostAsJsonAsync("posts", request);
        if(!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());
        
        return (await response.Content.ReadFromJsonAsync<PostDto>())!;
    }

    public async Task<PostDto> GetPostAsync(int id)
    {
        var response = await client.GetAsync($"posts/{id}");
        if(!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());

        return (await response.Content.ReadFromJsonAsync<PostDto>())!;
    }

    public Task<List<PostDto>> GetPostsAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdatePostAsync(int id, UpdatePostDto request)
    {
        throw new NotImplementedException();
    }

    public Task DeletePostAsync(int id)
    {
        throw new NotImplementedException();
    }
}