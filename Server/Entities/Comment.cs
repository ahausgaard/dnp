namespace ass1;

public class Comment
{
    public int Id { get; } = IdGenerator.NextId();

    public string Body { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }

    public Comment(string body)
    {
        this.Body = body;
    }
}