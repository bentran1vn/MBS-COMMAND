using MBS_COMMAND.Contract.Abstractions.Messages;

namespace MBS_COMMAND.Contract.Services.Groups;

public static class Command
{
    public record CreateGroupCommand(string Name, Guid MentorId, string Stacks) : ICommand;
    public record AddListMemberToGroup(Guid GroupId, List<Guid> MemberId) : ICommand;
    public record RemoveListMemberFromGroup(Guid GroupId, List<Guid> MemberId) : ICommand;
    public record AddMemberToGroup(Guid GroupId, Guid MemberId) : ICommand;
    public record RemoveMemberFromGroup(Guid GroupId, Guid MemberId) : ICommand;
}
