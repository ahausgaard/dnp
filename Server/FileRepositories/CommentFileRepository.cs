using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository : ICommentRepository
{
    private readonly string filePath = "comments.json";
    
    public CommentFileRepository()
    {
        if (!File.Exists(filePath))
            File.WriteAllText(filePath, "[]");
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        List<Comment> comments = await JsonFile.LoadAsync<Comment>(filePath);
        comment.Id = comments.Any()
            ? comments.Max(c => c.Id) + 1
            : 1;
        comments.Add(comment);
        await JsonFile.SaveAsync(filePath, comments);
        return comment;
    }

    public async Task UpdateAsync(Comment comment)
    {
        List<Comment> comments = await JsonFile.LoadAsync<Comment>(filePath);
        
        Comment? existingComment = comments.SingleOrDefault(c => c.Id == comment.Id);
        if (existingComment is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{comment.Id}' not found");
        }

        int index = comments.IndexOf(existingComment);
        comments[index] = comment;
        
        await JsonFile.SaveAsync(filePath, comments);
    }

    public async Task DeleteAsync(int id)
    {
        List<Comment> comments = await JsonFile.LoadAsync<Comment>(filePath);
        Comment? commentToRemove = comments.SingleOrDefault(c => c.Id == id);
        if (commentToRemove is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{id}' not found");
        }

        comments.Remove(commentToRemove);
        await JsonFile.SaveAsync(filePath, comments);
    }

    public async Task<Comment> GetSingleAsync(int id)
    {
        List<Comment> comments = await JsonFile.LoadAsync<Comment>(filePath);
        Comment? comment = comments.SingleOrDefault(c => c.Id == id);
        if (comment is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{id}' not found");
        }

        return comment;
    }

    public IQueryable<Comment> GetMany()
    {
        List<Comment> comments = JsonFile.Load<Comment>(filePath);
        return comments.AsQueryable();
    }
}