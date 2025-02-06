using apiExo.domain.entity;

namespace apiExo.domain.services;


public interface IHashService {
    public string HashPassword(string email, string password);
    public bool VerifyPassword(string email, string hashedPassword, string password);
}