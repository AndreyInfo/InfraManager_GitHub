using InfraManager.DAL.Asset.DeviceMonitors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class DeviceMonitorConfigurationBase : IEntityTypeConfiguration<DeviceMonitor>
{
    protected abstract string PrimaryKeyName { get; }
    public void Configure(EntityTypeBuilder<DeviceMonitor> builder)
    {
        builder.HasKey(c=> c.ID).HasName(PrimaryKeyName);

        builder.Property(c=> c.Name).IsRequired(true).HasMaxLength(250);
        builder.Property(c=> c.Note).IsRequired(true).HasMaxLength(500);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<DeviceMonitor> builder);
}
