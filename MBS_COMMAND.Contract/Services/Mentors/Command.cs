using MBS_COMMAND.Contract.Abstractions.Messages;

namespace MBS_COMMAND.Contract.Services.Mentors;

public static class Command
{
    public record CreateMentorCommand(Guid MentorId) : ICommand;
    
    public record MentorAcceptOrDeclineFromGroupCommand(Guid MentorId, bool IsAccepted,Guid GroupId) : ICommand;
}