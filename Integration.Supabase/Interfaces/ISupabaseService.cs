namespace Integration.Supabase.Interfaces;

/// <summary>
/// Supabase's C# NuGet Package does not come out of the box with all of the functionality
/// that they have on their REST API. This service is to fill in the gaps; make manual
/// calls wherever they don't have their own method. Currently, no Supabase C# NuGet Package
/// method is being used.
/// </summary>
public interface ISupabaseService
{
    ISupabaseAdminService Admin { get; }
    ISupabaseUserService User { get; }
    Task<T> SendGetAsAdmin<T>(string endpoint);
    Task<T> SendGetAsUser<T>(string endpoint);
    Task<T> SendPostAsUser<T>(string endpoint, object requestBody);
}
