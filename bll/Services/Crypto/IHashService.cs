using apiExo.bll.entity;
namespace apiExo.bll.services;


public interface IHashService {
    public string HashPassword(ApplicationUser user, string password);
    public bool VerifyPassword(ApplicationUser user, string hashedPassword, string password);
}