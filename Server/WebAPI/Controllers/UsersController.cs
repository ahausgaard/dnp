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
}