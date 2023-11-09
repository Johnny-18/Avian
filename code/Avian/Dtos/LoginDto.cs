using System.Text.Json.Serialization;

namespace Avian.Dtos;

public sealed class LoginDto
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = null!;
    
    [JsonPropertyName("password")]
    public string Password { get; set; } = null!;
}