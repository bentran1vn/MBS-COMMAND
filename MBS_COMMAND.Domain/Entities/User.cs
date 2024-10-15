using MBS_COMMAND.Domain.Abstractions.Aggregates;
using MBS_COMMAND.Domain.Abstractions.Entities;
using MBS_CONTRACT.SHARE.Services.Users;
using static MBS_CONTRACT.SHARE.Services.Mentors.DomainEvent;

namespace MBS_COMMAND.Domain.Entities;

public class User : AggregateRoot<Guid>, IAuditableEntity
{
    public string Email { get; set; }
    public string? FullName { get; set; }
    public string Password { get; set; }
    public int Role { get; set; }
    public int Points { get; set; }
    public int Status { get; set; } //0 Not Active, 1 Active, 2 Blocked
    public Guid? MentorId { get; set; }
    public bool IsFirstLogin { get; set; } = true;
    public virtual User? Mentor { get; set; }
    public virtual ICollection<Group_Student_Mapping>? Groups { get; set; } = [];
    public virtual IReadOnlyCollection<MentorSkills> MentorSkillsList { get; set; } = default!;
    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }

    public void CreateMentor(User user)
    {
        RaiseDomainEvent(new MentorCreated(
            Guid.NewGuid(), user.Id, user.Email,
            user.FullName ?? "", user.Role, user.Points,
            user.Status, user.CreatedOnUtc, user.IsDeleted));
    }

    public void CreateSlot(IEnumerable<Slot> slots, Guid MentorID)
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
            CreatedOnUtc = x.CreatedOnUtc,
            ModifiedOnUtc = x.ModifiedOnUtc
        }).ToList();
        RaiseDomainEvent(new DomainEvent.SlotsCreated(Guid.NewGuid(), slot, MentorID));
    }
}