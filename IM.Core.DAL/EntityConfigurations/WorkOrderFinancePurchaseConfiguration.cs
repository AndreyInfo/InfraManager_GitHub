using InfraManager.DAL.ServiceDesk.WorkOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class WorkOrderFinancePurchaseConfiguration: IEntityTypeConfiguration<WorkOrderFinancePurchase>
    {
        public void Configure(EntityTypeBuilder<WorkOrderFinancePurchase> builder)
        {
            builder.HasKey(x => x.WorkOrderID).HasName(PrimaryKeyName);
            builder.Property(x => x.WorkOrderID).ValueGeneratedNever();
            builder.HasOne(x => x.Supplier)
                .WithMany()
                .HasForeignKey(x => x.SupplierID)
                .HasConstraintName(SupplierForeignKeyName);

            builder.Property(e => e.Concord).HasMaxLength(250).IsRequired();
            builder.Property(e => e.Bill).HasMaxLength(250).IsRequired();
            builder.Property(e => e.DetailBudget).HasMaxLength(250).IsRequired();

            DatabaseConfigure(builder);
        }

        protected abstract string SupplierForeignKeyName { get; }
        protected abstract string PrimaryKeyName { get; }

        protected abstract void DatabaseConfigure(EntityTypeBuilder<WorkOrderFinancePurchase> builder);
    }
}