using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;

namespace StraySafe.Logic.Sightings.Models;

[ExcludeFromCodeCoverage]
public class UploadRequest
{
    public IFormFile? Image { get; set; }
}
