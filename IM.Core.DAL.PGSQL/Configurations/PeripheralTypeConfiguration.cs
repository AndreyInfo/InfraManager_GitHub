using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal sealed class PeripheralTypeConfiguration : PeripheralTypeConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_peripheral_type";
        protected override string VendorForeignKeyName => "fk_peripheral_type_manufacturers";
        protected override string ProductCatalogTypeForeignKeyName => "fk_peripheral_type_product_catalog_type";
        protected override string ProductCatalogTypeIDIndexName => "ix_peripheral_type_product_catalog_type";


        protected override void ConfigureDatabase(EntityTypeBuilder<PeripheralType> builder)
        {
            builder.ToTable("peripheral_type", Options.Scheme);

            builder.Property(x => x.IMObjID).HasColumnName("peripheral_type_id");

            builder.Property(x => x.ManufacturerID).HasColumnName("vendor_id");

            builder.Property(x => x.Name).HasColumnName("name");

            builder.Property(x => x.Parameters).HasColumnName("parameters");

            builder.Property(x => x.Note).HasColumnName("note");

            builder.Property(x => x.ProductNumber).HasColumnName("product_number");
            
            builder.Property(x => x.ExternalID).HasColumnName("external_id");
            
            builder.Property(x => x.Code).HasColumnName("code");

            builder.Property(x => x.MaxLoad).HasColumnName("max_load");

            builder.Property(x => x.NomLoad).HasColumnName("nom_load");

            builder.Property(x => x.CanColorPrint).HasColumnName("can_color_print");

            builder.Property(x => x.CanFotoPrint).HasColumnName("can_foto_print");

            builder.Property(x => x.Removed).HasColumnName("removed");

            builder.Property(x => x.Removed).HasColumnName("removed").HasDefaultValueSql("false");
            
            builder.Property(x => x.ComplementaryID).HasColumnName("complementary_id");

            builder.Property(x => x.ProductCatalogTypeID).HasColumnName("product_catalog_type_id");

            builder.HasXminRowVersion(x => x.RowVersion);

            builder.Property(x => x.CanBuy).HasColumnName("can_buy");


            builder.IsMarkableForDelete();

           
        }
    }
}