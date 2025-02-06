using apiExo.CQS;

namespace apiExo.domain.Commands;


public class PatchTaskCommand( string status, int userId,int taskId) : ICommandDefinition
{
    public string Status { get; set; } = status;
    public int UserId {get;set;} = userId;
    public int TaskId {get;set;} = taskId;
}