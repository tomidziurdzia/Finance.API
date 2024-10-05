using Newtonsoft.Json;

namespace FinanceApp.WebApi.Errors;

public class CodeErrorException(int statusCode, string[]? message = null, string? details = null)
    : CodeErrorResponse(statusCode, message)
{
    [JsonProperty(PropertyName = "details")]
    public string? Details { get; set; } = details;
}