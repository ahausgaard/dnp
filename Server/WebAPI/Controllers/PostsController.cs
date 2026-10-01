using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using WebAPI.Mappings;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;
    private readonly ICommentRepository commentRepo;

    public PostsController(IPostRepository postRepo, IUserRepository userRepo,
        ICommentRepository commentRepo)
    {
        this.postRepo = postRepo;
        this.userRepo = userRepo;
        this.commentRepo = commentRepo;
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
            UserId = request.UserId
        };

        Post created = await postRepo.AddAsync(post);

        PostDto dto = created.ToDto(author);

        return Created($"/posts/{dto.Id}", dto);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostDto>> GetSingle(
        [FromRoute] int id, [FromQuery] bool includeComments = false)
    {
        try
        {
            Post post = await postRepo.GetSingleAsync(id);
            Dictionary<int, User> usersById =
                userRepo.GetMany().ToDictionary(u => u.Id);

            List<CommentDto>? comments = includeComments
                ? commentRepo.GetMany()
                    .Where(c => c.PostId == id)
                    .ToList()
                    .Select(c => c.ToDto(usersById.GetValueOrDefault(c.UserId)))
                    .ToList()
                : null;

            PostDto dto = post.ToDto(usersById.GetValueOrDefault(post.UserId),
                comments);
            return Ok(dto);
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<PostDto>> GetAllPosts(
        [FromQuery] string? titleContains, [FromQuery] int? userId,
        [FromQuery] string? userName, [FromQuery] bool includeComments = false)
    {
        IQueryable<Post> posts = postRepo.GetMany();

        Dictionary<int, User> usersById =
            userRepo.GetMany().ToDictionary(u => u.Id);

        if (titleContains is not null)
        {
            posts = posts.Where(x => x.Title.Contains(titleContains));
        }

        if (userId is null && userName is not null)
        {
            User? user =
                usersById.Values.SingleOrDefault(u => u.UserName == userName);
            if (user is null)
                return NotFound($"User {userName} not found");
            userId = user.Id;
        }

        if (userId is not null)
            posts = posts.Where(x => x.UserId == userId);

        List<Post> postList = posts.ToList();

        Dictionary<int, List<CommentDto>> commentsByPost = includeComments
            ? commentRepo.GetMany()
                .ToList()
                .GroupBy(c => c.PostId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(c =>
                            c.ToDto(usersById.GetValueOrDefault(c.UserId)))
                        .ToList())
            : [];

        List<PostDto> dtos = postList
            .Select(p => p.ToDto(usersById.GetValueOrDefault(p.UserId),
                includeComments
                    ? commentsByPost.GetValueOrDefault(p.Id, [])
                    : null))
            .ToList();

        return Ok(dtos);
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
    public async Task<ActionResult> DeleteSingle([FromRoute] int id)
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