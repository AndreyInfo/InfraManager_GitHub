using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

internal sealed class ServiceContractConfiguration : ServiceContractConfigurationBase
{
    protected override string PrimaryKeyName => "pk_service_contract";

    protected override string LifeCycleStateForeignKeyName => "fk_life_cycle_state_id";
    protected override string ProductCatalogTypeForeignKeyName => "fk_product_catalog_type_id";
    protected override string SupplierForeignKeyName => "fk_service_contract_supplier";
    protected override string WorkOrderForeignKeyName => "fk_service_contract_work_order";
    protected override string FinanceCenterForeignKeyName => "fk_service_contract_finance_center";
    protected override string ServiceContractModelForeignKeyName => "fk_service_contract_service_contract_model";

    protected override void ConfigureDatabase(EntityTypeBuilder<ServiceContract> builder)
    {
        builder.ToTable("service_contract", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("service_contract_id");
        builder.Property(x => x.Notice).HasColumnName("notice");
        builder.Property(x => x.ModelID).HasColumnName("service_contract_model_id");
        builder.Property(x => x.ServiceContractTypeID).HasColumnName("service_contract_type_id");
        builder.Property(x => x.LifeCycleStateID).HasColumnName("life_cycle_state_id");
        builder.Property(x => x.WorkOrderID).HasColumnName("work_order_id");
        builder.Property(x => x.FinanceCenterID).HasColumnName("finance_center_id");
        builder.Property(x => x.SupplierID).HasColumnName("supplier_id");
        builder.Property(x => x.Cost).HasColumnName("cost").HasColumnType("money");
        builder.Property(x => x.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
        builder.Property(x => x.ComplementaryID).HasColumnName("complementary_id");
        builder.Property(x => x.TimePeriod).HasColumnName("time_period");
        builder.Property(x => x.GoodsInvoiceID).HasColumnName("goods_invoice_id");
        builder.Property(x => x.ExternalNumber).HasColumnName("external_number");
        builder.Property(x => x.Description).HasColumnName("description");
        builder.Property(x => x.ManufacturerID).HasColumnName("manufacturer_id");
        builder.Property(x => x.ManagerID).HasColumnName("manager_id");
        builder.Property(x => x.ManagerClassID).HasColumnName("manager_class_id");
        builder.Property(x => x.UpdateType).HasColumnName("update_type");
        builder.Property(x => x.UpdatePeriod).HasColumnName("update_period");
        builder.Property(x => x.NdsType).HasColumnName("nds_type");
        builder.Property(x => x.NdsPercent).HasColumnName("nds_percent");
        builder.Property(x => x.ProductCatalogTypeID).HasColumnName("product_catalog_type_id");
        builder.Property(x => x.AddressLicence).HasColumnName("address_licence");
        builder.Property(x => x.LoginLicence).HasColumnName("login_licence");
        builder.Property(x => x.PasswordLicence).HasColumnName("password_licence");
        builder.Property(x => x.AddressAsset).HasColumnName("address_asset");
        builder.Property(x => x.LoginAsset).HasColumnName("login_asset");
        builder.Property(x => x.PasswordAsset).HasColumnName("password_asset");
        
        builder.Property(x => x.InitiatorID).HasColumnName("initiator_id");
        builder.Property(x => x.InitiatorClassID).HasColumnName("initiator_class_id");
        builder.Property(x => x.ReInit).HasColumnName("re_init");

        builder.Property(x => x.Number).HasColumnName("number").HasDefaultValueSql("nextval('service_contract_number')");
        builder.Property(x => x.UtcStartDate).HasColumnName("utc_start_date").HasColumnType("timestamp(3)");
        builder.Property(x => x.UtcFinishDate).HasColumnName("utc_finish_date").HasColumnType("timestamp(3)");
        builder.Property(x => x.NdsCustomValue).HasColumnName("nds_custom_value").HasColumnType("numeric(18,2)");
        builder.Property(x => x.UtcDateRegistered).HasColumnName("utc_date_registered").HasColumnType("timestamp(3)");
        builder.Property(x => x.UtcInitialDateStart).HasColumnName("utc_initial_date_start").HasColumnType("timestamp(3)");
        builder.Property(x => x.UtcInitialDateFinish).HasColumnName("utc_initial_date_finish").HasColumnType("timestamp(3)");
        builder.Property(x => x.InitialCost).HasColumnName("initial_cost").HasColumnType("numeric(18,2)");
        builder.Property(x => x.UtcDateCreated).HasColumnName("utc_date_created").HasColumnType("timestamp(3)");

        builder.HasXminRowVersion(x => x.RowVersion);

    }
}