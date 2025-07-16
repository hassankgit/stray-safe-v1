using System.Diagnostics.CodeAnalysis;
using StraySafe.Data.Database.Enums;
using StraySafe.Data.Database.Models.Sightings;

namespace StraySafe.Logic.Sightings.Models;

[ExcludeFromCodeCoverage]
public class CreateSightingRequest
{
    public string? Name { get; set; }
    public string? Species { get; set; }
    public string? Breed { get; set; }
    public DateTime? DateTime { get; set; }
    public required Coordinates Coordinates { get; set; }
    public string? Location { get; set; }
    public string? ImageUrl { get; set; }
    public EAnimalAge Age { get; set; }
    public EAnimalSex Sex { get; set; }
    public EAnimalStatus Status { get; set; }
    public EAnimalBehavior Behavior { get; set; }
    public EAnimalHealth Health { get; set; }
    public string? Notes { get; set; }

}
