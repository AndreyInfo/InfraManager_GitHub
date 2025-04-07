using InfraManager.DAL.Asset.DeviceMonitors;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;

internal class DeviceMonitorConfiguration : DeviceMonitorConfigurationBase
{
    protected override string PrimaryKeyName => "PK_DeviceMonitor";

    protected override void ConfigureDataBase(EntityTypeBuilder<DeviceMonitor> builder)
    {
        builder.ToTable("device_monitor", Options.Scheme);

        builder.Property(c => c.ID).HasColumnName("id");
        builder.Property(c => c.ObjectID).HasColumnName("object_id");
        builder.Property(c => c.ObjectClassID).HasColumnName("object_class_id");
        builder.Property(c => c.Name).HasColumnName("name");
        builder.Property(c => c.Note).HasColumnName("note");
        builder.Property(c => c.Supervise).HasColumnName("supervise");
        builder.Property(c => c.Template).HasColumnName("template");
        builder.Property(c => c.IntervisitInterval).HasColumnName("intervisit_interval");
        builder.Property(c => c.PeriodOfStorage).HasColumnName("period_of_storage");
        builder.HasXminRowVersion(c=> c.RowVersion);
    }
}
