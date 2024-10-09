using MBS_COMMAND.Contract.Abstractions.Messages;

namespace MBS_COMMAND.Contract.Services.Mentors;

public class Command
{
    public record CreateMentorCommand(string Name, decimal Price, string Description) : ICommand;
}