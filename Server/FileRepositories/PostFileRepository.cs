using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    private readonly string filePath = "posts.json";

    public PostFileRepository()
    {
        if (!File.Exists(filePath))
            File.WriteAllText(filePath, "[]");
    }

    public async Task<Post> AddAsync(Post post)
    {
        List<Post> posts = await JsonFile.LoadAsync<Post>(filePath);
        post.Id = posts.Any()
            ? posts.Max(p => p.Id) + 1
            : 1;
        posts.Add(post);
        await JsonFile.SaveAsync(filePath, posts);
        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        List<Post> posts = await JsonFile.LoadAsync<Post>(filePath);
        Post? existingPost = posts.SingleOrDefault(p => p.Id == post.Id);
        if (existingPost is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{post.Id}' not found");
        }

        int index = posts.IndexOf(existingPost);
        posts[index] = post;

        await JsonFile.SaveAsync(filePath, posts);
    }

    public async Task DeleteAsync(int id)
    {
        List<Post> posts = await JsonFile.LoadAsync<Post>(filePath);
        Post? postToRemove = posts.SingleOrDefault(p => p.Id == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        posts.Remove(postToRemove);
        await JsonFile.SaveAsync(filePath, posts);
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        List<Post> posts = await JsonFile.LoadAsync<Post>(filePath);
        Post? post = posts.SingleOrDefault(p => p.Id == id);
        if (post is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        return post;
    }

    public IQueryable<Post> GetMany()
    {
        List<Post> posts = JsonFile.Load<Post>(filePath);
        return posts.AsQueryable();
    }
}