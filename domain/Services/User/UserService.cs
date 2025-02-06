using apiExo.dal.database;
using apiExo.domain.Commands;
using apiExo.domain.entity;
using apiExo.domain.Queries;
using Microsoft.Data.SqlClient;

namespace apiExo.domain.services;

public class UserService(IDataContext context, IHashService hashService, IJWTService jwt) : IUserService
{
    public string Execute(RegisterCommand command)
    {
        using SqlConnection conn = context.CreateConnection();
        string hashedPassword = hashService.HashPassword(command.Email,command.Password);

        string query = @"
                INSERT INTO Users (FirstName, LastName, Email, Password) 
                VALUES (@FirstName, @LastName, @Email, @Password);";

        using SqlCommand cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@FirstName", command.FirstName);
        cmd.Parameters.AddWithValue("@LastName", command.LastName);
        cmd.Parameters.AddWithValue("@Email", command.Email);
        cmd.Parameters.AddWithValue("@Password", hashedPassword); 

        conn.Open();
        int result = cmd.ExecuteNonQuery(); 
        return result > 0 ? $"User inserted succeeded" : "User insertion failed.";
       
    }

    public string Execute(LoginQuery query)
    {

        ApplicationUser user = Execute(new UserFromEmailQuery(query.Email)) ?? throw new Exception("Unknown user");


        if (!hashService.VerifyPassword(query.Email,user.Password,query.Password)){
           throw new Exception("Invalid credentials");
        }

        return jwt.generate(user);
    }

    public ApplicationUser? Execute(UserFromEmailQuery query)
    {
        using SqlConnection conn = context.CreateConnection();
        string sql_query = "SELECT * FROM Users WHERE Email = @Email";
        using SqlCommand cmd = new(sql_query, conn);
        cmd.Parameters.AddWithValue("@Email", query.Email);


        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new ApplicationUser(
                (int)reader[nameof(ApplicationUser.Id)],
                (string)reader[nameof(ApplicationUser.FirstName)],
                (string)reader[nameof(ApplicationUser.LastName)],
                (string)reader[nameof(ApplicationUser.Email)],
                (string)reader[nameof(ApplicationUser.Password)],
                (DateTime)reader[nameof(ApplicationUser.CreatedAt)]
            );
        }
        return null;
    }
}