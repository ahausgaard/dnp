namespace Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public User()
    {
    }

    public User(string name, string password)
    {
        Name = name;
        Password = password;
    }

    public Post CreatePost(string title, string body)
    {
        return new Post(title, body, Id);
    }
}
