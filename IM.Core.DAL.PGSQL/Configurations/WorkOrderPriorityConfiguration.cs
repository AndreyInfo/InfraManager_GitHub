using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class WorkOrderPriorityConfiguration : WorkOrderPriorityConfigurationBase
    {
        protected override string NameUniqueName => "ui_work_order_priority_name";
        protected override void ConfigureDatabase(EntityTypeBuilder<WorkOrderPriority> builder)
        {
            builder.ToTable("work_order_priority", Options.Scheme);

            builder.Property(x => x.ID).ValueGeneratedOnAdd().HasColumnName("id");
            builder.Property(x => x.Name).HasColumnName("name");
            builder.HasXminRowVersion(e => e.RowVersion);
            builder.Property(x => x.Sequence).HasColumnName("sequence");
            builder.Property(x => x.Color).HasColumnName("color");
            builder.Property(x => x.Default).HasColumnName("is_default");
            builder.Property(x => x.Removed).HasColumnName("removed");
        }
    }
}