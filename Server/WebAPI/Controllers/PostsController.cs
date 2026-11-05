using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using WebAPI.Mappings;

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
        User author;
        try
        {
            author = await userRepo.GetSingleAsync(request.UserId);
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }

        Post post = new()
        {
            Title = request.Title,
            Body = request.Body,
            User = author
        };

        Post created = await postRepo.AddAsync(post);

        PostDto dto = created.ToDto(author);

        return Created($"/posts/{dto.Id}", dto);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostDto>> GetPost(
        [FromRoute] int id,
        [FromQuery] bool includeComments = false)
    {
        PostDto? dto = await ProjectToDto(
                postRepo.GetMany().Where(p => p.Id == id), includeComments)
            .SingleOrDefaultAsync();

        if (dto is null)
            return NotFound($"Post with id {id} not found");

        if (!includeComments)
            dto.Comments = null;

        return Ok(dto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetPosts(
        [FromQuery] string? titleContains, [FromQuery] int? userId,
        [FromQuery] string? userName, [FromQuery] bool includeComments = false)
    {
        IQueryable<Post> query = postRepo.GetMany();

        if (titleContains is not null)
            query = query.Where(p =>
                p.Title.ToLower().Contains(titleContains.ToLower()));

        if (userId is not null)
        {
            query = query.Where(p => p.User.Id == userId);
        }
        else if (userName is not null)
        {
            if (!await userRepo.GetMany().AnyAsync(u => u.UserName == userName))
                return NotFound($"User {userName} not found");
            query = query.Where(p => p.User.UserName == userName);
        }

        List<PostDto> posts =
            await ProjectToDto(query, includeComments).ToListAsync();

        if (!includeComments)
            posts.ForEach(p => p.Comments = null);

        return Ok(posts);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdatePost(
        [FromRoute] int id, [FromBody] UpdatePostDto request)
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
    public async Task<ActionResult> DeletePost([FromRoute] int id)
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

    // Translated by EF into a single SQL query (joins Users and Comments).
    // The Where(c => includeComments) trick skips the comment rows when not
    // requested; callers set Comments to null afterwards so it's omitted.
    private static IQueryable<PostDto> ProjectToDto(IQueryable<Post> query,
        bool includeComments) =>
        query.Select(p => new PostDto
        {
            Id = p.Id,
            Title = p.Title,
            Body = p.Body,
            Author = new UserDto { Id = p.User.Id, UserName = p.User.UserName },
            Comments = p.Comments
                .Where(c => includeComments)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Body = c.Body,
                    PostId = c.Post.Id, // not p.Id: referencing the outer p here needs SQL APPLY, which SQLite lacks
                    Author = new UserDto
                        { Id = c.User.Id, UserName = c.User.UserName }
                })
                .ToList()
        });
}
