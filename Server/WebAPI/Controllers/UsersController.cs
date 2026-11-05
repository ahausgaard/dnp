using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using WebAPI.Mappings;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public UsersController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> AddUser(
        [FromBody] CreateUserDto request)
    {
        if (IsUserNameTaken(request.UserName))
            return Conflict($"Username '{request.UserName}' is already taken");

        User user = new()
        {
            UserName = request.UserName,
            Password = request.Password
        };
        User created = await userRepo.AddAsync(user);
        
        UserDto dto = created.ToDto();
        
        return Created($"/users/{dto.Id}", dto);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetUser([FromRoute] int id)
    {
        UserDto? dto = await ProjectToDto(
                userRepo.GetMany().Where(u => u.Id == id))
            .SingleOrDefaultAsync();

        if (dto is null)
            return NotFound($"User with id {id} not found");

        return Ok(dto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers(
        [FromQuery] string? userNameContains = null)
    {
        IQueryable<User> query = userRepo.GetMany();

        if (userNameContains is not null)
            query = query.Where(u =>
                u.UserName.ToLower().Contains(userNameContains.ToLower()));

        List<UserDto> users = await ProjectToDto(query).ToListAsync();

        return Ok(users);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateUser(
        [FromRoute] int id, [FromBody] UpdateUserDto request)
    {
        try
        {
            User user = await userRepo.GetSingleAsync(id);
            
            if (IsUserNameTaken(request.UserName, id))
                return Conflict($"Username '{request.UserName}' is already taken");
            
            user.UserName = request.UserName;
            await userRepo.UpdateAsync(user);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteUser([FromRoute] int id)
    {
        try
        {
            await userRepo.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }
    
    private bool IsUserNameTaken(string userName, int? exceptUserId = null) => 
        userRepo.GetMany().Any(u => u.UserName == userName && u.Id != exceptUserId);

    // Translated by EF into SQL that selects only Id and UserName (never Password).
    private static IQueryable<UserDto> ProjectToDto(IQueryable<User> query) =>
        query.Select(u => new UserDto { Id = u.Id, UserName = u.UserName });

}