using Integration.Supabase.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using StraySafe.Logic.Users;
using StraySafe.Logic.Users.Models;

namespace StraySafe.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserClient _userClient;

    public AuthController(UserClient userClient)
    {
        _userClient = userClient;
    }

    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        TokenDto token = await _userClient.Login(request);
        return Ok(token);
    }

    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        TokenDto token = await _userClient.Register(request);
        return Ok(token);
    }
}