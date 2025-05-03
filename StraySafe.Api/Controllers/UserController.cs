using Integration.Supabase.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StraySafe.Logic.Users;

namespace StraySafe.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserClient _userClient;

    public UserController(UserClient userClient)
    {
        _userClient = userClient;
    }

    [HttpGet("MyEmail")]
    public async Task<IActionResult> MyName()
    {
        // TODO : Delete, only for testing auth
        User user = await _userClient.GetCurrentUser();
        return Ok(user.Email);
    }
    [HttpGet("All")]
    public async Task<IActionResult> GetAllUsers()
    {
        return Ok(await _userClient.GetAllUsers());
    }
}
