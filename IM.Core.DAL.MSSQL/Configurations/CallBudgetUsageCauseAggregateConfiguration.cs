using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class CallBudgetUsageCauseAggregateConfiguration : IEntityTypeConfiguration<CallBudgetUsageCauseAggregate>
    {
        public void Configure(EntityTypeBuilder<CallBudgetUsageCauseAggregate> builder)
        {
            builder.ToTable("CallBudgetUsageCause_Aggregate", "dbo");

            builder.HasKey(e => e.ID);

            builder.Property(x => x.ID)
                .HasColumnName("ID");

            builder.Property(x => x.SlaID)
                .HasColumnName("SlaID");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(1000)
                .HasColumnName("Name");
        }
    }
}
