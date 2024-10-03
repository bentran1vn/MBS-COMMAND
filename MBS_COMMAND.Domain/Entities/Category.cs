using MBS_COMMAND.Domain.Abstractions.Entities;

namespace MBS_COMMAND.Domain.Entities;

public class Category : Entity<Guid>, IAuditableEntity
{
    public string Name { get; set; }
    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }
    public virtual IReadOnlyCollection<Skill> SkillList { get; set; } = default!;
}
