using StraySafe.Data.Database.Enums;

namespace StraySafe.Data.Database.Models.Sightings;

public class SightingTags
{
    public EAnimalStatus Status { get; set; }
    public EAnimalBehavior Behavior { get; set; }
    public EAnimalHealth Health { get; set; }
}
