using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using WebAPI.Mappings;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public AuthController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login([FromBody] LoginRequest request)
    {
        User? user = await userRepo.GetMany()
            .SingleOrDefaultAsync(u => u.UserName == request.UserName);
        
        if (user is null)
            return Unauthorized("Incorrect username or password");

        if (user.Password != request.Password)
            return Unauthorized("Incorrect username or password");

        return Ok(user.ToDto());
    }
    
}