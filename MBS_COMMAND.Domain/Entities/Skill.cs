using MBS_COMMAND.Domain.Abstractions.Entities;

namespace MBS_COMMAND.Domain.Entities;

public class Skill : Entity<Guid>, IAuditableEntity
{

    public Guid? CategoryId { get; set; }
    public virtual Category? Category { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string? CertificateId { get; set; }
    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }

    public virtual IEnumerable<Certificate>? Certificates { get; set; } = [];

}
