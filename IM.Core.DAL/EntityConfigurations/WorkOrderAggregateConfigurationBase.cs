using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class WorkOrderAggregateConfigurationBase :
        IEntityTypeConfiguration<WorkOrderAggregate>
    {
        public void Configure(EntityTypeBuilder<WorkOrderAggregate> builder)
        {
            builder.HasKey(x => x.ID);
            builder.HasIndex(x => x.WorkOrderID).IsUnique();
            builder.Property(x => x.DocumentCount).HasDefaultValue(0);
            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<WorkOrderAggregate> builder);
    }
}
