using MBS_COMMAND.Contract.Abstractions.Messages;

namespace MBS_COMMAND.Contract.Services.Groups;

public static class DomainEvents
{
    public record GroupCreated(Guid IdEvent, Guid Id, string Name, Guid? MentorId, string Stacks) : IDomainEvent, ICommand;
}
