namespace StraySafe.Logic.Models;

public class Coordinates
{
    // use [Required] when working with models that are populated from outside sources, because those are validated later, not at compile time.
    // for internal models, just use required
    // this is in an internal model, so im just using required.
    public double? Latitude { get; set; }

    public double? Longitude { get; set; }
}
