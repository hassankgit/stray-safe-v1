using System.Text.Json;
using Integration.Supabase.Interfaces;
using Integration.Supabase.Models.Users;
using Microsoft.AspNetCore.Http;
using SupabaseClient = global::Supabase.Client;

namespace Integration.Supabase;

// Supabase's C# client does not come out of the box with all of the functionality
// that they have on their REST API. This service is to fill in the gaps; make manual
// calls wherever they don't have their own method.

// TODO: Extract user logic to a SupabaseUserRepository
//       Extract admin logic to a SupabaseAdminRepository

public class SupabaseService : ISupabaseService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly SupabaseClient _supabaseClient;            // will be needed for logging in and stuff
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SupabaseService(HttpClient httpClient, JsonSerializerOptions jsonOptions, SupabaseClient supabaseClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _jsonOptions = jsonOptions;
        _supabaseClient = supabaseClient;
        _httpContextAccessor = httpContextAccessor;
    }

    // to move to SupabaseUserRepository
    public async Task<User> GetCurrentUser()
    {
        User user = await SendRequestToUserEndpoint<User>("auth/v1/user") ?? throw new Exception("Failed to find current user");
        return user;
    }

    // to move to SupabaseAdminRepository
    public async Task<List<User>> GetAllUsersAsync()
    {
        SupabaseUserResponse response = await SendRequestToEndpoint<SupabaseUserResponse>("auth/v1/admin/users");
        return response?.Users ?? [];
    }

    /// <summary>
    ///     Sends a request to a Supabase API endpoint as a logged in user. Uses JWT in the Authentication header of request.
    ///     For querying "https://abcdefghijklm.supabase.co/auth/v1/test",
    ///     pass in "auth/v1/test" as the endpoint param.
    /// </summary>
    /// <typeparam name="T">Type to deserialize to</typeparam>
    /// <param name="endpoint">Endpoint to send request</param>
    /// <returns>Response of type T</returns>
    /// <exception cref="Exception"></exception>
    /// <exception cref="JsonException"></exception>
    public async Task<T> SendRequestToUserEndpoint<T>(string endpoint)
    {
        var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Substring("Bearer ".Length);
        return await SendRequestToEndpoint<T>(endpoint, token);
    }

    /// <summary>
    ///     Sends a request to a Supabase API endpoint as an admin. Uses Service Role Key (Admin access).
    ///     For querying "https://abcdefghijklm.supabase.co/auth/v1/test",
    ///     pass in "auth/v1/test" as the endpoint param.
    /// </summary>
    /// <typeparam name="T">Type to deserialize to</typeparam>
    /// <param name="endpoint">Endpoint to send request</param>
    /// <returns>Response of type T</returns>
    /// <exception cref="Exception"></exception>
    /// <exception cref="JsonException"></exception>
    public async Task<T> SendRequestToAdminEndpoint<T>(string endpoint)
    {
        return await SendRequestToEndpoint<T>(endpoint, null);
    }

    /// <summary>
    ///     Sends a request to a Supabase API endpoint. Default to using Service Role Key (Admin access).
    ///     For querying "https://abcdefghijklm.supabase.co/auth/v1/test",
    ///     pass in "auth/v1/test" as the endpoint param.
    /// </summary>
    /// <typeparam name="T">Type to deserialize to</typeparam>
    /// <param name="endpoint">Endpoint to send request</param>
    /// <param name="token">Specify an optional JWT token to use instead of Service Role Key</param>
    /// <returns>Response of type T</returns>
    /// <exception cref="Exception"></exception>
    /// <exception cref="JsonException"></exception>
    private async Task<T> SendRequestToEndpoint <T>(string endpoint, string? token = null)
    {

        if (token != null)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        // base address will be in the form of https://abcdefghijklm.supabase.co/
        HttpResponseMessage? response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}{endpoint}");
     
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Supabase error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
        }

        string? json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<T>(json, _jsonOptions) ??
            throw new JsonException($"Supabase service: failed to deserialize json of type {typeof(T)}");

    }
}
