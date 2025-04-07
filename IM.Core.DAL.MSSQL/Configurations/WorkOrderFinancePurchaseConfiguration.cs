using InfraManager.DAL.ServiceDesk.WorkOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations
{
    internal class WorkOrderFinancePurchaseConfiguration : InfraManager.DAL.EntityConfigurations.WorkOrderFinancePurchaseConfiguration
    {
        protected override void DatabaseConfigure(EntityTypeBuilder<WorkOrderFinancePurchase> builder)
        {
            builder.ToTable("WorkOrderFinancePurchase", "dbo");
            builder.Property(e => e.WorkOrderID).HasColumnName("WorkOrderID");
            builder.Property(e => e.Concord).HasColumnName("Concord");
            builder.Property(e => e.Bill).HasColumnName("Bill");
            builder.Property(e => e.DetailBudget).HasColumnName("DetailBudget");
            builder.Property(e => e.UtcDateDelivered).HasColumnType("datetime").HasColumnName("UtcDateDelivered");
            builder.Property(e => e.SupplierID).HasColumnName("SupplierID");
            builder.Property(e => e.ResponsibleID).HasColumnName("ResponsibleID");
            builder.Property(e => e.ResponsibleClass).HasColumnName("ResponsibleClassID");
        }

        protected override string SupplierForeignKeyName => "FK_WorkOrderFinancePurchase_Supplier";
        protected override string PrimaryKeyName => "PK_WorkOrderFinancePurchase";
    }
}