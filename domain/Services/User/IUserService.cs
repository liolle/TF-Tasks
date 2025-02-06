using apiExo.CQS;
using apiExo.domain.Commands;
using apiExo.domain.entity;
using apiExo.domain.Queries;

namespace apiExo.domain.services;

public interface IUserService : 
    ICommandHandler<RegisterCommand>,
    IQueryHandler<UserFromEmailQuery,ApplicationUser?>,
    IQueryHandler<LoginQuery,string>
{
}