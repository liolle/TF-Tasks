using apiExo.CQS;
using apiExo.dal.database;
using apiExo.domain.Commands;
using apiExo.domain.entity;
using apiExo.domain.Queries;
using Microsoft.Data.SqlClient;

namespace apiExo.domain.services;

public class TaskService(IDataContext context) : ITaskService
{
    public QueryResult<ICollection<TaskEntity>> Execute(AllTaskQuery query)
    {
        var tasks = new List<TaskEntity>();

        try
        {
            using SqlConnection conn = context.CreateConnection();
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
        catch (Exception e)
        {
            return IQueryResult<ICollection<TaskEntity>>.Failure(e.Message,e);
        }

        return IQueryResult<ICollection<TaskEntity>>.Success(tasks);
    }

    public QueryResult<TaskEntity?> Execute(TaskByIdQuery query)
    {
        try
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
                TaskEntity task = new(
                    (int)reader[nameof(TaskEntity.Id)],
                    (string)reader[nameof(TaskEntity.Title)],
                    (string)reader[nameof(TaskEntity.Status)],
                    (DateTime)reader[nameof(TaskEntity.CreatedAt)]

                );
                return IQueryResult<TaskEntity?>.Success(task);
            }
            return IQueryResult<TaskEntity?>.Failure("Could not find Corresponding task");
        }
        catch (Exception e) 
        {
            return IQueryResult<TaskEntity?>.Failure("",e);
        }
    }

    public CommandResult Execute(AddTaskCommand command)
    {
        try
        {
            using SqlConnection conn = context.CreateConnection();
            string query = "INSERT INTO Tasks (Title, Status, UserId) VALUES (@Title, @Status,@UserId)";
            using SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@Title", command.Title);
            cmd.Parameters.AddWithValue("@Status", command.Status);
            cmd.Parameters.AddWithValue("@UserId", command.UserId);

            conn.Open();
            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected != 1) {return ICommandResult.Failure("Failed to add task");}
            return ICommandResult.Success();
        }
        catch (Exception e)
        {
            return ICommandResult.Failure("",e);
        }
    }

    public CommandResult Execute(UpdateTaskCommand command)
    {
        try
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
            if (rowsAffected != 1) {return ICommandResult.Failure("Task not found or update failed");}
            return ICommandResult.Success();
        }
        catch (Exception e)
        {
            return ICommandResult.Failure("",e);
        }
    }

    public CommandResult Execute(PatchTaskCommand command)
    {
        try
        {
            using SqlConnection conn = context.CreateConnection();
            string query = "UPDATE Tasks SET Status = @Status WHERE Id = @Id AND UserId = @UserId";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Status", command.Status);
            cmd.Parameters.AddWithValue("@Id", command.TaskId);
            cmd.Parameters.AddWithValue("@UserId", command.UserId);

            conn.Open();
            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected != 1) {return ICommandResult.Failure("Task not found or patch failed");}
            return ICommandResult.Success();
        }
        catch (Exception e)
        {
            return ICommandResult.Failure("",e);
        }
    }
}