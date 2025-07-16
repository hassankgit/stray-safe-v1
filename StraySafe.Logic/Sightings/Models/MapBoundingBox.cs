using System.Diagnostics.CodeAnalysis;

namespace StraySafe.Logic.Sightings.Models;

[ExcludeFromCodeCoverage]
public class MapBoundingBox
{
    public double MinLat { get; set; }
    public double MaxLat { get; set; }
    public double MinLng { get; set; }
    public double MaxLng { get; set; }
}
