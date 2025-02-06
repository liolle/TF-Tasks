using apiExo.domain.entity;
using Microsoft.AspNetCore.Identity;
namespace apiExo.domain.services;


public class HashService(IPasswordHasher<string> passwordHasher) : IHashService
{
    private readonly IPasswordHasher<string> _passwordHasher = passwordHasher;

    public string HashPassword(string email, string password)
    {
        return _passwordHasher.HashPassword(email, password);
    }

    public bool VerifyPassword(string email, string hashedPassword, string password){
        var result = _passwordHasher.VerifyHashedPassword(email, hashedPassword, password);
        return result == PasswordVerificationResult.Success;
    }
}