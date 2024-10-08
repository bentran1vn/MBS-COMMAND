using MBS_COMMAND.Contract.Abstractions.Messages;

namespace MBS_COMMAND.Contract.Services.Mentors;

public class DomainEvent
{
    public record MentorCreated(Guid IdEvent, Guid Id, string Email,
        string FullName, int Role, int Points, int Status,
        DateTimeOffset CreatedOnUtc, bool IsDeleted) : IDomainEvent;
}