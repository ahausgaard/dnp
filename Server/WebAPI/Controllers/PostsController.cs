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
    public async Task<ActionResult<PostDto>> GetSingle(
        [FromRoute] int id, [FromQuery] bool includeComments = false)
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

            if (includeComments)
            {
                dto.Comments = commentRepo.GetMany()
                    .Where(c => c.PostId == id)
                    .Select(c => new CommentDto
                    {
                        Id = c.Id,
                        Body = c.Body,
                        UserId = c.UserId,
                        PostId = c.PostId
                    }).ToList();
            }
            return Ok(dto);
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<PostDto>> GetAllPosts(
        [FromQuery] string? titleContains, [FromQuery] int? userId,  [FromQuery] string? userName, [FromQuery] bool includeComments = false)
    {
        IQueryable<Post> posts = postRepo.GetMany();

        if (titleContains is not null)
        {
            posts = posts.Where(x => x.Title.Contains(titleContains));
        }

        if (userId is not null)
        {
            posts = posts.Where(x => x.UserId == userId);
        }

        if (userName is not null)
        {
            User? user = userRepo.GetMany()
                .SingleOrDefault(x => x.UserName == userName);

            if (user is null)
            {
                return NotFound($"User {userName} not found");
            }

            posts = posts.Where(p => p.UserId == user.Id);
        }
        
        

        List<PostDto> dtos = posts.Select(p => new PostDto()
        {
            Id = p.Id,
            Title = p.Title,
            Body = p.Body,
            UserId = p.UserId,
        }).ToList();
        
        if (includeComments)
        {
            List<int> postIds = dtos.Select(p => p.Id).ToList();

            Dictionary<int, List<CommentDto>> commentsByPost = commentRepo
                .GetMany()
                .Where(c => postIds.Contains(c.PostId))
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Body = c.Body,
                    UserId = c.UserId,
                    PostId = c.PostId
                })
                .ToList()
                .GroupBy(c => c.PostId)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (PostDto dto in dtos)
            {
                dto.Comments = commentsByPost.GetValueOrDefault(dto.Id, new List<CommentDto>());
            }
        }
        
        return Ok(dtos);
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