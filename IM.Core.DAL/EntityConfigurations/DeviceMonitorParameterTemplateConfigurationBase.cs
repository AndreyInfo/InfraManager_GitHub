using InfraManager.DAL.Asset.DeviceMonitors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;
public abstract class DeviceMonitorParameterTemplateConfigurationBase : IEntityTypeConfiguration<DeviceMonitorParameterTemplate>
{
    protected abstract string PrimaryKeyName { get; }

    public void Configure(EntityTypeBuilder<DeviceMonitorParameterTemplate> builder)
    {
        builder.HasKey(c=> c.ID).HasName(PrimaryKeyName);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<DeviceMonitorParameterTemplate> builder);
}
