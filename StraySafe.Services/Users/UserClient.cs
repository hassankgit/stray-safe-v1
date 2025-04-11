using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using StraySafe.Nucleus.Database;
using StraySafe.Nucleus.Database.Models.Users;
using StraySafe.Services.Users.Models;

namespace StraySafe.Services.Users;
public class UserClient
{
    private readonly UserManager<User> _userManager;
    private readonly JwtService _jwtService;

    public UserClient(UserManager<User> userManager, JwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public static void AddTokenCookieToResponse(HttpResponse response, TokenResponse token)
    {
        response.Cookies.Append("token", token.Token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Path = "/",
            Expires = DateTimeOffset.UtcNow.AddDays(1)
        });
    }

    public async Task<TokenResponse> Login(LoginRequest request)
    {
        if (request.Username == null || request.Password == null)
        {
            throw new ArgumentException("username, email, and password are required");
        }

        User? nameMatch = await _userManager.FindByNameAsync(request.Username);
        if (nameMatch != null)
        {
            return await ValidatePassword(nameMatch, request.Password);
        }
        User? emailMatch = await _userManager.FindByEmailAsync(request.Username);
        if (emailMatch != null)
        {
            return await ValidatePassword(emailMatch, request.Password);
        }

        throw new InvalidOperationException("invalid username or password");
    }

    public async Task<TokenResponse> Register(RegisterRequest request)
    {
        User? usernameMatch = await _userManager.FindByNameAsync(request.Username!);
        if (usernameMatch != null)
        {
            throw new InvalidOperationException("username already exists");
        }

        User? emailMatch = await _userManager.FindByEmailAsync(request.Email!);
        if (emailMatch != null)
        {
            throw new InvalidOperationException("email is already in use");
        }

        User user = new()
        {
            UserName = request.Username,
            Email = request.Email,
        };

        IdentityResult result = await _userManager.CreateAsync(user, request.Password!);
        if (result.Succeeded)
        {
            string token = _jwtService.GenerateToken(user.Id);
            return new TokenResponse
            {
                Token = token
            };
        }

        throw new InvalidOperationException(result.Errors.ToString());
    }

    private async Task<TokenResponse> ValidatePassword(User user, string password)
    {
        bool passwordMatch = await _userManager.CheckPasswordAsync(user, password);
        if (!passwordMatch)
        {
            throw new InvalidOperationException("invalid username or password");
        }
        string token = _jwtService.GenerateToken(user.Id);
        return new TokenResponse
        {
            Token = token
        };
    }
}
