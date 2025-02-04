using apiExo.bll.entity;
using apiExo.dal.database;
using Microsoft.Data.SqlClient;

namespace apiExo.bll.services;

public class UserService(IDataContext context, IHashService hashService, IJWTService jwt)  : IUserService
{
    public UserEntity? GetByEmail(string email)
    {
        using SqlConnection conn = context.CreateConnection();
        string query = "SELECT * FROM Users WHERE Email = @Email";
        using SqlCommand cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@Email", email);

        conn.Open();
        using SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new UserEntity
            {
                Id = (int)reader[nameof(UserEntity.Id)],
                FirstName = (string)reader[nameof(UserEntity.FirstName)],
                LastName = (string)reader[nameof(UserEntity.LastName)],
                Password = (string)reader[nameof(UserEntity.Password)],
                Email = (string)reader[nameof(UserEntity.Email)],
                CreatedAt = (DateTime)reader[nameof(UserEntity.CreatedAt)]
            };
        }
        return null;
    }

    public string Insert(UserEntity user)
    {
        try
        {
            using SqlConnection conn = context.CreateConnection();
            string query = @"
                    INSERT INTO Users (FirstName, LastName, Email, Password) 
                    VALUES (@FirstName, @LastName, @Email, @Password);
                    SELECT SCOPE_IDENTITY();";

            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
            cmd.Parameters.AddWithValue("@LastName", user.LastName);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Password", user.Password); // Consider hashing

            conn.Open();
            int? result = (int)cmd.ExecuteScalar(); // Get the inserted ID
            return result != null ? $"User inserted with ID: {result}" : "User insertion failed.";
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    public string Login(CredentialModel user)
    {
        UserEntity? entity = GetByEmail(user.Email) ?? throw new Exception("Unknown user");

        ApplicationUser applicationUser = new(){
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email =user.Email,
        };

        if (!hashService.VerifyPassword(applicationUser,entity.Password,user.Password)){
            throw new Exception("Invalid credentials");
        }

        return jwt.generate(applicationUser);
    }

    public string Register(RegisterModel model){
        ApplicationUser applicationUser = new(){
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email =model.Email,
        };
        string hashedPassword = hashService.HashPassword(applicationUser,model.Password);
        UserEntity userEntity = new(){
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email =model.Email,
            Password = hashedPassword
        };
        return Insert(userEntity);
    }
}