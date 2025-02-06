using apiExo.CQS;

namespace apiExo.domain.Commands;

public class RegisterCommand(string firstName, string lastName, string email, string password) : ICommandDefinition
{
    public string FirstName { get; } = firstName;
    public string LastName { get; } = lastName;
    public string Email { get; } = email;
    public string Password { get; } = password;
}