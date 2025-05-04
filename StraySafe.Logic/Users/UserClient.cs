using Integration.Supabase.Interfaces;
using Integration.Supabase.Models.Auth;
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

    public async Task<User> GetCurrentUser()
    {
        return await _supabaseService.User.GetCurrentUserAsync();
    }

    public async Task<TokenDto> Login(LoginRequest request)
    {
        TokenResponse response = await _supabaseService.User.Login(request);
        TokenDto dto = new()
        {
            Token = response.Token
        };
        return dto;
    }

    public async Task<TokenDto> Register(RegisterRequest request)
    {
        TokenResponse response = await _supabaseService.User.Register(request);
        TokenDto dto = new()
        {
            Token = response.Token
        };
        return dto;
    }
}
