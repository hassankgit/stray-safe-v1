using Microsoft.AspNetCore.Mvc;
using StraySafe.Logic.Users;
using StraySafe.Logic.Users.Models;

namespace StraySafe.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserClient _userClient;

    public AuthController(UserClient userClient)
    {
        _userClient = userClient;
    }

    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        TokenResponse token = await _userClient.Login(request);
        Response.Cookies.Append("token", token.Token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddHours(1),
            Path = "/"
        });
        return Ok(token);
    }

    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        TokenResponse token = await _userClient.Register(request);
        Response.Cookies.Append("token", token.Token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddHours(1),
            Path = "/"
        });
        return Ok(token);
    }
}