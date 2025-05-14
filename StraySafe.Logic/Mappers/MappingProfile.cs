using AutoMapper;

namespace StraySafe.Logic.Mappers;

public class MappingProfile
{
    public static MapperConfiguration InitializeAutoMapper()
    {
        MapperConfiguration config = new MapperConfiguration(c =>
        {
            c.AddProfile(new UserMapper());
            c.AddProfile(new SightingDetailMapper());
        });

        return config;
    }
}
