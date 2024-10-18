using MBS_COMMAND.Domain.Abstractions.Entities;

namespace MBS_COMMAND.Domain.Entities;

public class Schedule : Entity<Guid>, IAuditableEntity
{
    public Guid MentorId { get; set; }
    public virtual User? Mentor { get; set; }
    public Guid SlotId { get; set; }
    public virtual Slot? Slot { get; set; }
    public Guid GroupId { get; set; }
    public virtual Group? Group { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public DateOnly Date { get; set; }
    public Guid SubjectId { get; set; }
    public virtual Subject? Subject { get; set; }
    public DateTimeOffset CreatedOnUtc { get ; set ; }
    public DateTimeOffset? ModifiedOnUtc { get ; set ; }
}
