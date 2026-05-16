using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using AuthGateway.Helpers;
using AuthGateway.Models;
using AuthGateway.Data;
using AuthGateway.DTOs;
using AuthGateway.Services;

namespace AuthGateway.Controllers;

//auth/login
//auth/register
[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly JwtService _jwtService;

    public AuthController(AppDbContext db, JwtService jwtService)
    {
        _db = db;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (string.IsNullOrEmpty(dto.Username) || string.IsNullOrEmpty(dto.Password))
        {
            return BadRequest("Username and Password are required.");
        }

        var userExists = await _db.Users.AnyAsync(x => x.Username == dto.Username);
        if (userExists)
        {
            return BadRequest("Username already taken!");
        }

        var user = new User()
        {
            Username = dto.Username,
            Password = PasswordHelper.HashPassword(dto.Password)
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return Ok("Register success");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x => x.Username == dto.Username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
        {
            return Unauthorized("Invalid credentials!");
        }

        var token = _jwtService.GenerateToken(user);

        return Ok(new
        {
            token = token,
            message = "Login success"
        });

    }
}