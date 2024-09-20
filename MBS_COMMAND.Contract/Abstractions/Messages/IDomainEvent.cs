using MassTransit;

namespace MBS_COMMAND.Contract.Abstractions.Messages;

[ExcludeFromTopology]
public interface IDomainEvent
{
    public Guid IdEvent { get; init; }
}