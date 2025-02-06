using apiExo.CQS;
using apiExo.domain.entity;

namespace apiExo.domain.Queries;

public class UserFromEmailQuery(string email) : IQueryDefinition<ApplicationUser?>
{
    public string Email { get; } = email;

}