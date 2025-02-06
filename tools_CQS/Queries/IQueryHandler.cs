namespace apiExo.CQS;


public interface IQueryHandler<TQuery,TResult> : 
   IQueryDefinition<TResult> where TQuery : IQueryDefinition<TResult>
{
   TResult Execute(TQuery query);
}