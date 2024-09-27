using MBS_AUTHORIZATION.Domain.Entities;
using MBS_COMMAND.Domain.Abstractions.Entities;

namespace MBS_COMMAND.Domain.Entities;

public class Transaction : Entity<Guid>, IAuditableEntity
{
    public Guid? UserId { get; set; }
    public virtual User? User { get; set; }
    public Guid? SlotId { get; set; }
    public virtual Slot? Slot { get; set; }
    public DateOnly Date { get; set; }
    public int Status { get; set; }

    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }
}
