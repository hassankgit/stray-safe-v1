using System.Diagnostics.CodeAnalysis;

namespace StraySafe.Logic.Middleware.Models;

[ExcludeFromCodeCoverage]
public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }
}
