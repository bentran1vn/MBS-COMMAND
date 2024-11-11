using System.ComponentModel.DataAnnotations;
using MBS_COMMAND.Domain.Abstractions.Entities;

namespace MBS_COMMAND.Domain.Entities;

public class Feedback : Entity<Guid>, IAuditableEntity
{
    [MaxLength(100)]
    public string? Content { get; set; }
    [Range(1,5)]
    public int Rating { get; set; }
    public Guid? ScheduleId { get; set; }
    public virtual Schedule? Schedule { get; set; }
    public bool? IsPresent { get; set; }
    public bool IsMentor { get; set; }
    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }
}