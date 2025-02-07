namespace apiExo.CQS;

public interface IResult
{
    bool IsSuccess { get; }
    bool IsFailure { get; }

    string? ErrorMessage { get; }
    Exception? Exception { get; }
}