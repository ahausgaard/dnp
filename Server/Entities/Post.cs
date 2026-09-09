namespace ass1;

public class Post
{
    private string Title { get; set; }
    private string Body { get; set; }
    public int Id { get; set; }
    public int UserId { get; set; }
    
    public Post(string title)
    {
        this.Title = title;
    }
}