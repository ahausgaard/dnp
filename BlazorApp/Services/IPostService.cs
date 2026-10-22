using DTOs;

namespace BlazorApp.Services;

public interface IPostService
{
    Task<PostDto> AddPostAsync(CreatePostDto request);
    Task<PostDto> GetPostAsync(int id);
    Task<List<PostDto>> GetPostsAsync();
    Task UpdatePostAsync(int id, UpdatePostDto post);
    Task DeletePostAsync(int id);
}