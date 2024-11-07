using MBS_COMMAND.Domain.Abstractions.Entities;

namespace MBS_COMMAND.Domain.Entities;

public class Transaction : Entity<Guid>, IAuditableEntity
{
    public Guid? UserId { get; set; }
    public virtual User? User { get; set; }
    public Guid? ScheduleId { get; set; }
    public virtual Schedule? Schedule { get; set; }
    public DateOnly Date { get; set; }
    public int Point { get; set; }
    public int Status { get; set; }

    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }
}
