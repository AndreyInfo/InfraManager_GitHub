using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace IM.Core.DAL.PGSQL.Configurations;

internal sealed class ManufacturerConfiguration : ManufacturerConfigurationBase
{
    protected override string UiName => "ui_name";
    protected override string ImObjIDIndexName => "ak_manufacturers_im_obj_id";
    protected override string PrimaryKeyName => "pk_manufacturers";
    protected override string DefaultValueImObjID => "(gen_random_uuid())";
    protected override string DefaultValueID => "nextval('pk_manufacturer_seq'::regclass)";

    protected override void ConfigureDatabase(EntityTypeBuilder<Manufacturer> entity)
    {
        entity.ToTable("manufacturers", Options.Scheme);
       
        entity.Property(e => e.ID).HasColumnName("identificator");
        entity.Property(e => e.Name).HasColumnName("name");
        entity.Property(e => e.IsPanel).HasColumnName("panel");
        entity.Property(e => e.IsCable).HasColumnName("cable");
        entity.Property(e => e.IsRack).HasColumnName("cabinet");
        entity.Property(e => e.IsOutlet).HasColumnName("sockets");
        entity.Property(e => e.ImObjID).HasColumnName("im_obj_id");
        entity.Property(e => e.IsComputer).HasColumnName("computer");
        entity.Property(e => e.IsSoftware).HasColumnName("software");
        entity.Property(e => e.IsMaterials).HasColumnName("materials");
        entity.Property(e => e.IsCableCanal).HasColumnName("channels");
        entity.Property(e => e.ExternalID).HasColumnName("external_id");
        entity.Property(e => e.ComplementaryID).HasColumnName("complementary_id");
        entity.Property(e => e.IsNetworkDevice).HasColumnName("active_equipment");
        entity.Property(e => e.ComplementaryGuidID).HasColumnName("complementary_guid_id");
    }
}
