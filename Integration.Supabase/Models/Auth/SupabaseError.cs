using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Integration.Supabase.Models.Auth;

[ExcludeFromCodeCoverage]
public class SupabaseError
{
    [JsonPropertyName("code")]
    public int Code { get; set; }
    [JsonPropertyName("error_code")]
    public string? ErrorCode { get; set; } // TODO Make enum?
    [JsonPropertyName("msg")]
    public string? Message { get; set; }

    [SetsRequiredMembers]
    public SupabaseError(int code, string? errorCode, string? message)
    {
        this.Code = code;
        this.ErrorCode = errorCode;
        this.Message = FirstLetterToUppercase(message);
    }

    // TODO : Determine if this is going to stay excluded from code coverage or if this method is necessary
    public static string FirstLetterToUppercase(string? s)
    {
        if (string.IsNullOrEmpty(s))
            return string.Empty;

        char[] a = s.ToCharArray();
        a[0] = char.ToUpper(a[0]);
        return new string(a);
    }
}
