using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using IM.Core.DAL.Microsoft.SqlServer;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal sealed class AdapterTypeConfiguration : AdapterTypeConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_AlterType";
        protected override string ProductCatalogTypeForeignKeyName => "FK_AdapterType_ProductCatalogType";
        protected override string VendorForeignKeyName => "FK_AdapterType_Производители";
        protected override string SlotForeignKeyName => "FK_AdapterType_SlotType";
        protected override string ProductCatalogTypeIDIndexName => "IX_AdapterType_ProductCatalogType";

        protected override void ConfigureDatabase(EntityTypeBuilder<AdapterType> entity)
        {
            entity.ToTable("AdapterType", Options.Scheme);

            entity.Property(e => e.IMObjID).HasColumnName("AdapterTypeID");
            entity.Property(e => e.ManufacturerID).HasColumnName("VendorID");
            entity.Property(e => e.Name).HasColumnName("Name");
            entity.Property(e => e.Parameters).HasColumnName("Parameters");
            entity.Property(e => e.Note).HasColumnName("Note");
            entity.Property(e => e.ProductNumber).HasColumnName("ProductNumber");
            entity.Property(e => e.ExternalID).HasColumnName("ExternalID").HasDefaultValueSql("(N'Адаптер')");
            entity.Property(e => e.Code).HasColumnName("Code");
            entity.Property(e => e.Removed).HasColumnName("Removed").HasDefaultValueSql("0");
            entity.Property(e => e.ComplementaryID).HasColumnName("ComplementaryID");
            entity.Property(e => e.ProductCatalogTypeID).HasColumnName("ProductCatalogTypeID");
            entity.Property(e => e.RowVersion).HasColumnName("RowVersion").IsRowVersion().IsConcurrencyToken();
            entity.Property(e => e.SlotTypeID).HasColumnName("SlotTypeID");
            entity.Property(e => e.CanBuy).HasColumnName("CanBuy");
        }

    }
}
