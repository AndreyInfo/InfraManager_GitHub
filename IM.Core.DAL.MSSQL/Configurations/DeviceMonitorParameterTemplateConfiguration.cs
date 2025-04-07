using InfraManager.DAL.Asset.DeviceMonitors;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;
internal sealed class DeviceMonitorParameterTemplateConfiguration : DeviceMonitorParameterTemplateConfigurationBase
{
    protected override string PrimaryKeyName => "PK_DeviceMonitorParameterTemplate";

    protected override void ConfigureDataBase(EntityTypeBuilder<DeviceMonitorParameterTemplate> builder)
    {
        builder.ToTable("DeviceMonitorParameterTemplate", Options.Scheme);

        builder.Property(c => c.ID).HasColumnName("ID");
        builder.Property(c => c.ObjectID).HasColumnName("ObjectID");
        builder.Property(c => c.ObjectClassID).HasColumnName("ObjectClassID");
        builder.Property(c => c.Type).HasColumnName("Type");
        builder.Property(c => c.Value).HasColumnName("Value").HasColumnType("image");

        builder.Property(c => c.RowVersion).HasColumnName("RowVersion").IsRowVersion();

    }
}
