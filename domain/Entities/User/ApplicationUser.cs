namespace apiExo.domain.entity;
public class ApplicationUser 
{
    public int Id {get;}
    public string FirstName {get;set;}
    public string LastName {get;set;}
    public string Email {get;set;}
    public string Password {get;set;}
    public DateTime CreatedAt {get;}

    // Meant to be used on when an user is created.
    internal ApplicationUser(int id, string firstName, string lastName, string email,string password,DateTime createdAt)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        CreatedAt = createdAt;
    }
}