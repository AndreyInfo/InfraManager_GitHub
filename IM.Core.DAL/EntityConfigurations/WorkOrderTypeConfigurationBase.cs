using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class WorkOrderTypeConfigurationBase : IEntityTypeConfiguration<WorkOrderType>
    {
        protected abstract string UI_Name { get; }
        public void Configure(EntityTypeBuilder<WorkOrderType> builder)
        {
            builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(500);
            builder.Property(x => x.IconName).HasMaxLength(200);
            builder.Property(x => x.WorkflowSchemeIdentifier).HasMaxLength(500);
            builder.IsMarkableForDelete();

            builder.HasIndex(x => x.Name).IsUnique().HasDatabaseName(UI_Name);

            builder
                .HasOne(c => c.WorkFlowScheme)
                .WithMany(c => c.WorkOrderTypes)
                .HasForeignKey(c => c.WorkflowSchemeIdentifier)
                .HasPrincipalKey(c => c.Identifier);

            ConfigureDatabase(builder);
        }

        protected abstract string PrimaryKeyName { get; }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<WorkOrderType> builder);
    }
}
