using IM.Core.DAL.Postgres;
using InfraManager.DAL.Finance;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations
{
    internal class BudgetUsageConfiguration : IEntityTypeConfiguration<BudgetUsage>
    {
        public void Configure(EntityTypeBuilder<BudgetUsage> builder)
        {
            builder.ToTable("budget_usage", Options.Scheme);
            builder.HasKey(x => new {x.ObjectId, x.ObjectClass});

            builder.Property(e => e.BudgetId).HasColumnName("budget_id");
            builder.Property(e => e.BudgetObjectClass).HasColumnName("budget_object_class_id");
            builder.Property(e => e.BudgetObjectId).HasColumnName("budget_object_id");
            builder.Property(e => e.ObjectClass).HasColumnName("object_class_id");
            builder.Property(e => e.ObjectId).HasColumnName("object_id");
        }
    }
}