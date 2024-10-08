using MBS_AUTHORIZATION.Domain.Entities;
using MBS_COMMAND.Domain.Abstractions.Aggregates;
using MBS_COMMAND.Domain.Abstractions.Entities;

namespace MBS_COMMAND.Domain.Entities;

public class Group : Entity<Guid>, IAuditableEntity
{
    public string Name { get;  set; }
    public Guid? MentorId { get;  set; }
    public virtual User? Mentor { get; set; }

    public Guid? LeaderId { get; set; }
    public virtual User? Leader { get; set; }
    public string Stack { get;  set; }
    public Guid? ProjectId { get;  set; }
    public virtual Project? Project { get; set; }
    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }
    

}


