using Avian.Domain.ValueObjects;

namespace Avian.Domain.Users;

public sealed class User
{
    public User(string email, string passwordHash, UserTypes role)
    {
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
    }

    public string Email { get; }
    
    public string PasswordHash { get; }
    
    public UserTypes Role { get; }

    public string RoleToString => Role switch
    {
        UserTypes.Regular => "regular",
        UserTypes.Administrator => "administrator",
        _ => throw new ArgumentOutOfRangeException()
    };
}