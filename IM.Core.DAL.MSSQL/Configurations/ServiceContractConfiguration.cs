using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class ServiceContractConfiguration : ServiceContractConfigurationBase
{
    protected override string PrimaryKeyName => "PK_ServiceContract";
    protected override string LifeCycleStateForeignKeyName => "FK_LifeCycleStateID";
    protected override string ProductCatalogTypeForeignKeyName => "FK_ProductCatalogTypeID";
    protected override string SupplierForeignKeyName => "FK_ServiceContract_Supplier";
    protected override string WorkOrderForeignKeyName => "FK_ServiceContract_WorkOrder";
    protected override string FinanceCenterForeignKeyName => "FK_ServiceContract_FinanceCenter";
    protected override string ServiceContractModelForeignKeyName => "FK_ServiceContract_ServiceContractModel";

    protected override void ConfigureDatabase(EntityTypeBuilder<ServiceContract> builder)
    {
        builder.ToTable("ServiceContract", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ServiceContractID");
        builder.Property(x => x.ModelID).HasColumnName("ServiceContractModelID");
        builder.Property(x => x.SupplierID).HasColumnName("SupplierID");
        builder.Property(x => x.LifeCycleStateID).HasColumnName("LifeCycleStateID");
        builder.Property(x => x.WorkOrderID).HasColumnName("WorkOrderID");
        builder.Property(x => x.FinanceCenterID).HasColumnName("FinanceCenterID");
        builder.Property(x => x.ServiceContractTypeID).HasColumnName("ServiceContractTypeID");
        builder.Property(x => x.Number).HasColumnName("Number");
        builder.Property(x => x.UtcStartDate).HasColumnName("UtcStartDate").HasColumnType("datetime");
        builder.Property(x => x.UtcFinishDate).HasColumnName("UtcFinishDate").HasColumnType("datetime");
        builder.Property(x => x.Cost).HasColumnName("Cost").HasColumnType("money");
        builder.Property(x => x.Notice).HasColumnName("Notice");
        builder.Property(x => x.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
        builder.Property(x => x.ComplementaryID).HasColumnName("ComplementaryID");
        builder.Property(x => x.TimePeriod).HasColumnName("TimePeriod").HasColumnType("tinyint");
        builder.Property(x => x.GoodsInvoiceID).HasColumnName("GoodsInvoiceID");
        builder.Property(x => x.ManufacturerID).HasColumnName("ManufacturerID");
        builder.Property(x => x.ManagerID).HasColumnName("ManagerID");
        builder.Property(x => x.ManagerClassID).HasColumnName("ManagerClassID");
        builder.Property(x => x.ExternalNumber).HasColumnName("ExternalNumber");
        builder.Property(x => x.Description).HasColumnName("Description");
        builder.Property(x => x.UpdateType).HasColumnName("UpdateType").HasColumnType("tinyint");
        builder.Property(x => x.UpdatePeriod).HasColumnName("UpdatePeriod").HasColumnType("tinyint");
        builder.Property(x => x.NdsType).HasColumnName("NDSType").HasColumnType("tinyint");
        builder.Property(x => x.NdsPercent).HasColumnName("NDSPercent").HasColumnType("tinyint");
        builder.Property(x => x.NdsCustomValue).HasColumnName("NDSCustomValue").HasColumnType("decimal(18, 2)");
        builder.Property(x => x.AddressLicence).HasColumnName("AddressLicence");
        builder.Property(x => x.LoginLicence).HasColumnName("LoginLicence");
        builder.Property(x => x.PasswordLicence).HasColumnName("PasswordLicence");
        builder.Property(x => x.AddressAsset).HasColumnName("AddressAsset");
        builder.Property(x => x.LoginAsset).HasColumnName("LoginAsset");
        builder.Property(x => x.PasswordAsset).HasColumnName("PasswordAsset");
        builder.Property(x => x.UtcDateRegistered).HasColumnName("UtcDateRegistered").HasColumnType("datetime");
        builder.Property(x => x.UtcInitialDateStart).HasColumnName("UtcInitialDateStart").HasColumnType("datetime");
        builder.Property(x => x.UtcInitialDateFinish).HasColumnName("UtcInitialDateFinish").HasColumnType("datetime");
        builder.Property(x => x.InitialCost).HasColumnName("InitialCost").HasColumnType("decimal(18, 2)");
        builder.Property(x => x.UtcDateCreated).HasColumnName("UtcDateCreated").HasColumnType("datetime");
        builder.Property(x => x.InitiatorID).HasColumnName("InitiatorID");
        builder.Property(x => x.InitiatorClassID).HasColumnName("InitiatorClassID");
        builder.Property(x => x.ReInit).HasColumnName("ReInit");
        builder.Property(x => x.ProductCatalogTypeID).HasColumnName("ProductCatalogTypeID");
        builder.Property(x => x.RowVersion)
            .HasColumnName("RowVersion")
            .HasColumnType("timestamp")
            .IsRowVersion();
    }
}