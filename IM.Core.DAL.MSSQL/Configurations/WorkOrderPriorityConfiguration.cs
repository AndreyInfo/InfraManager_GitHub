using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class WorkOrderPriorityConfiguration : WorkOrderPriorityConfigurationBase
    {
        protected override string NameUniqueName => "UI_WorkOrderPriority_Name";

        protected override void ConfigureDatabase(EntityTypeBuilder<WorkOrderPriority> builder)
        {
            builder.ToTable("WorkOrderPriority", "dbo");

            builder.Property(x => x.ID).HasDefaultValueSql("NEWID()").HasColumnName("ID");
            builder.Property(x => x.Name).HasColumnName("Name");
            builder.Property(x => x.RowVersion)
                .IsRowVersion()
                .HasColumnType("timestamp")
                .HasColumnName("RowVersion");
            builder.Property(x => x.Sequence).HasColumnName("Sequence");
            builder.Property(x => x.Color).HasColumnName("Color");
            builder.Property(x => x.Default).HasColumnName("Default");
            builder.Property(x => x.Removed).HasColumnName("Removed");
        }
    }
}
