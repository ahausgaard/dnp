namespace DTOs;

public class CommentDto
{
    public int Id { get; set; }
    public required string Body { get; set; }
    public UserDto? Author { get; set; }
    public int PostId { get; set; }
}
