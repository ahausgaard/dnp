namespace ass1;

public class Post
{
    private string title { get; set; }
    private string body { get; set; }
    public int id { get; private set; }
    
    public Post(string title, string body)
    {
        this.title = title;
        this.body = body;
        id = Random.Shared.Next(1, 100000001);
    }
}