using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using WebAPI.Mappings;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepo;
    private readonly IUserRepository userRepo;
    private readonly IPostRepository postRepo;

    public CommentsController(ICommentRepository commentRepo,
        IUserRepository userRepo, IPostRepository postRepo)
    {
        this.commentRepo = commentRepo;
        this.userRepo = userRepo;
        this.postRepo = postRepo;
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> AddComment(
        [FromBody] CreateCommentDto request)
    {
        User author;
        try
        {
            author = await userRepo.GetSingleAsync(request.UserId);
            await postRepo.GetSingleAsync(request.PostId);
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }

        Comment comment = new()
        {
            Body = request.Body,
            UserId = request.UserId,
            PostId = request.PostId
        };
        
        Comment created = await commentRepo.AddAsync(comment);

        CommentDto dto = created.ToDto(author);
        
        return Created($"/comments/{dto.Id}", dto);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CommentDto>> GetSingle([FromRoute] int id)
    {
        try
        {
            Comment comment = await commentRepo.GetSingleAsync(id);
            User? author = userRepo.GetMany().SingleOrDefault(u => u.Id == comment.UserId);
            
            return Ok(comment.ToDto(author));
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommentDto>> GetAllComments(
        [FromQuery] int? userId, [FromQuery] string? userName, [FromQuery] int? postId)
    {
        IQueryable<Comment> comments = commentRepo.GetMany();
        Dictionary<int, User> usersById =
            userRepo.GetMany().ToDictionary(u => u.Id);
        
        if (userId is null && userName is not null)
        {
            User? user = usersById.Values.SingleOrDefault(u => u.UserName == userName);
            if (user is null)
                return NotFound($"User {userName} not found");
            userId = user.Id;
        }
        
        if (userId is not null)
            comments = comments.Where(c => c.UserId == userId);
        
        if (postId is not null)
            comments = comments.Where(c => c.PostId == postId);

        List<CommentDto> dtos = comments
            .ToList()
            .Select(c => c.ToDto(usersById.GetValueOrDefault(c.UserId)))
            .ToList();
        
        return Ok(dtos);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateComment(
        [FromRoute] int id, [FromBody] UpdateCommentDto request)
    {
        try
        {
            Comment comment = await commentRepo.GetSingleAsync(id);
            comment.Body = request.Body;
            await commentRepo.UpdateAsync(comment);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteSingle([FromRoute] int id)
    {
        try
        {
            await commentRepo.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }
}