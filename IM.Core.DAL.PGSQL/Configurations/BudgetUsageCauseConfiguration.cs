using IM.Core.DAL.Postgres;
using InfraManager.DAL.Finance;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations
{
    internal class BudgetUsageCauseConfiguration : IEntityTypeConfiguration<BudgetUsageCause>
    {
        public void Configure(EntityTypeBuilder<BudgetUsageCause> builder)
        {
            builder.ToTable("budget_usage_cause", Options.Scheme);
            builder.HasKey(x => new {x.ObjectId, x.ObjectClass});

            builder.Property(x => x.ObjectClass).HasColumnName("object_class_id");
            builder.Property(x => x.Text).HasColumnName("text").HasMaxLength(1000);
            builder.Property(e => e.ObjectId).HasColumnName("object_id");

            builder.Property(x => x.SLAID)
                .HasColumnName("sla_id");

            builder.HasOne(x => x.Sla)
                .WithMany()
                .HasForeignKey(d => d.SLAID)
                .HasConstraintName("fk_budget_usage_cause_sla");
        }
    }
}