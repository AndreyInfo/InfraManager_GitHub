using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class WorkOrderAggregateConfiguration : WorkOrderAggregateConfigurationBase
    {
        protected override void ConfigureDatabase(EntityTypeBuilder<WorkOrderAggregate> builder)
        {
            builder.ToTable("WorkOrder_Aggregate", "dbo");

            builder.Property(x => x.ID).HasColumnName("ID");
            builder.Property(x => x.WorkOrderID).HasColumnName("WorkOrderID");
            builder.Property(x => x.DocumentCount).HasColumnName("DocumentCount");
        }
    }
}
