using MBS_COMMAND.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS_COMMAND.Persistence.Configurations;

public class Group_Student_MappingConfiguration : IEntityTypeConfiguration<Group_Student_Mapping>
{
    public void Configure(EntityTypeBuilder<Group_Student_Mapping> builder)
    {
        builder.HasKey(x => new { x.GroupId, x.StudentId });
    }
}
