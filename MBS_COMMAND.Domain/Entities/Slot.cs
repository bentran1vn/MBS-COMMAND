using MBS_AUTHORIZATION.Domain.Entities;
using MBS_COMMAND.Domain.Abstractions.Entities;

namespace MBS_COMMAND.Domain.Entities;

public class Slot : Entity<Guid>, IAuditableEntity
{
    public Guid? MentorId { get; set; }
    public virtual User? Mentor { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public DateOnly Date { get; set; }
    public int Status { get; set; }
    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }
}
