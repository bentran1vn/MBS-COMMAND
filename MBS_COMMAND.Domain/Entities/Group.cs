using MBS_AUTHORIZATION.Domain.Entities;
using MBS_COMMAND.Domain.Abstractions.Aggregates;
using MBS_COMMAND.Domain.Abstractions.Entities;

namespace MBS_COMMAND.Domain.Entities;

public class Group : AggregateRoot<Guid>, IAuditableEntity
{
    public string Name { get; private set; }
    public Guid? MentorId { get; private set; }
    public virtual User? Mentor { get; set; }

    public Guid? LeaderId { get; set; }
    public virtual User? Leader { get; set; }
    public string Stack { get; private set; }
    public Guid? ProjectId { get; private set; }
    public virtual Project? Project { get; set; }
    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }



    public Group(string name, string stack, Guid? mentorId)
    {
        Name = name;
        Stack = stack;
        MentorId = mentorId;
    }

    public static Group Create(string name, string stack, Guid? mentorId)
    {
        var G = new Group(name, stack, mentorId);
        G.RaiseDomainEvent(new Contract.Services.Groups.DomainEvents.GroupCreated(Guid.NewGuid(), G.Id, G.Name, G.MentorId, G.Stack));
        return G;
    }

}


