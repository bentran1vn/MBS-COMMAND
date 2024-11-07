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
    
    public double? BookingPoints { get; set; }
    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }
    public virtual ICollection<Group_Student_Mapping>? Members { get; set; } = [];


}


