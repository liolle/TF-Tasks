namespace apiExo.CQS;


public interface IQueryHandler<TQuery,TResult> : 
   IQueryDefinition<TResult> where TQuery : IQueryDefinition<TResult>
{
   QueryResult<TResult> Execute(TQuery query);
}