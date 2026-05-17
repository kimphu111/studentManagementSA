using Microsoft.AspNetCore.Mvc;
using AuthGateway.Data;

namespace AuthGateway.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DbTestController : ControllerBase
{
    private readonly AppDbContext _db;

    public DbTestController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("users")]
    public IActionResult GetUsers()
    {
        var users = _db.Users.ToList();
        return Ok(users);
    }
}