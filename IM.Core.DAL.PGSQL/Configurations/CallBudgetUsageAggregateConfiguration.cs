using IM.Core.DAL.Postgres;
using InfraManager.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations
{
    internal class CallBudgetUsageAggregateConfiguration : IEntityTypeConfiguration<CallBudgetUsageAggregate>
    {
        public void Configure(EntityTypeBuilder<CallBudgetUsageAggregate> builder)
        {
            builder.ToTable("call_budget_usage_aggregate", Options.Scheme);

            builder.HasKey(e => e.ID).HasName("pk_call_budget_usage_ag");

            builder.Property(x => x.ID)
                .HasColumnName("id");

            builder.Property(x => x.FullName)
                .IsRequired()
                .HasColumnName("full_name");

            builder.Property(x => x.BudgetID)
                .HasColumnName("budget_id");

            builder.Property(x => x.BudgetObjectID)
                .HasColumnName("budget_object_id");

            builder.Property(x => x.BudgetObjectClassID)
                .HasColumnName("budget_object_class_id");
        }
    }
}