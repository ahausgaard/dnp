using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class PostInMemoryRepository : IPostRepository
{
    private readonly List<Post> posts = new();

    public PostInMemoryRepository()
    {
        AddDummyData();
    }

    private void AddDummyData()
    {
        posts.Add(new Post
        {
            Id = 1,
            Title = "Welcome to the forum",
            Body = "This is the very first post. Say hello!",
            UserId = 1
        });
        posts.Add(new Post
        {
            Id = 2,
            Title = "Anyone else struggling with async?",
            Body = "I keep forgetting to await things and nothing happens.",
            UserId = 2
        });
        posts.Add(new Post
        {
            Id = 3,
            Title = "Best coffee near campus",
            Body = "The place across the street is criminally underrated.",
            UserId = 3
        });
    }

    public Task<Post> AddAsync(Post post)
    {
        post.Id = posts.Any()
            ? posts.Max(p => p.Id) + 1
            : 1;
        posts.Add(post);
        return Task.FromResult(post);
    }

    public Task UpdateAsync(Post post)
    {
        Post? existingPost = posts.SingleOrDefault(p => p.Id == post.Id);
        if (existingPost is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{post.Id}' not found");
        }

        posts.Remove(existingPost);
        posts.Add(post);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Post? postToRemove = posts.SingleOrDefault(p => p.Id == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        posts.Remove(postToRemove);
        return Task.CompletedTask;
    }

    public Task<Post> GetSingleAsync(int id)
    {
        Post? post = posts.SingleOrDefault(p => p.Id == id);
        if (post is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        return Task.FromResult(post);
    }

    public IQueryable<Post> GetManyAsync()
    {
        return posts.AsQueryable();
    }
}
