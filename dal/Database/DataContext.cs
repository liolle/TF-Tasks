using System.Data;
using apiExo.dal.database;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
namespace apiExo.bll.entity;

public class DataContext : IDataContext
{
    private readonly string _connectionString;

    public DataContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("todo")!;
    }

    public SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }

    public int ExecuteNonQuery(string query, SqlParameter[] parameters)
    {
        using (SqlConnection conn = CreateConnection())
        using (SqlCommand cmd = new SqlCommand(query, conn))
        {
            cmd.Parameters.AddRange(parameters);
            conn.Open();
            return cmd.ExecuteNonQuery(); // Returns the number of affected rows
        }
    }

    public DataTable ExecuteQuery(string query, SqlParameter[] parameters)
    {
        using (SqlConnection conn = CreateConnection())
        using (SqlCommand cmd = new SqlCommand(query, conn))
        {
            cmd.Parameters.AddRange(parameters);
            conn.Open();
            
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                DataTable resultTable = new DataTable();
                adapter.Fill(resultTable);
                return resultTable;
            }
        }
    }
}