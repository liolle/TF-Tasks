using apiExo.bll.entity;
namespace apiExo.bll.services;

public interface IJWTService {
    public string generate(ApplicationUser user);
}