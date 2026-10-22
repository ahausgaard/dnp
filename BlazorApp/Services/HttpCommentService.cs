using DTOs;

namespace BlazorApp.Services;

public class HttpCommentService(HttpClient client) : ICommentService
{
    public async Task<CommentDto> AddCommentAsync(CreateCommentDto request)
    {
        var response = await client.PostAsJsonAsync("posts", request);
        if(!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());

        return (await response.Content.ReadFromJsonAsync<CommentDto>())!;
    }

    public async Task<CommentDto> GetCommentAsync(int id)
    {
        var response = await client.GetAsync($"comments/{id}");
        if(!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());

        return (await response.Content.ReadFromJsonAsync<CommentDto>())!;
    }

    public Task<List<CommentDto>> GetCommentsAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdateCommentAsync(int id, UpdateCommentDto request)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCommentAsync(int id)
    {
        throw new NotImplementedException();
    }
}