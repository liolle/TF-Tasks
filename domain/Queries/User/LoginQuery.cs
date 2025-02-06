using apiExo.CQS;

namespace apiExo.domain.Queries;

public class LoginQuery(string email, string password) : IQueryDefinition<string>
{
    public string Email { get; } = email;
    public string Password { get; } = password;
}