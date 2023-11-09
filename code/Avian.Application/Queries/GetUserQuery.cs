using Avian.Application.Interfaces;
using Avian.Domain.Users;

namespace Avian.Application.Queries;

public sealed class GetUserQuery : IQuery<User?>
{
    public required string Email { get; init; }
    
    public required string Password { get; init; }
}