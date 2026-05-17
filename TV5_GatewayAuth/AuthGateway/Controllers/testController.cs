using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthGateway.Controllers;

[ApiController]
[Route("test")]
public class TestController : ControllerBase
{
    // API này ai cũng vào được (không cần Token)
    [HttpGet("public")]
    public IActionResult Public()
    {
        return Ok("Public OK");
    }

    // API này BẮT BUỘC phải có Token hợp lệ mới vào được
    [Authorize]
    [HttpGet("private")]
    public IActionResult Private()
    {
        var username = User.Identity?.Name;
        return Ok($"HI, {username}, authorized");
    }
}
