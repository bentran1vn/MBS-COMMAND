﻿using MBS_COMMAND.Contract.Abstractions.Messages;

namespace MBS_COMMAND.Contract.Services.Groups;

public static class Command
{
    public record CreateGroupCommand(string Name, Guid SubjectId, string Stacks) : ICommand;   
    public record AddMemberToGroup(Guid GroupId, Guid MemberId) : ICommand;
    public record RemoveMemberFromGroup(Guid GroupId, Guid MemberId) : ICommand;
    public record ChangeLeader(Guid GroupId, Guid NewLeaderId) : ICommand;
    public record UpdateGroup(Guid GroupId, string Name, string Stacks) : ICommand;
    public record AcceptGroupInvitation(Guid GroupId, Guid MemberId) : ICommand;
    public record AddMentorToGroup(Guid GroupId, Guid MentorId) : ICommand;
}
