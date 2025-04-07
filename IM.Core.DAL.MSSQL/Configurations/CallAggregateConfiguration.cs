using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk.Calls;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class CallAggregateConfiguration : CallAggregateConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_CallAG";

        protected override string CallIDIndexName => "UK_CallAG_CallID";

        protected override void ConfigureProvider(EntityTypeBuilder<CallAggregate> builder)
        {
            builder.ToTable("Call_Aggregate", "dbo");
            builder.Property(x => x.CallID).HasColumnName("CallID");
            builder.Property(x => x.DocumentCount).HasColumnName("DocumentCount");
            builder.Property(x => x.ID).HasColumnName("ID");
            builder.Property(x => x.ProblemCount).HasColumnName("ProblemCount");
            builder.Property(x => x.QueueName).HasColumnName("QueueName");
            builder.Property(x => x.WorkOrderCount).HasColumnName("WorkOrderCount");
        }
    }
}
