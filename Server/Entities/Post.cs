namespace Entities;

public class Post
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Body { get; set; }
    public User User { get; set; } = null!;
    public List<Comment> Comments { get; set; } = new();
}
