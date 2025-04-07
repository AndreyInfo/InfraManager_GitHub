using IM.Core.DAL.Postgres;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations
{
    internal class CallBudgetUsageCauseAggregateConfiguration : IEntityTypeConfiguration<CallBudgetUsageCauseAggregate>
    {
        public void Configure(EntityTypeBuilder<CallBudgetUsageCauseAggregate> builder)
        {
            builder.ToTable("call_budget_usage_cause_aggregate", Options.Scheme);

            //builder.HasIndex(e => e.ID, "ix_budget_usage_cause_aggregate_sla_id");

            builder.HasKey(e => e.ID).HasName("pk_call_budget_usage_cause_aggregate");

            builder.Property(x => x.ID)
                .HasColumnName("id");

            builder.Property(x => x.SlaID)
                .HasColumnName("sla_id");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(1000)
                .HasColumnName("name");
        }
    }
}