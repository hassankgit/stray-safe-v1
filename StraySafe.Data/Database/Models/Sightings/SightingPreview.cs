namespace StraySafe.Data.Database.Models.Sightings;

public class SightingPreview
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Species { get; set; }
    public string? Breed { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime LastSpotted { get; set; }
    public required Coordinates Coordinates { get; set; }
    public required string SubmittedById { get; set; }
}
