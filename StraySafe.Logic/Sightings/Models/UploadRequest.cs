using Microsoft.AspNetCore.Http;

namespace StraySafe.Logic.Sightings.Models;

public class UploadRequest
{
    public IFormFile? Image { get; set; }
}
