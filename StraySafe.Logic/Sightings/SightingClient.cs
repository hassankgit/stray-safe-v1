using AutoMapper;
using Integration.Supabase.Interfaces;
using Integration.Supabase.Models.Auth.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using StraySafe.Data.Database;
using StraySafe.Data.Database.Models.Sightings;
using StraySafe.Logic.ImageLogic;
using StraySafe.Logic.Sightings.Models;
using StraySafe.Logic.Utilities;

namespace StraySafe.Logic.Sightings;

public class SightingClient
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly ISupabaseService _supabaseService;
    private readonly ImageMetadataClient _imageMetadataClient;

    public SightingClient(DataContext context,
                          IMapper mapper,
                          ISupabaseService supabaseService,
                          ImageMetadataClient imageMetadataClient)
    {
        _context = context;
        _mapper = mapper;
        _supabaseService = supabaseService;
        _imageMetadataClient = imageMetadataClient;
    }

    public SightingDetailDto? GetSightingDetailById(int id)
    {
        SightingDetail? detail = _context.SightingDetails.Where(x => x.Id == id).FirstOrDefault();
        SightingDetailDto dto = _mapper.Map<SightingDetailDto>(detail);
        return dto;
    }

    public async Task<UploadResponseDto> UploadImage(IFormFile image)
    {
        UploadResponseDto dto = new()
        {
            Url = await _supabaseService.User.UploadImage(image),
            Coordinates = _imageMetadataClient.GetCoordinates(image),
            DateTime = _imageMetadataClient.GetDateTime(image) ?? DateTime.Now,
        };
        return dto;
    }

    public List<SightingPreview> GetSightingPreviewsByCoordinates(Coordinates coordinates)
    {
        if (coordinates.Latitude == null || coordinates.Longitude == null)
        {
            return [];
        }

        // Radius currently sent to 3000 miles TODO: user defined radius?
        MapBoundingBox boundingBox = GetBoundingBox((double)coordinates.Latitude, (double)coordinates.Longitude, 3000);
        List<SightingPreview> sightingPreviewsInRange = _context.SightingPreviews.Where(
                x => x.Coordinates.Latitude <= boundingBox.MaxLat &&
                x.Coordinates.Latitude >= boundingBox.MinLat &&
                x.Coordinates.Longitude <= boundingBox.MaxLng &&
                x.Coordinates.Longitude >= boundingBox.MinLng)
            .ToList();
        return sightingPreviewsInRange;
    }

    private static MapBoundingBox GetBoundingBox(double lat, double lng, double radiusInMiles)
    {
        double latOffset = radiusInMiles / 69.0;
        double lngOffset = radiusInMiles / (69.0 * Math.Cos(lat * Math.PI / 180.0));

        double minLat = lat - latOffset;
        double maxLat = lat + latOffset;
        double minLng = lng - lngOffset;
        double maxLng = lng + lngOffset;

        return new MapBoundingBox()
        {
            MinLat = minLat,
            MaxLat = maxLat,
            MinLng = minLng,
            MaxLng = maxLng,
        };
    }

    public async Task<CreateSightingResponseDto> CreateSighting(CreateSightingRequest request)
    {
        User user = await _supabaseService.User.GetCurrentUserAsync();

        using IDbContextTransaction? transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            SightingDetail detail = new()
            {
                Name = request.Name.NullIfWhiteSpace(),
                Species = request.Species.NullIfWhiteSpace(),
                Breed = request.Breed.NullIfWhiteSpace(),
                Age = request.Age,
                Sex = request.Sex,
                ImageUrl = request.ImageUrl,
                LastSpotted = DateTime.SpecifyKind(request.DateTime ?? DateTime.UtcNow, DateTimeKind.Utc),
                Location = request.Location,
                Tags = new SightingTags()
                {
                    Status = request.Status,
                    Behavior = request.Behavior,
                    Health = request.Health,
                },
                Notes = request.Notes.NullIfWhiteSpace(),
                SubmittedById = user.Id,
                SubmittedByName = user.Email,
            };

            await _context.SightingDetails.AddAsync(detail);
            await _context.SaveChangesAsync();

            SightingPreview preview = new()
            {
                Name = detail.Name,
                Species = detail.Species,
                Breed = detail.Breed,
                ImageUrl = detail.ImageUrl,
                LastSpotted = detail.LastSpotted,
                Coordinates = request.Coordinates,
                SubmittedById = user.Id,
                SightingDetailId = detail.Id,
            };

            await _context.SightingPreviews.AddAsync(preview);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return new CreateSightingResponseDto()
            {
                SightingId = preview.Id,
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception($"Failed to create sighting: {ex.Message}");
        }
    }
}
