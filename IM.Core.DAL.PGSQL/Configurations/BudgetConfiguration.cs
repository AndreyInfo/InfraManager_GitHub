using InfraManager.DAL.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations
{
    internal class BudgetConfiguration : IEntityTypeConfiguration<Budget>
    {
        public void Configure(EntityTypeBuilder<Budget> entity)
        {
            entity.ToTable("budget", Options.Scheme);
            entity.HasKey(x => x.ID); //  .HasConstraintName("pk_budget");

            entity.Property(x => x.ID).HasColumnName("id");
            entity.Property(x => x.Code).HasColumnName("code").HasMaxLength(10);
            entity.Property(x => x.ExternalID).HasColumnName("external_id").HasMaxLength(250).IsRequired();
            entity.Property(x => x.Name).HasColumnName("name").HasMaxLength(250).IsRequired();
            entity.Property(x => x.Removed).HasColumnName("removed").IsRequired();
            entity.Ignore(x => x.RowVersion);

            entity.HasOne(x => x.Parent).WithMany()
                .HasForeignKey("parent_budget_id")
                .HasConstraintName("fk_budget_budget");
        }
    }
}