namespace ass1;

public class User
{
    private string name { get; set;}
    private string password { get; set; }
    
    public User(string name, string password)
    {
        this.name = name;
        this.password = password;
    }
}