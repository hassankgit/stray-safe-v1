namespace StraySafe.Logic.Sightings.Models;

public class SightingDetailDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Species { get; set; }
    public string? Breed { get; set; }
    public string? Age { get; set; }
    public string? Sex { get; set; }
    public List<string>? Tags { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime LastSpotted { get; set; }
    public string? Location { get; set; }
    public string? Notes { get; set; }
    public required string SubmittedById { get; set; }
    public string? SubmittedByName { get; set; }

}
