using StraySafe.Data.Database.Enums;

namespace StraySafe.Data.Database.Models.Sightings;

public class SightingDetail
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Species { get; set; }
    public string? Breed { get; set; }
    public EAnimalAge Age { get; set; }
    public EAnimalSex Sex { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime LastSpotted { get; set; }
    public string? Location { get; set; }
    public SightingTags? Tags { get; set; }
    public string? Notes { get; set; }
    public required string SubmittedById { get; set; }
    public string? SubmittedByName { get; set; }
}
