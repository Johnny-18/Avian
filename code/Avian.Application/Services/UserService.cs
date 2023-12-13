using Avian.Application.Generators;
using Avian.Dal;
using Avian.Dal.Entities;
using Avian.Domain.Users;
using Avian.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Avian.Application.Services;

public interface IUserService
{
    Task<User?> LoginAsync(string email, string password, CancellationToken cancellationToken);
    Task<User?> GetAsync(string email, CancellationToken cancellationToken);
    Task<User[]> GetAllAsync(CancellationToken cancellationToken);
    Task<User?> RegisterAsync(string email, string password, UserTypes type, CancellationToken cancellationToken);
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
    
    public async Task<User?> LoginAsync(string email, string password, CancellationToken cancellationToken)
    {
        var userDal = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        if (userDal is null)
        {
            return null;
        }

        var inputPasswordHash = _hashGenerator.GenerateHash(password);
        if (inputPasswordHash != userDal.PasswordHash)
        {
            return null;
        }
        return new User(userDal.Email, userDal.PasswordHash, userDal.Type);
    }

    public async Task<User?> GetAsync(string email, CancellationToken cancellationToken)
    {
        var userDal = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        if (userDal is null)
        {
            return null;
        }

        return new User(userDal.Email, userDal.PasswordHash, userDal.Type);
    }

    public async Task<User[]> GetAllAsync(CancellationToken cancellationToken)
    {
        var usersDal = await _context.Users.AsNoTracking().ToArrayAsync(cancellationToken);

        return usersDal.Select(x => new User(x.Email, x.PasswordHash, x.Type)).ToArray();
    }

    public async Task<User?> RegisterAsync(string email, string password, UserTypes type, CancellationToken cancellationToken)
    {
        var existUser = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken: cancellationToken);
        if (existUser is not null)
        {
            return null;
        }
        
        var inputPasswordHash = _hashGenerator.GenerateHash(password);
        var userDal = new UserDal
        {
            Email = email,
            PasswordHash = inputPasswordHash,
            Type = type,
        };

        _context.Users.Add(userDal);
        await _context.SaveChangesAsync(cancellationToken);

        return new User(userDal.Email, userDal.PasswordHash, userDal.Type);
    }
}