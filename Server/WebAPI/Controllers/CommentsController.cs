using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        Post post;
        try
        {
            author = await userRepo.GetSingleAsync(request.UserId);
            post = await postRepo.GetSingleAsync(request.PostId);
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }

        Comment comment = new()
        {
            Body = request.Body,
            User = author,
            Post = post
        };
        
        Comment created = await commentRepo.AddAsync(comment);

        CommentDto dto = created.ToDto(author);
        
        return Created($"/comments/{dto.Id}", dto);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CommentDto>> GetComment([FromRoute] int id)
    {
        CommentDto? dto = await ProjectToDto(
                commentRepo.GetMany().Where(c => c.Id == id))
            .SingleOrDefaultAsync();

        if (dto is null)
            return NotFound($"Comment with id {id} not found");

        return Ok(dto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments(
        [FromQuery] int? userId, [FromQuery] string? userName, [FromQuery] int? postId)
    {
        IQueryable<Comment> query = commentRepo.GetMany();

        if (userId is not null)
        {
            query = query.Where(c => c.User.Id == userId);
        }
        else if (userName is not null)
        {
            if (!await userRepo.GetMany().AnyAsync(u => u.UserName == userName))
                return NotFound($"User {userName} not found");
            query = query.Where(c => c.User.UserName == userName);
        }

        if (postId is not null)
            query = query.Where(c => c.Post.Id == postId);

        List<CommentDto> dtos = await ProjectToDto(query).ToListAsync();

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
    public async Task<ActionResult> DeleteComment([FromRoute] int id)
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

    // Translated by EF into a single SQL query (joins Users).
    private static IQueryable<CommentDto> ProjectToDto(IQueryable<Comment> query) =>
        query.Select(c => new CommentDto
        {
            Id = c.Id,
            Body = c.Body,
            PostId = c.Post.Id,
            Author = new UserDto { Id = c.User.Id, UserName = c.User.UserName }
        });
}