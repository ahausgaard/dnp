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

    public CommentsController(ICommentRepository commentRepo, IUserRepository userRepo, IPostRepository postRepo)
    {
        this.commentRepo = commentRepo;
        this.userRepo = userRepo;
        this.postRepo = postRepo;
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> AddComment([FromBody] CreateCommentDto request)
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

    /*[HttpGet]
    public async Task<ActionResult<CommentDto>> GetSingle(CommentDto request)
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
        return Created($"/comments/{dto.Id}", dto);
    }
    */
    
}
