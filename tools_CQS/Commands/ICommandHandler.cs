namespace apiExo.CQS ;

// Generic type constraint stating that T must implement the ICommandDefinition interface
public interface ICommandHandler<T> where T : ICommandDefinition
{
    CommandResult Execute(T command);
}