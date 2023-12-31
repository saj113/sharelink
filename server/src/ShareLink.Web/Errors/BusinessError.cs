using System.Text.Json.Serialization;

namespace ShareLink.Web.Errors;

public class BusinessError(string code, string message)
{
    public string Type => "business_error";

    /// <summary>
    /// The HTTP status code([RFC7231], Section 6) generated by the origin server for this occurrence of the problem.
    /// </summary>
    [JsonPropertyOrder(-3)]
    public int? Status => StatusCodes.Status400BadRequest;

    /// <summary>
    /// An application-specific error code, expressed as a string value.
    /// </summary>
    [JsonPropertyOrder(-2)]
    public string Code { get; } = code;

    /// <summary>
    /// A human-readable explanation specific to this occurrence of the problem.
    /// </summary>
    [JsonPropertyOrder(-1)]
    public string Message { get; } = message;
}