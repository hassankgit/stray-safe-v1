using AutoMapper;
using StraySafe.Logic.Users.Models;

namespace StraySafe.Logic.Mappers;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<User, UserDto>();
    }
}
