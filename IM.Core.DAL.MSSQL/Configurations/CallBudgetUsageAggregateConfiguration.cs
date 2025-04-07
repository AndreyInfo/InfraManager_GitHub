using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class CallBudgetUsageAggregateConfiguration : IEntityTypeConfiguration<CallBudgetUsageAggregate>
    {
        public void Configure(EntityTypeBuilder<CallBudgetUsageAggregate> builder)
        {
            builder.ToTable("CallBudgetUsage_Aggregate", "dbo");

            builder.HasKey(e => e.ID);

            builder.Property(x => x.ID)
                .HasColumnName("ID");

            builder.Property(x => x.FullName)
                .IsRequired()
                .HasColumnName("FullName");

            builder.Property(x => x.BudgetID)
                .HasColumnName("BudgetID");

            builder.Property(x => x.BudgetObjectID)
                .HasColumnName("BudgetObjectID");

            builder.Property(x => x.BudgetObjectClassID)
                .HasColumnName("BudgetObjectClassID");
        }
    }
}
