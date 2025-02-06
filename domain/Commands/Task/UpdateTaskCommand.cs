using apiExo.CQS;

namespace apiExo.domain.Commands;


public class UpdateTaskCommand(string title, string status, int userId,int taskId) : ICommandDefinition
{
    public string Title { get; set; } = title;
    public string Status { get; set; } = status;
    public int UserId {get;set;} = userId;
    public int TaskId {get;set;} = taskId;
}