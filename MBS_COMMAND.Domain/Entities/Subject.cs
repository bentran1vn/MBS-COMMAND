using MBS_COMMAND.Domain.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS_COMMAND.Domain.Entities;

public class Subject : Entity<Guid>, IAuditableEntity
{
    public string Name { get ; set ; }
    public int Status { get ; set ; }
    public Guid SemesterId { get; set; }
    public virtual Semester? Semester { get; set; }


    public DateTimeOffset CreatedOnUtc { get ; set ; }
    public DateTimeOffset? ModifiedOnUtc { get ; set ; }
}
