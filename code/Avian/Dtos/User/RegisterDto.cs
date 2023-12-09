using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Avian.Domain.ValueObjects;

namespace Avian.Dtos.User;

public sealed class RegisterDto
{
    [Required]
    [JsonPropertyName("email")]
    public string Email { get; set; } = null!;
    
    [Required]
    [MinLength(6)]
    [JsonPropertyName("password")]
    public string Password { get; set; } = null!;
    
    [Required]
    [JsonPropertyName("role")]
    public UserTypes Role { get; set; }
}