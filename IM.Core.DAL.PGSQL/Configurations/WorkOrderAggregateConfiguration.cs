using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class WorkOrderAggregateConfiguration : WorkOrderAggregateConfigurationBase
    {
        protected override void ConfigureDatabase(EntityTypeBuilder<WorkOrderAggregate> builder)
        {
            builder.ToTable("work_order_aggregate", Options.Scheme);

            builder.Property(x => x.ID).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(x => x.WorkOrderID).HasColumnName("work_order_id");
            builder.Property(x => x.DocumentCount).HasColumnName("document_count");
        }
    }
}