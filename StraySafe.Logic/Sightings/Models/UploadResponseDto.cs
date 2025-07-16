using System.Diagnostics.CodeAnalysis;
using StraySafe.Data.Database.Models.Sightings;

namespace StraySafe.Logic.Sightings.Models;

[ExcludeFromCodeCoverage]
public class UploadResponseDto
{
    public string? Url { get; set; }
    public DateTime? DateTime { get; set; }
    public Coordinates? Coordinates { get; set; }
}
