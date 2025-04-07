using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal sealed class PeripheralTypeConfiguration : PeripheralTypeConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_PeripheralType";
        protected override string VendorForeignKeyName => "FK_PeripheralType_Производители";
        protected override string ProductCatalogTypeForeignKeyName => "FK_PeripheralType_ProductCatalogType";
        protected override string ProductCatalogTypeIDIndexName => "IX_PeripheralType_ProductCatalogType";

        protected override void ConfigureDatabase(EntityTypeBuilder<PeripheralType> builder)
        {
            builder.ToTable("PeripheralType", Options.Scheme);

            builder.Property(x => x.IMObjID).HasColumnName("PeripheralTypeID");
            builder.Property(e => e.ManufacturerID).HasColumnName("VendorID");
            builder.Property(x => x.Name).HasColumnName("Name");
            builder.Property(x => x.Parameters).HasColumnName("Parameters");
            builder.Property(x => x.Note).HasColumnName("Note");
            builder.Property(x => x.ProductNumber).HasColumnName("ProductNumber");
            builder.Property(x => x.ExternalID).HasColumnName("ExternalID");
            builder.Property(x => x.Code).HasColumnName("Code");
            builder.Property(x => x.MaxLoad).HasColumnName("MaxLoad").HasColumnType("decimal");
            builder.Property(x => x.NomLoad).HasColumnName("NomLoad").HasColumnType("decimal");
            builder.Property(x => x.CanColorPrint).HasColumnName("CanColorPrint").HasColumnType("tinyint");
            builder.Property(x => x.CanFotoPrint).HasColumnName("CanFotoPrint").HasColumnType("tinyint");
            builder.Property(x => x.Removed).HasColumnName("Removed").HasDefaultValueSql("(0)");
            builder.Property(x => x.ComplementaryID).HasColumnName("ComplementaryID");
            builder.Property(x => x.ProductCatalogTypeID).HasColumnName("ProductCatalogTypeID");
            builder.Property(x => x.RowVersion).HasColumnName("RowVersion").HasColumnType("timestamp").IsConcurrencyToken();
            builder.Property(x => x.CanBuy).HasColumnName("CanBuy");
        }
    }
}
