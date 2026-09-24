using System.Diagnostics.CodeAnalysis;

namespace Entities;

public class User
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }

    public User()
    {
    }
    
    [SetsRequiredMembers]
    public User(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }
}
