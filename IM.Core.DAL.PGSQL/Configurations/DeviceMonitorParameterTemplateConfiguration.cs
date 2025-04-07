using InfraManager.DAL.Asset.DeviceMonitors;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;
internal sealed class DeviceMonitorParameterTemplateConfiguration : DeviceMonitorParameterTemplateConfigurationBase
{
    protected override string PrimaryKeyName => "pk_device_monitor_parameter_template";

    protected override void ConfigureDataBase(EntityTypeBuilder<DeviceMonitorParameterTemplate> builder)
    {
        builder.ToTable("device_monitor_parameter_template", Options.Scheme);

        builder.Property(c => c.ID).HasColumnName("ID");
        builder.Property(c => c.ObjectID).HasColumnName("object_id");
        builder.Property(c => c.ObjectClassID).HasColumnName("object_class_id");
        builder.Property(c => c.Type).HasColumnName("type");
        builder.Property(c => c.Value).HasColumnName("value").HasColumnType("bytea");

        builder.HasXminRowVersion(c=> c.RowVersion);
    }
}
