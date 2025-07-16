using System.Diagnostics.CodeAnalysis;

namespace StraySafe.Data.Database.Models.Sightings;

[ExcludeFromCodeCoverage]
public class Coordinates
{
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}
