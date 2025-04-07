using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class WorkOrderTypeConfiguration : WorkOrderTypeConfigurationBase
    {
        protected override string UI_Name => "UI_WorkOrderPriority_Name";
        protected override string PrimaryKeyName => "PK_WorkOrderType";

        protected override void ConfigureDatabase(EntityTypeBuilder<WorkOrderType> builder)
        {
            builder.ToTable("WorkOrderType", "dbo");

            builder
                .Property(x => x.RowVersion)
                .IsRowVersion()
                .HasColumnType("timestamp")
                .HasColumnName("RowVersion");

            builder.Property(c => c.Name).HasColumnName("Name");
            builder.Property(c => c.Default).HasColumnName("Default");
            builder.Property(c => c.Removed).HasColumnName("Removed");
            builder.Property(c => c.Color).HasColumnName("Color");
            builder.Property(c => c.TypeClass).HasColumnName("TypeClass");
            builder.Property(c => c.WorkflowSchemeIdentifier).HasColumnName("WorkflowSchemeIdentifier");
            builder.Property(c => c.FormID).HasColumnName("FormId");
            builder.Property(c => c.IconName).HasColumnName("IconName");
        }
    }
}
