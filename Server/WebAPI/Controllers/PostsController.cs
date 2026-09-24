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
}