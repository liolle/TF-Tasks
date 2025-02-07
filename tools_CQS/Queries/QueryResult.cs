namespace apiExo.CQS;


public class QueryResult<TResult>(bool isSuccess,TResult result, string errorMessage, Exception? exception) : IQueryResult<TResult>
{
    public bool IsSuccess { get; } = isSuccess;

    public bool IsFailure { get; } = !isSuccess;

    public string? ErrorMessage { get; } = errorMessage;

    public TResult Result {
        get {
            if (IsFailure){throw new InvalidOperationException();}
            return result;
        }
    }

    public Exception? Exception { get; } = exception;
}