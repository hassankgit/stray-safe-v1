using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SquirrelSite.Models
{
    public class Submission
    {
        // Each submission will contain an image and get an id, latitude,
        // and longitude attached to it before being saved to DB
        public int Id { get; set; }
        [Required]
        public required IFormFile Image { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
