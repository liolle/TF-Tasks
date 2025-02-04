using System.Text;
using System.Text.Json;
using apiExo.bll.entity;

namespace apiExo.bll.services;

public class TaskAPIService(IHttpClientFactory clientFactory) : ITaskService
{

    JsonSerializerOptions options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public string Add(TaskEntity task)
    {
        throw new NotImplementedException();
    }

    public async Task<string> AddAsync(TaskEntity task)
    {

        using HttpClient client = clientFactory.CreateClient("MyApiClient");

        using StringContent jsonContent = new(
            JsonSerializer.Serialize(task),
            Encoding.UTF8,
            "application/json"
        );

        using HttpResponseMessage response = await client.PostAsync("task/add",jsonContent);

        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"{jsonResponse}\n");
        return jsonResponse;
    }

    public string Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task<string> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public ICollection<TaskEntity> GetAll()
    {
        
        throw new NotImplementedException();
    }

    public async Task<ICollection<TaskEntity>> GetAllAsync()
    {
        using HttpClient client = clientFactory.CreateClient("MyApiClient");
        using HttpResponseMessage response = await client.GetAsync("task/all");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var tasks = JsonSerializer.Deserialize<ICollection<TaskEntity>>(content,options);

        return tasks ?? new List<TaskEntity>();
    }

    public TaskEntity? GetByID(int id)
    {
        throw new NotImplementedException();
    }

    public Task<TaskEntity?> GetByIDAsync(int id)
    {
        throw new NotImplementedException();
    }

    public string Patch(TaskPatch task)
    {
        throw new NotImplementedException();
    }

    public Task<string> PatchAsync(TaskPatch task)
    {
        throw new NotImplementedException();
    }

    public string Update(TaskUpdate task)
    {
        throw new NotImplementedException();
    }

    public Task<string> UpdateAsync(TaskUpdate task)
    {
        throw new NotImplementedException();
    }

    string ITaskService.Add(TaskEntity task)
    {
        throw new NotImplementedException();
    }
}