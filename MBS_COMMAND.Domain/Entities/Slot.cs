using MBS_COMMAND.Domain.Abstractions.Aggregates;
using MBS_COMMAND.Domain.Abstractions.Entities;
using MBS_CONTRACT.SHARE.Services.Groups;
using MBS_CONTRACT.SHARE.Services.Slots;

namespace MBS_COMMAND.Domain.Entities;

public class Slot : AggregateRoot<Guid>, IAuditableEntity
{
    public Guid? MentorId { get; set; }
    public virtual User? Mentor { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public DateOnly Date { get; set; }
    public bool IsOnline { get; set; }
    public string? Note { get; set; }
    public short? Month { get; set; }
    public bool IsBook { get; set; }
    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }

    public void CreateSlot(IEnumerable<Slot> slots)
    {
        var slot = slots.Select(x => new DomainEvent.Slot
        {
            MentorId = x.MentorId,
            StartTime = x.StartTime,
            EndTime = x.EndTime,
            Date = x.Date,
            IsOnline = x.IsOnline,
            Note = x.Note,
            Month = x.Month,
            IsBook = x.IsBook,
        }).ToList();
        RaiseDomainEvent(new DomainEvent.SlotsCreated(Guid.NewGuid(), slot));
    }

}
