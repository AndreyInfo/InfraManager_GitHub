using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations
{
    internal sealed class AdapterTypeConfiguration : AdapterTypeConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_alter_type";
        protected override string VendorForeignKeyName => "fk_adapter_type_manifacturers";
        protected override string ProductCatalogTypeForeignKeyName => "fk_adapter_type_product_catalog_type";
        protected override string SlotForeignKeyName => "fk_adapter_type_slot_type";
        protected override string ProductCatalogTypeIDIndexName => "ix_adapter_type_product_catalog_type";

        protected override void ConfigureDatabase(EntityTypeBuilder<AdapterType> entity)
        {
            entity.ToTable("adapter_type", Options.Scheme);
            
            entity.Property(e => e.IMObjID).HasColumnName("adapter_type_id");
            entity.Property(e => e.ManufacturerID).HasColumnName("vendor_id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Parameters).HasColumnName("parameters");
            entity.Property(e => e.Note).HasColumnName("note");
            entity.Property(e => e.ProductNumber).HasColumnName("product_number");
            entity.Property(e => e.ExternalID).HasColumnName("external_id").HasDefaultValueSql("'Адаптер'");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(x => x.Removed).HasColumnName("removed").HasDefaultValueSql("false");
            entity.Property(e => e.ComplementaryID).HasColumnName("complementary_id");
            entity.Property(e => e.SlotTypeID).HasColumnName("slot_type_id");
            entity.Property(e => e.CanBuy).HasColumnName("can_buy");
            entity.Property(e => e.ProductCatalogTypeID).HasColumnName("product_catalog_type_id");

            entity.HasXminRowVersion(e => e.RowVersion);
        }
    }
}