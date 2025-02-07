

using apiExo.CQS;
using apiExo.domain.Commands;
using apiExo.domain.entity;
using apiExo.domain.Queries;

namespace apiExo.domain.services;

public interface ITaskService :
    ICommandHandler<AddTaskCommand>,
    ICommandHandler<UpdateTaskCommand>,
    ICommandHandler<PatchTaskCommand>,
    
    IQueryHandler<AllTaskQuery,ICollection<TaskEntity>>,
    IQueryHandler<TaskByIdQuery,TaskEntity?>
{
}