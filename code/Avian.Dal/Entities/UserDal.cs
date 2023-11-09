namespace Avian.Dal.Entities;

public class UserDal
{
    public Guid Id { get; set; }
    
    public string Email { get; set; } = null!;
    
    public string PasswordHash { get; set; } = null!;
}