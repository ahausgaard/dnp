using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;

    public PostsController(IPostRepository postRepo, IUserRepository userRepo)
    {
        this.postRepo = postRepo;
        this.userRepo = userRepo;
    }

    [HttpPost]
    public async Task<ActionResult<PostDto>> AddPost(
        [FromBody] CreatePostDto request)
    {
        try
        {
            await userRepo.GetSingleAsync(request.UserId);
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        
        Post post = new()
        {
            Title = request.Title,
            Body = request.Body,
            UserId = request.UserId
        };

        Post created = await postRepo.AddAsync(post);
        PostDto dto = new()
        {
            Id = created.Id,
            Title = created.Title,
            Body = created.Body,
            UserId = created.UserId
        };

        return Created($"/posts/{dto.Id}", dto);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostDto>> GetSingle([FromRoute] int id)
    {
        try
        {
            Post post = await postRepo.GetSingleAsync(id);
            PostDto dto = new()
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId
            };
            return Ok(dto);
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UpdatePostDto>> UpdatePost([FromRoute] int id, [FromBody] UpdatePostDto request)
    {
        try
        {
            Post post = await postRepo.GetSingleAsync(id);
            post.Title = request.Title;
            post.Body = request.Body;
            await postRepo.UpdateAsync(post);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<PostDto>> DeleteSingle([FromRoute] int id)
    {
        try
        {
            await postRepo.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }
    
    
}