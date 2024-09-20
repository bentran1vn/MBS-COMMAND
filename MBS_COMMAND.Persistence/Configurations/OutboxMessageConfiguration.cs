using MBS_COMMAND.Persistence.Constants;
using MBS_COMMAND.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MBS_COMMAND.Persistence.Configurations;

internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable(TableNames.OutboxMessages);

        builder.HasKey(x => x.Id);
    }
}