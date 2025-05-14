using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StraySafe.Data.Database.Models.Sightings;
using StraySafe.Logic.Sightings;
using StraySafe.Logic.Sightings.Models;

namespace StraySafe.Api.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class SightingController : ControllerBase
{
    private readonly SightingClient _sightingClient;

    public SightingController(SightingClient sightingClient)
    {
        _sightingClient = sightingClient;
    }

    [ProducesResponseType(typeof(List<SightingPreview>), StatusCodes.Status200OK)]
    [HttpPost("Previews")]
    public IActionResult GetSightingPreviewsByCoordinates([FromBody] Coordinates coordinates)
    {
        List<SightingPreview> sightingPreviews = _sightingClient.GetSightingPreviewsByCoordinates(coordinates);
        return Ok(sightingPreviews);
    }

    [ProducesResponseType(typeof(SightingDetailDto), StatusCodes.Status200OK)]
    [HttpGet("Detail/{id}")]
    public IActionResult GetSightingDetailById(int id)
    {
        SightingDetailDto? detail = _sightingClient.GetSightingDetailById(id);
        return Ok(detail);
    }
}
