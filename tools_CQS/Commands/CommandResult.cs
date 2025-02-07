
namespace apiExo.CQS;


public class CommandResult(bool isSuccess, string errorMessage, Exception? exception) : ICommandResult
{
    public bool IsSuccess { get; } = isSuccess;

    public bool IsFailure { get; } = !isSuccess;

    public string? ErrorMessage { get; } = errorMessage;

    public Exception? Exception { get; } = exception;
}