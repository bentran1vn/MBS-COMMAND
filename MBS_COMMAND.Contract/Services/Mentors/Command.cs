using MBS_COMMAND.Contract.Abstractions.Messages;

namespace MBS_COMMAND.Contract.Services.Mentors;

public class Command
{
    public record CreateMentorCommand(Guid MentorId) : ICommand;
}