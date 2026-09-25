using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

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
        try
        {
            await VerifyUserNameIsAvailableAsync(request.UserName);
        }
        catch (InvalidOperationException e)
        {
            return Conflict(e.Message);
        }

        User user = new()
        {
            UserName = request.UserName,
            Password = request.Password
        };
        User created = await userRepo.AddAsync(user);
        UserDto dto = new()
        {
            Id = created.Id,
            UserName = created.UserName
        };
        return Created($"/users/{dto.Id}", dto);
    }

    private Task VerifyUserNameIsAvailableAsync(string userName)
    {
        bool taken = userRepo.GetMany().Any(u => u.UserName == userName);
        if (taken)
            throw new InvalidOperationException(
                $"Username '{userName}' is already taken");
        return Task.CompletedTask;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetSingle([FromRoute] int id)
    {
        try
        {
            User user = await userRepo.GetSingleAsync(id);
            UserDto dto = new()
            {
                Id = user.Id,
                UserName = user.UserName
            };
            return Ok(dto);
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UpdateUserDto>> UpdateUser(
        [FromRoute] int id, [FromBody] UpdateUserDto request)
    {
        try
        {
            User user = await userRepo.GetSingleAsync(id);
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
    public async Task<ActionResult<UserDto>> DeleteSingle([FromRoute] int id)
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
}