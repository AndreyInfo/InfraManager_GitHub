using InfraManager.DAL.ServiceDesk.Calls;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class CallAggregateConfigurationBase :
        IEntityTypeConfiguration<CallAggregate>
    {
        public void Configure(EntityTypeBuilder<CallAggregate> builder)
        {
            builder.HasKey(x => x.ID).HasName(PrimaryKeyName);
            builder.HasIndex(x => x.CallID).IsUnique().HasDatabaseName(CallIDIndexName);

            builder.Property(x => x.ID).ValueGeneratedOnAdd();
            builder.Property(x => x.QueueName).HasMaxLength(500);

            ConfigureProvider(builder);
        }

        protected abstract string PrimaryKeyName { get; }
        protected abstract string CallIDIndexName { get; }

        protected abstract void ConfigureProvider(EntityTypeBuilder<CallAggregate> builder);
    }
}
