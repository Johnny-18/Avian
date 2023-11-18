using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Avian.Dtos.User;

public sealed class LoginDto
{
    [Required]
    [JsonPropertyName("email")]
    public string Email { get; set; } = null!;
    
    [Required]
    [MinLength(6)]
    [JsonPropertyName("password")]
    public string Password { get; set; } = null!;
}