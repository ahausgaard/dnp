namespace DTOs;

public class PostDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Body { get; set; }
    public UserDto? Author { get; set; }
    public List<CommentDto>? Comments { get; set; }
}