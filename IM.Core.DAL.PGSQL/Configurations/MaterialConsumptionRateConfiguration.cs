using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace IM.Core.DAL.PGSQL.Configurations;

internal sealed class MaterialConsumptionRateConfiguration : MaterialConsumptionRateConfigurationBase
{
    protected override string PrimaryKeyName => "pk_material_consumption_rate";

    protected override string MaterialModelForeignKeyName => "fk_material_consumption_rate_material_model";

    protected override string IndexByMaterialModelID => "ix_material_consumption_rate_device_model_id";

    protected override string IndexByDeviceCategoryID => "ix_material_consumption_rate_material_model_id";

    protected override void ConfigureDataBase(EntityTypeBuilder<MaterialConsumptionRate> entity)
    {
        entity.ToTable("material_consumption_rate", Options.Scheme);

        entity.Property(e => e.ID).HasColumnName("mcr_id");
        entity.Property(e => e.DeviceModelID).HasColumnName("device_model_id");
        entity.Property(e => e.DeviceCategoryID).HasColumnName("device_category_id");
        entity.Property(e => e.MaterialModelID).HasColumnName("material_model_id");
        entity.Property(e => e.Amount).HasColumnName("amount").HasColumnType("numeric(18)");
        entity.Property(e => e.UseBWPrint).HasColumnName("use_bw_print").HasColumnType("smallint");
        entity.Property(e => e.UseColorPrint).HasColumnName("use_color_print").HasColumnType("smallint");
        entity.Property(e => e.UsePhotoPrint).HasColumnName("use_foto_print").HasColumnType("smallint");
    }
}
