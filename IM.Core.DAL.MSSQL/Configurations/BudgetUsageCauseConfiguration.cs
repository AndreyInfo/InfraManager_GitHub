using InfraManager.DAL.Finance;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class BudgetUsageCauseConfiguration : IEntityTypeConfiguration<BudgetUsageCause>
    {
        public void Configure(EntityTypeBuilder<BudgetUsageCause> builder)
        {
            builder.ToTable("BudgetUsageCause", "dbo");
            builder.HasKey(x => new { x.ObjectId, x.ObjectClass });

            builder.Property(x => x.ObjectClass)
                .HasColumnName("ObjectClassId");

            builder.Property(x => x.Text)
                .HasMaxLength(1000)
                .HasColumnName("Text");

            builder.Property(x => x.SLAID)
                .HasColumnName("SLAID");

            builder.HasOne(x => x.Sla)
                .WithMany()
                .HasForeignKey("SlaID")
                .HasConstraintName("FK_BudgetUsageCause_SLA");

        }
    }
}
