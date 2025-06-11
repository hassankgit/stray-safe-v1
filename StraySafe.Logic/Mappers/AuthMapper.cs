using AutoMapper;
using Integration.Supabase.Models.Auth;
using Integration.Supabase.Models.Auth.Users;
using StraySafe.Logic.Users.Models;

namespace StraySafe.Logic.Mappers;

public class AuthMapper : Profile
{
    public AuthMapper()
    {
        CreateMap<User, UserDto>();
        CreateMap<TokenResponse, TokenDto>();
    }
}
