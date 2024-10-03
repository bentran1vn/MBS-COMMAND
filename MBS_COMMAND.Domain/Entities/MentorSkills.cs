using MBS_AUTHORIZATION.Domain.Entities;
using MBS_COMMAND.Domain.Abstractions.Aggregates;
using MBS_COMMAND.Domain.Abstractions.Entities;

namespace MBS_COMMAND.Domain.Entities;

public class MentorSkills : AggregateRoot<Guid>, IAuditableEntity
{
    public Guid SkillId { get; set; }
    public virtual Skill Skill { get; set; } = default!;
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = default!;
    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }
    public virtual IReadOnlyCollection<Certificate> CertificateList { get; set; } = default!;
}