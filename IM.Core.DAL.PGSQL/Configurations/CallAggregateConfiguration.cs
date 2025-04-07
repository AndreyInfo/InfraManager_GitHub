using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk.Calls;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class CallAggregateConfiguration : CallAggregateConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_call_ag";

        protected override string CallIDIndexName => "uk_call_ag_call_id";

        protected override void ConfigureProvider(EntityTypeBuilder<CallAggregate> builder)
        {
            builder.ToTable("call_aggregate", Options.Scheme);
            builder.Property(x => x.CallID).HasColumnName("call_id");
            builder.Property(x => x.DocumentCount).HasColumnName("document_count");
            builder.Property(x => x.ID).HasColumnName("id");
            builder.Property(x => x.ProblemCount).HasColumnName("problem_count");
            builder.Property(x => x.QueueName).HasColumnName("queue_name");
            builder.Property(x => x.WorkOrderCount).HasColumnName("work_order_count");
        }
    }
}