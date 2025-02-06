using apiExo.CQS;
using apiExo.domain.entity;

namespace apiExo.domain.Queries;

public class AllTaskQuery : IQueryDefinition<ICollection<TaskEntity>>
{
    public int Id {get;set;}
}