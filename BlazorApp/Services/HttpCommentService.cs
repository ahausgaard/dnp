using DTOs;

namespace BlazorApp.Services;

public class HttpCommentService(HttpClient client) : ICommentService
{
    public async Task<CommentDto> AddCommentAsync(CreateCommentDto request)
    {
        var response = await client.PostAsJsonAsync("comments", request);
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

    public async Task<List<CommentDto>> GetCommentsAsync()
    {
        var response = await client.GetAsync("comments");
        if(!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());

        return (await response.Content.ReadFromJsonAsync<List<CommentDto>>())!;
    }

    public async Task UpdateCommentAsync(int id, UpdateCommentDto request)
    {
        var response = await client.PutAsJsonAsync($"comments/{id}", request);
        if(!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());
    }

    public async Task DeleteCommentAsync(int id)
    {
        var response = await client.DeleteAsync($"comments/{id}");
        if(!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());
    }
}