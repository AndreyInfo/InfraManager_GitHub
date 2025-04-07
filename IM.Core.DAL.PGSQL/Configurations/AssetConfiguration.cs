
using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Postgres;

namespace IM.Core.DAL.PGSQL.Configurations
{
    public class AssetConfiguration : AssetConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_asset";
        protected override string UserForeignKeyName => "fk_asset_users";
        protected override string LifeCycleStateForeignKeyName => "fk_asset_life_cycle_state";
        protected override string FixedAssetForeignKeyName => "fk_asset_fixed_asset";
        protected override string StorageLocationForeignKeyName => "fk_asset_storage";
        protected override string SupplierForeignKeyName => "fk_asset_supplier";
        protected override string ServiceCenterForeignKeyName => "fk_asset_service_center";
        protected override string ServiceContractForeignKeyName => "fk_asset_service_contract";
        protected override string FixedAssetIDIndexName => "ix_asset_fixed_asset_id";
        protected override string AssetIDIndexName => "ix_asset_id";
        protected override string LifeCycleStateIDIndexName => "ix_asset_life_cycle_state_id";
        protected override string OwnerIDIndexName => "ix_asset_owner_id";
        protected override string SupplierIDIndexName => "ix_asset_supplier_id";
        protected override string UserIDIndexName => "ix_asset_user_id";
        protected override string UtilizerIDIndexName => "ix_asset_utilizer_id";

        protected override void ConfigureDatabase(EntityTypeBuilder<Asset> builder)
        {
            builder.ToTable("asset", Options.Scheme);
        
            builder.Property(x => x.DeviceID).HasColumnName("device_id");
            builder.Property(x => x.DateReceived).HasColumnName("date_received").HasColumnType("timestamp(3)");
            builder.Property(x => x.Agreement).HasColumnName("agreement");
            builder.Property(x => x.SupplierID).HasColumnName("supplier_id");
            builder.Property(x => x.Warranty).HasColumnName("warranty").HasColumnType("timestamp(3)");
            builder.Property(x => x.ServiceContractID).HasColumnName("service_contract_id");
            builder.Property(x => x.UserID).HasColumnName("user_id");
            builder.Property(x => x.Cost).HasColumnName("cost").HasColumnType("numeric(12, 2)");
            builder.Property(x => x.Founding).HasColumnName("founding");
            builder.Property(x => x.AppointmentDate).HasColumnName("appointment_date").HasColumnType("timestamp(3)");
            builder.Property(x => x.UserField1).HasColumnName("user_field1");
            builder.Property(x => x.UserField2).HasColumnName("user_field2");
            builder.Property(x => x.UserField3).HasColumnName("user_field3");
            builder.Property(x => x.UserField4).HasColumnName("user_field4");
            builder.Property(x => x.UserField5).HasColumnName("user_field5");
            builder.Property(x => x.OnStore).IsRequired().HasColumnName("on_store").HasDefaultValueSql("true");
            builder.Property(x => x.ID).IsRequired().HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            builder.Property(x => x.DateInquiry).HasColumnName("date_inquiry").HasColumnType("timestamp(3)");
            builder.Property(x => x.OwnerID).HasColumnName("owner_id");
            builder.Property(x => x.OwnerClassID).HasColumnName("owner_class_id");
            builder.Property(x => x.UtilizerID).HasColumnName("utilizer_id");
            builder.Property(x => x.UtilizerClassID).HasColumnName("utilizer_class_id");
            builder.Property(x => x.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
            builder.Property(x => x.ComplementaryID).HasColumnName("complementary_id");
            builder.Property(x => x.ComplementaryGuidID).HasColumnName("complementary_guid_id");
            builder.Property(x => x.IsWorking).HasColumnName("is_working");
            builder.Property(x => x.DateAnnuled).HasColumnName("date_annuled").HasColumnType("timestamp(3)");
            builder.Property(x => x.LifeCycleStateID).HasColumnName("life_cycle_state_id");
            builder.Property(x => x.FixedAssetID).HasColumnName("fixed_asset_id");
            builder.Property(x => x.GoodsInvoiceID).HasColumnName("goods_invoice_id");
            builder.Property(x => x.ServiceCenterID).HasColumnName("service_center_id");
            builder.Property(x => x.StorageID).HasColumnName("storage_id");
            builder.HasXminRowVersion(e => e.tstamp);
        }
    }
}