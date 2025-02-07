using apiExo.CQS;
using apiExo.dal.database;
using apiExo.domain.Commands;
using apiExo.domain.entity;
using apiExo.domain.Queries;
using Microsoft.Data.SqlClient;

namespace apiExo.domain.services;

public class UserService(IDataContext context, IHashService hashService, IJWTService jwt) : IUserService
{
    public CommandResult Execute(RegisterCommand command)
    {
        try
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
            if (result != 1) {
                return ICommandResult.Failure("User insertion failed.");
            }
            return ICommandResult.Success();
        }
        catch (Exception e)
        {
            return ICommandResult.Failure("",e);
        }
    }

    public QueryResult<string> Execute(LoginQuery query)
    {
        try
        {
            QueryResult<ApplicationUser?> qr = Execute(new UserFromEmailQuery(query.Email));
            if (qr.IsFailure || qr.Result is null ){
                return IQueryResult<string>.Failure(qr.ErrorMessage!,qr.Exception);
            }

            if (!hashService.VerifyPassword(query.Email,qr.Result.Password,query.Password)){
                return IQueryResult<string>.Failure("Invalid credential combination");
            }

            return IQueryResult<string>.Success(jwt.generate(qr.Result));
        }
        catch (Exception e)
        {
            return IQueryResult<string>.Failure("",e);
        }
    }

    public QueryResult<ApplicationUser?>  Execute(UserFromEmailQuery query)
    {
        try
        {
            using SqlConnection conn = context.CreateConnection();
            string sql_query = "SELECT * FROM Users WHERE Email = @Email";
            using SqlCommand cmd = new(sql_query, conn);
            cmd.Parameters.AddWithValue("@Email", query.Email);

            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                ApplicationUser u = new(

                    (int)reader[nameof(ApplicationUser.Id)],
                    (string)reader[nameof(ApplicationUser.FirstName)],
                    (string)reader[nameof(ApplicationUser.LastName)],
                    (string)reader[nameof(ApplicationUser.Email)],
                    (string)reader[nameof(ApplicationUser.Password)],
                    (DateTime)reader[nameof(ApplicationUser.CreatedAt)]
                );
                return IQueryResult<ApplicationUser?>.Success(u);
            }
            return IQueryResult<ApplicationUser?>.Failure($"Could not find User with email {query.Email}");
        }
        catch (Exception e)
        {
            return IQueryResult<ApplicationUser?>.Failure("",e);
        }

    }
}