using MBS_COMMAND.Domain.Abstractions.Entities;

namespace MBS_COMMAND.Domain.Entities;

public class Feedback : Entity<Guid>, IAuditableEntity
{

    public string? Content { get; set; }
    public int Rating { get; set; }
    public Guid? SlotId { get; set; }
    public virtual Slot? Slot { get; set; }
    public bool IsMentor { get; set; }
    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }
}
