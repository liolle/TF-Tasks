namespace apiExo.CQS;

public interface ICommandResult : IResult
{
    static CommandResult Success(){
        return new CommandResult(true,"",null);
    }

    static CommandResult Failure(string errorMessage, Exception? exception = null){
        return new CommandResult(false,errorMessage,exception);
    }
}