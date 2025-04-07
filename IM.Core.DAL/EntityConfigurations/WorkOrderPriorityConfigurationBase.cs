using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class WorkOrderPriorityConfigurationBase : IEntityTypeConfiguration<WorkOrderPriority>
    {
        protected abstract string NameUniqueName { get; }
    
        public void Configure(EntityTypeBuilder<WorkOrderPriority> builder)
        {
            builder.HasKey(x => x.ID);

            builder.Property(x => x.Name).HasMaxLength(250).IsRequired();
            builder.Property(x => x.Sequence).IsRequired();
            builder.Property(x => x.Color).IsRequired();
            builder.Property(x => x.Default).IsRequired();
            builder.Property(x => x.Removed).IsRequired();
            builder.IsMarkableForDelete();

            builder.HasIndex(x => x.Name).IsUnique().HasDatabaseName(NameUniqueName);
            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<WorkOrderPriority> builder);
    }
}
