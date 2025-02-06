

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
    /*
    public ICollection<TaskEntity> GetAll(int userId);
    public TaskEntity? GetByID(int id,int userId);
    public string Add(TaskEntity task,int userId);
    public string Patch(TaskPatch task,int userId);
    public string Update(TaskUpdate task,int userId);
    public string Delete(int id,int userId);

    public Task<ICollection<TaskEntity>> GetAllAsync();
    public Task<TaskEntity?> GetByIDAsync(int id);
    public Task<string> AddAsync(TaskEntity task);
    public Task<string> PatchAsync(TaskPatch task);
    public Task<string> UpdateAsync(TaskUpdate task);

    public Task<string> DeleteAsync(int id);
    */

}