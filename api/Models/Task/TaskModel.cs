namespace apiExo.api.Models;

public class AddTaskModel 
{
    public required  string Title {get;set;}
    public required  string Status {get;set;}
}


public class UpdateTaskModel 
{
    public required int Id {get;set;}
    public required string Title {get;set;}
    public required string Status {get;set;}
}

public class PatchTaskModel 
{
    public required int Id {get;set;}
    public required string Status {get;set;}
}
