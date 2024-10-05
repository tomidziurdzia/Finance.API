using Newtonsoft.Json;

namespace FinanceApp.WebApi.Errors;

public class CodeErrorResponse
{
    [JsonProperty(PropertyName = "statusCode")]
    public int StatusCode { get; set; }
    [JsonProperty(PropertyName = "message")]
    public string[]? Message { get; set; }
    public CodeErrorResponse(int statusCode, string[]? message = null)
    {
        StatusCode = statusCode;
        if (message == null)
        {
            Message = new string[0];
            var text = GetDefaultMessageStatusCode(statusCode);
            Message[0] = text;
        }
        else
        {
            Message = message;
        }
    }
    private string GetDefaultMessageStatusCode(int statusCode)
    {
        return statusCode switch
        {
            400 => "The request sent contains errors",
            401 => "You are not authorized for this resource",
            404 => "The requested resource was not found",
            500 => "Server errors occurred",
            _ => string.Empty
        };
    }
}