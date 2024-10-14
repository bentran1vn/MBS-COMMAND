using MBS_COMMAND.Domain.Abstractions.Entities;

namespace MBS_COMMAND.Domain.Entities;

public class Slot : Entity<Guid>, IAuditableEntity
{
    public Guid? MentorId { get; set; }
    public virtual User? Mentor { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public DateOnly Date { get; set; }
    public bool IsOnline { get; set; }
    public string? Note { get; set; }
    public short? Month { get; set; }
    public bool IsBook { get; set; } = false;
    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }
}
