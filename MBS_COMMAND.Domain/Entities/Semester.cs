using MBS_COMMAND.Domain.Abstractions.Entities;

namespace MBS_COMMAND.Domain.Entities;

public class Semester : Entity<Guid>, IAuditableEntity
{
    public string Name { get; set; }
    public DateOnly From { get; set; }
    public DateOnly To { get; set; }
    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }
}
