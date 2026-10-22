using DTOs;

namespace BlazorApp.Services;

public interface ICommentService
{
    Task<CommentDto> AddCommentAsync(CreateCommentDto request);
    Task<CommentDto> GetCommentAsync(int id);
    Task<List<CommentDto>> GetCommentsAsync();
    Task UpdateCommentAsync(int id, UpdateCommentDto request);
    Task DeleteCommentAsync(int id);
}