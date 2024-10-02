using MBS_COMMAND.Contract.Abstractions.Messages;

namespace MBS_COMMAND.Contract.Services.Groups;

public static class Command
{
    public record CreateGroupCommand(string Name,Guid MentorId,string Stacks) : ICommand;
}
