using apiExo.domain.entity;

namespace apiExo.domain.services;

public interface IJWTService {
    public string generate(ApplicationUser user);
}