namespace Avian.Domain.Users;

public sealed class User
{
    public User(string email, string passwordHash, string role)
    {
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
    }

    public string Email { get; }
    
    public string PasswordHash { get; }
    
    public string Role { get; }
}