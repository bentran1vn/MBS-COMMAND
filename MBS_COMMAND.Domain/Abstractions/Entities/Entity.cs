namespace MBS_COMMAND.Domain.Abstractions.Entities;

public abstract class Entity<T> : IEntity<T>
{
    public T Id { get; set; }

    public bool IsDeleted { get; protected set; }
}
public abstract class Entity : Entity<Guid>, IEntity<Guid>
{
    protected Entity()
    {
        Id = Guid.NewGuid();
    }
}