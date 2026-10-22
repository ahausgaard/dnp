using DTOs;

namespace BlazorApp.Services;

public class HttpPostService(HttpClient client) : IPostService
{
    public async Task<PostDto> AddPostAsync(CreatePostDto request)
    {
        var response = await client.PostAsJsonAsync("posts", request);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<PostDto>())!;
    }

    public Task<PostDto> GetPostAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<PostDto>> GetPostsAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdatePostAsync(int id, UpdatePostDto post)
    {
        throw new NotImplementedException();
    }

    public Task DeletePostAsync(int id)
    {
        throw new NotImplementedException();
    }
}