using InfraManager.DAL.ServiceDesk.WorkOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations
{
    internal class WorkOrderFinancePurchaseConfiguration : InfraManager.DAL.EntityConfigurations.WorkOrderFinancePurchaseConfiguration
    {
        protected override void DatabaseConfigure(EntityTypeBuilder<WorkOrderFinancePurchase> builder)
        {
            builder.ToTable("work_order_finance_purchase", Options.Scheme);
            builder.Property(e => e.WorkOrderID).HasColumnName("work_order_id");
            builder.Property(e => e.Concord).HasColumnName("concord");
            builder.Property(e => e.Bill).HasColumnName("bill");
            builder.Property(e => e.DetailBudget).HasColumnName("detail_budget");
            builder.Property(e => e.UtcDateDelivered).HasColumnType("timestamp(3)").HasColumnName("utc_date_delivered");
            builder.Property(e => e.SupplierID).HasColumnName("supplier_id");
            builder.Property(e => e.ResponsibleID).HasColumnName("responsible_id");
            builder.Property(e => e.ResponsibleClass).HasColumnName("responsible_class_id");
        }

        protected override string SupplierForeignKeyName => "fk_work_order_finance_purchase_supplier";
        protected override string PrimaryKeyName => "pk_work_order_finance_purchase";
    }
}