using MBS_COMMAND.Contract.Abstractions.Messages;

namespace MBS_COMMAND.Contract.Services.Users;

public static class DomainEvent
{
    public record ProductCreated(Guid IdEvent, Guid Id, string Name, decimal Price, string Description) : IDomainEvent, ICommand;

    public record ProductDeleted(Guid IdEvent, Guid Id) : IDomainEvent, ICommand;

    public record ProductUpdated(Guid IdEvent, Guid Id, string Name, decimal Price, string Description) : IDomainEvent, ICommand;
}