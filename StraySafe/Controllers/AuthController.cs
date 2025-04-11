using Microsoft.AspNetCore.Mvc;
using StraySafe.Services.Users;
using StraySafe.Services.Users.Models;

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
        UserClient.AddTokenCookieToResponse(Response, token);
        return Ok(token);
    }

    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        TokenResponse token = await _userClient.Register(request);
        UserClient.AddTokenCookieToResponse(Response, token);
        return Ok(token);
    }
}