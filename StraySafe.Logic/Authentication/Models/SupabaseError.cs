using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Supabase.Gotrue.Exceptions;
using static Supabase.Gotrue.Exceptions.FailureHint;

namespace StraySafe.Logic.Authentication.Models;

public class SupabaseError
{
    [JsonProperty("code")]
    public int Code { get; set; }
    [JsonProperty("error_code")]
    public string? ErrorCode { get; set; }
    [JsonProperty("msg")]
    public string? Message { get; set; }
    public Reason Reason { get; set; }
}
