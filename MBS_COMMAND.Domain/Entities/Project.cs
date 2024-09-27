using MBS_COMMAND.Domain.Abstractions.Entities;

namespace MBS_COMMAND.Domain.Entities;

public class Project : Entity<Guid>, IAuditableEntity
{
    public string Name { get ; set ; }
    public string Description { get ; set ; }




    public DateTimeOffset CreatedOnUtc { get ; set ; }
    public DateTimeOffset? ModifiedOnUtc { get ; set ; }
}
