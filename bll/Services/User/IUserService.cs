using apiExo.bll.entity;

namespace apiExo.bll.services;

public interface IUserService
{
    public string Insert(UserEntity user);
    public UserEntity? GetByEmail(string email);

    public string Login(CredentialModel user);

    public string Register(RegisterModel model);
}