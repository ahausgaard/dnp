namespace Entities;

public class Comment
{
    public int Id { get; set; }
    public required string Body { get; set; }
    public User User { get; set; } = null!;
    public Post Post { get; set; } = null!;
}
