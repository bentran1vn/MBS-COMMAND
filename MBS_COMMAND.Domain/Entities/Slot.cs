using MBS_COMMAND.Domain.Abstractions.Aggregates;
using MBS_COMMAND.Domain.Abstractions.Entities;
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

    public void ChangeSlotStatusInToBooked(Guid SlotId)
    {
        RaiseDomainEvent(new DomainEvent.ChangeSlotStatusInToBooked(Guid.NewGuid(), SlotId));;
    }
    public void SlotUpdated(Slot slot)
    {
        var slotEvent = new DomainEvent.Slot()
        {
            SlotId = slot.Id,
            MentorId = slot.MentorId,
            StartTime = slot.StartTime,
            EndTime = slot.EndTime,
            Date = slot.Date,
            IsOnline = slot.IsOnline,
            Note = slot.Note,
            Month = slot.Month,
            IsBook = slot.IsBook,
            IsDeleted = slot.IsDeleted,
            CreatedOnUtc = slot.CreatedOnUtc,
            ModifiedOnUtc = slot.ModifiedOnUtc

        };
        RaiseDomainEvent(new DomainEvent.SlotUpdated(Guid.NewGuid(), slotEvent));
    }

}
