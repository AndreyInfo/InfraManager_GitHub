using InfraManager.DAL.Asset.DeviceMonitors;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;

internal sealed class DeviceMonitorConfiguration : DeviceMonitorConfigurationBase
{
    protected override string PrimaryKeyName => "PK_DeviceMonitor";

    protected override void ConfigureDataBase(EntityTypeBuilder<DeviceMonitor> builder)
    {
        builder.ToTable("DeviceMonitor", Options.Scheme);

        builder.Property(c=> c.ID).HasColumnName("ID");
        builder.Property(c=> c.ObjectID).HasColumnName("ObjectID");
        builder.Property(c=> c.ObjectClassID).HasColumnName("ObjectClassID");
        builder.Property(c=> c.Name).HasColumnName("Name");
        builder.Property(c=> c.Note).HasColumnName("Note");
        builder.Property(c=> c.Supervise).HasColumnName("Supervise");
        builder.Property(c=> c.Template).HasColumnName("Template");
        builder.Property(c=> c.IntervisitInterval).HasColumnName("IntervisitInterval");
        builder.Property(c=> c.PeriodOfStorage).HasColumnName("PeriodOfStorage");
        builder.Property(c=> c.RowVersion).HasColumnName("RowVersion").IsRowVersion();
    }
}
