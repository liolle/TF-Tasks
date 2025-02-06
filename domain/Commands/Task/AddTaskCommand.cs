using apiExo.CQS;

namespace apiExo.domain.Commands;


public class AddTaskCommand(string title, string status, int userId) : ICommandDefinition
{
    public string Title { get; set; } = title;
    public string Status { get; set; } = status;
    public int UserId {get;set;} = userId;
}