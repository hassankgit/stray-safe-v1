using Integration.Supabase.Interfaces;
using Integration.Supabase.Models.Auth;
using Integration.Supabase.Models.Auth.Users;
using StraySafe.Logic.Users.Models;

namespace StraySafe.Logic.Users;
public class UserClient
{
    private readonly ISupabaseService _supabaseService;

    public UserClient(ISupabaseService supabaseService)
    {
        _supabaseService = supabaseService;
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await _supabaseService.Admin.GetAllUsersAsync();
    }

    public async Task<UserDto> GetCurrentUser()
    {
        User user = await _supabaseService.User.GetCurrentUserAsync() ??
            throw new InvalidOperationException("Failed to get current user.");
        UserDto dto = new()
        {
            Id = user.Id,
            Email = user.Email,
            Username = null, // TODO: Add usernames
            Role = user.Role, // TODO: Configure or remove roles as well
            Phone = user.Phone,
        };
        return dto;
    }

    public async Task<TokenDto> Login(LoginRequest request)
    {
        TokenResponse response = await _supabaseService.User.Login(request);
        TokenDto dto = new()
        {
            Token = response.Token,
        };

        return dto;
    }

    public async Task<TokenDto> Register(RegisterRequest request)
    {
        TokenResponse response = await _supabaseService.User.Register(request);
        TokenDto dto = new()
        {
            Token = response.Token,
        };
        return dto;
    }
}
