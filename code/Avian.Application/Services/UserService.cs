using Avian.Application.Generators;
using Avian.Dal;
using Avian.Domain.Users;
using Avian.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Avian.Application.Services;

public interface IUserService
{
    Task<User?> GetUser(string email, string password, CancellationToken cancellationToken);
}

public sealed class UserService : IUserService
{
    private readonly AvianContext _context;
    private readonly HashGenerator _hashGenerator;

    public UserService(AvianContext context, HashGenerator hashGenerator)
    {
        _context = context;
        _hashGenerator = hashGenerator;
    }
    
    public async Task<User?> GetUser(string email, string password, CancellationToken cancellationToken)
    {
        var userFromDal = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        if (userFromDal is null)
        {
            return null;
        }

        var inputPasswordHash = _hashGenerator.GenerateHash(password);
        if (inputPasswordHash != userFromDal.PasswordHash)
        {
            return null;
        }

        var role = userFromDal.Type switch
        {
            UserTypes.Regular => "regular",
            UserTypes.Administrator => "administrator",
            _ => throw new ArgumentOutOfRangeException()
        };

        return new User(userFromDal.Email, userFromDal.PasswordHash, role);
    }
}