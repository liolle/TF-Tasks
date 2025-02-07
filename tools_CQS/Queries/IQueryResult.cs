namespace apiExo.CQS;

public interface IQueryResult<TResult> : IResult
{
    static QueryResult<TResult> Success(TResult result){
        return new QueryResult<TResult>(true,result,"",null);
    }

    static QueryResult<TResult> Failure(string errorMessage, Exception? exception = null){
        return new QueryResult<TResult>(true,default!,errorMessage,exception);
    }
}