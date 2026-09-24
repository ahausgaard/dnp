namespace DTOs;

public class UserDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Password { get; set; }
}