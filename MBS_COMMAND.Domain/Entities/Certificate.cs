using MBS_COMMAND.Domain.Abstractions.Entities;

namespace MBS_COMMAND.Domain.Entities;

public class Certificate : Entity<Guid>,IAuditableEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public Guid CategoryId { get; set; }
    public virtual Category? Category { get; set; }
    public DateTimeOffset CreatedOnUtc { get ; set ; }
    public DateTimeOffset? ModifiedOnUtc { get ; set ; }
}
