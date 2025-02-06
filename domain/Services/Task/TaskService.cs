using apiExo.dal.database;
using apiExo.domain.Commands;
using apiExo.domain.entity;
using apiExo.domain.Queries;
using Microsoft.Data.SqlClient;

namespace apiExo.domain.services;

public partial class TaskService(IDataContext context) : ITaskService
{
    public ICollection<TaskEntity> Execute(AllTaskQuery query)
    {
        var tasks = new List<TaskEntity>();

        using (SqlConnection conn = context.CreateConnection())
        {
            string sql_query = "SELECT Id, Title, Status, CreatedAt FROM Tasks WHERE UserId = @UserId";
            using SqlCommand cmd = new SqlCommand(sql_query, conn);
            cmd.Parameters.AddWithValue("@UserId", query.Id);
            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                tasks.Add(new TaskEntity(
                    (int)reader[nameof(TaskEntity.Id)],
                    (string)reader[nameof(TaskEntity.Title)],
                    (string)reader[nameof(TaskEntity.Status)],
                    (DateTime)reader[nameof(TaskEntity.CreatedAt)]
                ));
          
            }
        }
        return tasks;
    }

    public TaskEntity? Execute(TaskByIdQuery query)
    {
        using SqlConnection conn = context.CreateConnection();
        string sql_query = "SELECT Id, Title, Status, CreatedAt FROM Tasks WHERE Id = @Id AND UserId = @UserId";
        using SqlCommand cmd = new SqlCommand(sql_query, conn);
        cmd.Parameters.AddWithValue("@Id", query.TaskId);
        cmd.Parameters.AddWithValue("@UserId", query.UserId);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new TaskEntity(
                (int)reader[nameof(TaskEntity.Id)],
                (string)reader[nameof(TaskEntity.Title)],
                (string)reader[nameof(TaskEntity.Status)],
                (DateTime)reader[nameof(TaskEntity.CreatedAt)]
            );
        }
        return null;
    }

    public string Execute(AddTaskCommand command)
    {
        using SqlConnection conn = context.CreateConnection();
        string query = "INSERT INTO Tasks (Title, Status, UserId) VALUES (@Title, @Status,@UserId)";
        using SqlCommand cmd = new SqlCommand(query, conn);

        cmd.Parameters.AddWithValue("@Title", command.Title);
        cmd.Parameters.AddWithValue("@Status", command.Status);
        cmd.Parameters.AddWithValue("@UserId", command.UserId);

        conn.Open();
        int rowsAffected = cmd.ExecuteNonQuery();
        return rowsAffected > 0 ? "Task added successfully" : "Failed to add task";
    }

    public string Execute(UpdateTaskCommand command)
    {
        using SqlConnection conn = context.CreateConnection();
        string query = "UPDATE Tasks SET Status = @Status,Title = @Title WHERE Id = @Id AND UserId = @UserId";
        using SqlCommand cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@Status", command.Status);
        cmd.Parameters.AddWithValue("@Title", command.Title);
        cmd.Parameters.AddWithValue("@Id", command.TaskId);
        cmd.Parameters.AddWithValue("@UserId", command.UserId);

        conn.Open();
        int rowsAffected = cmd.ExecuteNonQuery();
        return rowsAffected > 0 ? "Task updated successfully" : "Task not found or update failed";
    }

    public string Execute(PatchTaskCommand command)
    {
        using SqlConnection conn = context.CreateConnection();
        string query = "UPDATE Tasks SET Status = @Status WHERE Id = @Id AND UserId = @UserId";
        using SqlCommand cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@Status", command.Status);
        cmd.Parameters.AddWithValue("@Id", command.TaskId);
        cmd.Parameters.AddWithValue("@UserId", command.UserId);

        conn.Open();
        int rowsAffected = cmd.ExecuteNonQuery();
        return rowsAffected > 0 ? "Task patched successfully" : "Task not found or patch failed";
    }
}