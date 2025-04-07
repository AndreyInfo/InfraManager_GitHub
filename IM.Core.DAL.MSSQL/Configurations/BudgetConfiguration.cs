using InfraManager.DAL.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class BudgetConfiguration : IEntityTypeConfiguration<Budget>
    {
        public void Configure(EntityTypeBuilder<Budget> builder)
        {
            builder.ToTable("Budget", "dbo");
            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID).HasColumnName("ID");
            builder.Property(x => x.Code).HasColumnName("Code").HasMaxLength(10);
            builder.Property(x => x.ExternalID).HasColumnName("ExternalID").HasMaxLength(250).IsRequired();
            builder.Property(x => x.Name).HasColumnName("Name").HasMaxLength(250).IsRequired();
            builder.Property(x => x.Removed).HasColumnName("Removed").IsRequired();
            builder.Property(x => x.RowVersion).HasColumnName("RowVersion").IsRowVersion();

            builder.HasOne(x => x.Parent).WithMany()
                .HasForeignKey("ParentBudgetID")
                .HasConstraintName("FK_Budget_Budget");
        }
    }
}
