using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Domain.Abstractions.Entities;
using DomainEventShared = MBS_CONTRACT.SHARE.Abstractions.Messages;

namespace MBS_COMMAND.Domain.Abstractions.Aggregates;

public abstract class AggregateRoot<T> : Entity<T>
{
    private readonly List<DomainEventShared.IDomainEvent> _domainEvents = new();

    public IReadOnlyCollection<DomainEventShared.IDomainEvent> GetDomainEvents() => _domainEvents.ToList();

    public void ClearDomainEvents() => _domainEvents.Clear();

    protected void RaiseDomainEvent(DomainEventShared.IDomainEvent domainEvent) =>
        _domainEvents.Add(domainEvent);
}