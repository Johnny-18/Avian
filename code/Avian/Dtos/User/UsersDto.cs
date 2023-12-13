using System.Text.Json.Serialization;

namespace Avian.Dtos.User;

public sealed class UsersDto
{
    [JsonPropertyName("users")]
    public IReadOnlyCollection<UserDto> Users { get; set; } = null!;
}