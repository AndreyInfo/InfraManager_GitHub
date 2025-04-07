using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    public class CabinetTypeConfiguration:CabinetTypeConfigurationBase
    {
        protected override string NameIndexName => "ux_name";

        protected override string PrimaryKeyName => "pk_cabinet_types";
        protected override void ConfigureDatabase(EntityTypeBuilder<CabinetType> entity)
        {
            entity.ToTable("cabinet_types", Options.Scheme);
            
            entity.Property(x => x.ID).HasColumnName("identificator").HasDefaultValueSql("(nextval('cabinet_type_number'))");

            entity.Property(x => x.Name).HasColumnName("name");

            entity.Property(x => x.ManufacturerID).HasColumnName("manufacturer_id");

            entity.Property(x => x.VerticalSize).HasColumnName("vertical_size");

            entity.Property(x => x.DepthSize).HasColumnName("depth_size")
                .HasColumnType("numeric(9,2)");

            entity.Property(x => x.Image).HasColumnName("image");

            entity.Property(x => x.ProductNumberCyrillic).HasColumnName("cyr_product_number");

            entity.Property(x => x.Category).HasColumnName("category")
                .HasDefaultValueSql("'Шкаф'::character");

            entity.Property(x => x.Code).HasColumnName("code");

            entity.Property(x => x.Note).HasColumnName("note");

            entity.Property(x => x.ProductNumber).HasColumnName("product_number");

            entity.Property(x => x.IMObjID).HasColumnName("im_obj_id").HasDefaultValueSql("(gen_random_uuid())");

            entity.Property(x => x.ComplementaryID).HasColumnName("complementary_id");

            entity.Property(x => x.WidthI).HasColumnName("width_i")
                .HasColumnType("numberic(9,2)");

            entity.Property(x => x.Height).HasColumnName("height")
                .HasColumnType("numeric(9,2)");

            entity.Property(x => x.Width).HasColumnName("width")
                .HasColumnType("numeric(9,2)");

            entity.Property(x => x.NumberingScheme).HasColumnName("numbering_scheme")
                .HasColumnType("smallint");

            entity.HasXminRowVersion(x => x.RowVersion);

            entity.Property(x => x.ExternalID).HasColumnName("external_id");

            entity.Property(x => x.ProductCatalogTypeID).HasColumnName("product_catalog_type_id");
        }
    }
}