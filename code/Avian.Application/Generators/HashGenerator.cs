using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;

namespace Avian.Application.Generators;

public sealed class HashGenerator
{
    private readonly byte[] _salt;
        
    public HashGenerator(IConfiguration configuration)
    {
        var salt = configuration.GetSection("HashSettings")["Salt"] ?? throw new ArgumentException();
        _salt = Encoding.UTF8.GetBytes(salt);
    }
        
    public string GenerateHash(string password)
    {
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: _salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
    }
}