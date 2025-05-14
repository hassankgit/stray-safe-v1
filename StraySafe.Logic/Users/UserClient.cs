using AutoMapper;
using Integration.Supabase.Interfaces;
using Integration.Supabase.Models.Auth;
using StraySafe.Logic.Users.Models;

namespace StraySafe.Logic.Users;
public class UserClient
{
    private readonly ISupabaseService _supabaseService;
    private readonly IMapper _mapper;

    public UserClient(ISupabaseService supabaseService, IMapper mapper)
    {
        _supabaseService = supabaseService;
        _mapper = mapper;
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await _supabaseService.Admin.GetAllUsersAsync();
    }

    public async Task<UserDto> GetCurrentUser()
    {
        User user = await _supabaseService.User.GetCurrentUserAsync();
        UserDto userDto = _mapper.Map<UserDto>(user);
        return userDto;
    }

    public async Task<TokenDto> Login(LoginRequest request)
    {
        TokenResponse response = await _supabaseService.User.Login(request);
        TokenDto dto = _mapper.Map<TokenDto>(response);
        return dto;
    }

    public async Task<TokenDto> Register(RegisterRequest request)
    {
        TokenResponse response = await _supabaseService.User.Register(request);
        TokenDto dto = _mapper.Map<TokenDto>(response);
        return dto;
    }
}
