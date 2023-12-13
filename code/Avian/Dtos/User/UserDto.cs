using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Avian.Domain.ValueObjects;

namespace Avian.Dtos.User;

public class UserDto
{
    [Required]
    [JsonPropertyName("email")]
    public string Email { get; set; } = null!;
    
    [Required]
    [JsonPropertyName("role")]
    public UserTypes Role { get; set; }

    public static UserDto FromDomain(Domain.Users.User domain)
    {
        return new UserDto
        {
            Email = domain.Email,
            Role = domain.Role,
        };
    }
}