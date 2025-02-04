using apiExo.bll.entity;
using apiExo.dal.database;
using Microsoft.Data.SqlClient;

namespace apiExo.bll.services;

public class TaskService (IDataContext context) : ITaskService
{
public string Add(TaskEntity task)
    {
        using (SqlConnection conn = context.CreateConnection())
        {
            string query = "INSERT INTO Tasks (Title, Status) VALUES (@Title, @Status)";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {

                cmd.Parameters.AddWithValue("@Title", task.Title);
                cmd.Parameters.AddWithValue("@Status", task.Status);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0 ? "Task added successfully" : "Failed to add task";
            }
        }
    }

    public Task<string> AddAsync(TaskEntity task)
    {
        throw new NotImplementedException();
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
        var tasks = new List<TaskEntity>();

        using (SqlConnection conn = context.CreateConnection())
        {
            string query = "SELECT Id, Title, Status, CreatedAt FROM Tasks";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tasks.Add(new TaskEntity
                        {
                            Id = (int) reader[nameof(TaskEntity.Id)],
                            Title = (string) reader[nameof(TaskEntity.Title)],
                            Status = (string) reader[nameof(TaskEntity.Status)],
                            CreatedAt = (DateTime) reader[nameof(TaskEntity.CreatedAt)]
                        });
                    }
                }
            }
        }
        return tasks;
    }

    public Task<ICollection<TaskEntity>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public TaskEntity? GetByID(int id)
    {
        using (SqlConnection conn = context.CreateConnection())
        {
            string query = "SELECT Id, Title, Status, CreatedAt FROM Tasks WHERE Id = @Id";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new TaskEntity
                        {
                            Id = (int) reader[nameof(TaskEntity.Id)],
                            Title = (string) reader[nameof(TaskEntity.Title)],
                            Status = (string) reader[nameof(TaskEntity.Status)],
                            CreatedAt = (DateTime) reader[nameof(TaskEntity.CreatedAt)]
                        };
                    }
                }
            }
        }
        return null;
    }

    public Task<TaskEntity?> GetByIDAsync(int id)
    {
        throw new NotImplementedException();
    }

    public string Patch(TaskPatch p_task)
    {
         using (SqlConnection conn = context.CreateConnection())
        {
            string query = "UPDATE Tasks SET Status = @Status WHERE Id = @Id";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Status", p_task.Status);
                cmd.Parameters.AddWithValue("@Id", p_task.Id);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0 ? "Task patched successfully" : "Task not found or patch failed";
            }
        }
    }

    public Task<string> PatchAsync(TaskPatch task)
    {
        throw new NotImplementedException();
    }

    public string Update(TaskUpdate task)
    {
         using (SqlConnection conn = context.CreateConnection())
        {
            string query = "UPDATE Tasks SET Status = @Status,Title = @Title WHERE Id = @Id";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Status", task.Status);
                cmd.Parameters.AddWithValue("@Title", task.Title);
                cmd.Parameters.AddWithValue("@Id", task.Id);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0 ? "Task updated successfully" : "Task not found or update failed";
            }
        }
    }

    public  Task<string> UpdateAsync(TaskUpdate task)
    {
        throw new NotImplementedException();
    }
}