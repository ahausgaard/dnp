namespace ass1;

public class User
{
    private string name { get; set;}
    private string password { get; set; }
    public int id { get; } = IdGenerator.NextId();
    
    public User(string name, string password)
    {
        this.name = name;
        this.password = password;
    }

    public Post createPost(string title)
    {
        return new Post(title);
    }
}