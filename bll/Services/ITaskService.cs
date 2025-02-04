
using apiExo.bll.entity;

namespace apiExo.bll.services;

public interface ITaskService {
    public ICollection<TaskEntity> GetAll();
    public TaskEntity? GetByID(int id);
    public string Add(TaskEntity task);
    public string Patch(TaskPatch task);
    public string Update(TaskUpdate task);
    public string Delete(int id);

    public Task<ICollection<TaskEntity>> GetAllAsync();
    public Task<TaskEntity?> GetByIDAsync(int id);
    public Task<string> AddAsync(TaskEntity task);
    public Task<string> PatchAsync(TaskPatch task);
    public Task<string> UpdateAsync(TaskUpdate task);

    public Task<string> DeleteAsync(int id);

}