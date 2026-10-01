using DTOs;
using Entities;

namespace WebAPI.Mappings;

public static class Mappings
{
    public static UserDto ToDto(this User user) =>
        new()
        {
            Id = user.Id,
            UserName = user.UserName
        };

    public static CommentDto ToDto(this Comment comment, User? author) =>
        new()
        {
            Id = comment.Id, 
            Body = comment.Body, 
            Author = author?.ToDto(),
            PostId = comment.PostId
        };

    public static PostDto ToDto(this Post post, User? author,
        List<CommentDto>? comments = null) =>
        new()
        {
            Id = post.Id, 
            Title = post.Title, 
            Body = post.Body,
            Author = author?.ToDto(),
            Comments = comments
        };
}