using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult<UserDto>> GetSingle([FromRoute] int id)
    {
        try
        {
            User user = await userRepo.GetSingleAsync(id);
            
            return Ok(user.ToDto());
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetAllUsers([FromQuery] string? userNameContains)
    {
        IQueryable<User> users = userRepo.GetMany();

        if (userNameContains is not null)
        {
            users = users.Where(x => x.UserName.Contains(userNameContains));
        }

        List<UserDto> dtos = users
            .ToList()
            .Select(u => u.ToDto())
            .ToList();
        
        return Ok(dtos);
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
    public async Task<ActionResult> DeleteSingle([FromRoute] int id)
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
    
}