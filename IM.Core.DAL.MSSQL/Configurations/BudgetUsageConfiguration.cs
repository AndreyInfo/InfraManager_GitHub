using InfraManager.DAL.Finance;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class BudgetUsageConfiguration : IEntityTypeConfiguration<BudgetUsage>
    {
        public void Configure(EntityTypeBuilder<BudgetUsage> builder)
        {
            builder.ToTable("BudgetUsage", "dbo");
            builder.HasKey(x => new { x.ObjectId, x.ObjectClass });

            builder.Property(x => x.ObjectClass).HasColumnName("ObjectClassId");
            builder.Property(x => x.BudgetObjectClass).HasColumnName("BudgetObjectClassId");
        }
    }
}
