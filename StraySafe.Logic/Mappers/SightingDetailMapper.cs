using AutoMapper;
using StraySafe.Data.Database.Models.Sightings;
using StraySafe.Data.Utilities;
using StraySafe.Logic.Sightings.Models;

namespace StraySafe.Logic.Mappers;


#pragma warning disable CS8602 // Dereference of a possibly null reference. 
                               // Automapper handles null reference exceptions already.
public class SightingDetailMapper : Profile
{
    public SightingDetailMapper()
    {
        CreateMap<SightingDetail, SightingDetailDto>()
            .ForMember(x => x.Age, y => y.MapFrom(s => s.Age.ToLabel()))
            .ForMember(x => x.Sex, y => y.MapFrom(s => s.Sex.ToLabel()))
            .ForMember(x => x.Tags, y => y.MapFrom(s => new List<string?>()
            {
                s.Tags.Status.ToLabel() ?? "unknown",
                s.Tags.Behavior.ToLabel() ?? "unknown",
                s.Tags.Health.ToLabel() ?? "unknown"
            }));
    }
}
