using System.Text.Json;
using Integration.Supabase.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Integration.Supabase;

/// <summary>
/// Supabase's C# NuGet Package does not come out of the box with all of the functionality
/// that they have on their REST API. This service is to fill in the gaps; make manual
/// calls wherever they don't have their own method. Currently, no Supabase C# NuGet Package
/// method is being used.
/// </summary>
public class SupabaseService : ISupabaseService
{
    // TODO: Create an exception handler

    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ISupabaseAdminService Admin { get; }
    public ISupabaseUserService User { get; }

    public SupabaseService(HttpClient httpClient,
                           JsonSerializerOptions jsonOptions,
                           IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _jsonOptions = jsonOptions;
        _httpContextAccessor = httpContextAccessor;

        Admin = new SupabaseAdminService(this);
        User = new SupabaseUserService(this);
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
    public async Task<T> SendPostAsUser<T>(string endpoint, object requestBody)
    {
        var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?["Bearer ".Length..];
        return await Post<T>(endpoint, requestBody, token);
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
    public async Task<T> SendGetAsUser<T>(string endpoint)
    {
        var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?["Bearer ".Length..];
        return await Get<T>(endpoint, token);
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
    public async Task<T> SendGetAsAdmin<T>(string endpoint)
    {
        return await Get<T>(endpoint, null);
    }

    /// <summary>
    ///     Sends a GET request to a Supabase API endpoint. Default to using Service Role Key (Admin access).
    ///     For querying "https://abcdefghijklm.supabase.co/auth/v1/test",
    ///     pass in "auth/v1/test" as the endpoint param.
    /// </summary>
    /// <typeparam name="T">Type to deserialize to</typeparam>
    /// <param name="endpoint">Endpoint to send request</param>
    /// <param name="bearerToken">Specify an optional JWT token to use instead of Service Role Key</param>
    /// <returns>Response of type T</returns>
    /// <exception cref="Exception"></exception>
    /// <exception cref="JsonException"></exception>
    private async Task<T> Get <T>(string endpoint, string? bearerToken = null)
    {
        if (bearerToken != null)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
        }

        // base address will be in the form of https://abcdefghijklm.supabase.co/
        HttpResponseMessage? response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}{endpoint}");
     
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"supabase error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
        }

        string? json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<T>(json, _jsonOptions) ??
            throw new JsonException($"supabase service: failed to deserialize json of type {typeof(T)}");

    }

    /// <summary>
    ///     Sends a POST request to a Supabase API endpoint. Default to using Service Role Key (Admin access).
    ///     For querying "https://abcdefghijklm.supabase.co/auth/v1/test",
    ///     pass in "auth/v1/test" as the endpoint param.
    /// </summary>
    /// <typeparam name="T">Type to deserialize to</typeparam>
    /// <param name="endpoint">Endpoint to send request</param>
    /// <param name="bearerToken">Specify an optional JWT token to use instead of Service Role Key</param>
    /// <returns>Response of type T</returns>
    /// <exception cref="Exception"></exception>
    /// <exception cref="JsonException"></exception>
    private async Task<T> Post<T>(string endpoint, object requestBody, string? token = null)
    {
        if (token != null)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        StringContent? jsonContent = new(
            JsonSerializer.Serialize(requestBody, _jsonOptions),
            System.Text.Encoding.UTF8,
            "application/json"
        );

        // base address will be in the form of https://abcdefghijklm.supabase.co/
        HttpResponseMessage? response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}{endpoint}", jsonContent);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"supabase error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
        }

        string? json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<T>(json, _jsonOptions) ??
            throw new JsonException($"supabase service: failed to deserialize json of type {typeof(T)}");

    }
}
