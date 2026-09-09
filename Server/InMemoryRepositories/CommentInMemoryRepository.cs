using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class CommentInMemoryRepository : ICommentRepository
{
    private readonly List<Comment> comments = new();

    public CommentInMemoryRepository()
    {
        AddDummyData();
    }

    private void AddDummyData()
    {
        comments.Add(new Comment { Id = 1, Body = "Hello right back!", UserId = 2, PostId = 1 });
        comments.Add(new Comment { Id = 2, Body = "Great to be here.", UserId = 3, PostId = 1 });
        comments.Add(new Comment { Id = 3, Body = "Await everything, that is my strategy.", UserId = 1, PostId = 2 });
        comments.Add(new Comment { Id = 4, Body = "Same, took me a week to get it.", UserId = 3, PostId = 2 });
        comments.Add(new Comment { Id = 5, Body = "Strongly disagree, it is burnt.", UserId = 1, PostId = 3 });
    }

    public Task<Comment> AddAsync(Comment comment)
    {
        comment.Id = comments.Any()
            ? comments.Max(c => c.Id) + 1
            : 1;
        comments.Add(comment);
        return Task.FromResult(comment);
    }

    public Task UpdateAsync(Comment comment)
    {
        Comment? existingComment = comments.SingleOrDefault(c => c.Id == comment.Id);
        if (existingComment is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{comment.Id}' not found");
        }

        comments.Remove(existingComment);
        comments.Add(comment);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Comment? commentToRemove = comments.SingleOrDefault(c => c.Id == id);
        if (commentToRemove is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{id}' not found");
        }

        comments.Remove(commentToRemove);
        return Task.CompletedTask;
    }

    public Task<Comment> GetSingleAsync(int id)
    {
        Comment? comment = comments.SingleOrDefault(c => c.Id == id);
        if (comment is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{id}' not found");
        }

        return Task.FromResult(comment);
    }

    public IQueryable<Comment> GetManyAsync()
    {
        return comments.AsQueryable();
    }
}
