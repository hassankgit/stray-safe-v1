using AutoMapper;

namespace StraySafe.Logic.Mappers;

public class MappingProfile
{
    public static MapperConfiguration InitializeAutoMapper()
    {
        MapperConfiguration config = new(c =>
        {
            c.AddProfile(new AuthMapper());
            c.AddProfile(new SightingDetailMapper());
        });

        return config;
    }
}
