using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

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
        try
        {
            await userRepo.GetSingleAsync(request.UserId);
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

        CommentDto dto = new()
        {
            Id = created.Id,
            Body = created.Body,
            UserId = created.UserId,
            PostId = created.PostId
        };
        return Created($"/comments/{dto.Id}", dto);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CommentDto>> GetSingle([FromRoute] int id)
    {
        try
        {
            Comment comment = await commentRepo.GetSingleAsync(id);
            CommentDto dto = new()
            {
                Id = comment.Id,
                Body = comment.Body,
                UserId = comment.UserId,
                PostId = comment.PostId
            };
            return Ok(dto);
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UpdateCommentDto>> UpdateComment(
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
    public async Task<ActionResult<CommentDto>> DeleteSingle([FromRoute] int id)
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