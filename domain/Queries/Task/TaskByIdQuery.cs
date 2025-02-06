using apiExo.CQS;
using apiExo.domain.entity;

namespace apiExo.domain.Queries;

public class TaskByIdQuery : IQueryDefinition<TaskEntity?>
{
    public int TaskId {get;set;}
    public int UserId {get;set;}
 }