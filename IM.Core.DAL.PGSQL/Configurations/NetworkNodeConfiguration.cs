using InfraManager.DAL.ConfigurationUnits;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;

internal sealed class NetworkNodeConfiguration : NetworkNodeConfigurationBase
{
    protected override string NetworkDeviceFK => "fk_network_node_network_device";
    protected override string TerminalDeviceFK => "fk_network_node_terminal_device";
    protected override string DeviceApplicationFK => "fk_network_node_device_application";

    protected override void ConfigureDataBase(EntityTypeBuilder<NetworkNode> builder)
    {
        builder.ToTable("network_node", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.IPAddress).HasColumnName("ip_address");
        builder.Property(x => x.IPMask).HasColumnName("ip_mask");
        builder.Property(x => x.NetworkDeviceID).HasColumnName("network_device_id");
        builder.Property(x => x.TerminalDeviceID).HasColumnName("terminal_device_id");
        builder.Property(x => x.DeviceApplicationID).HasColumnName("device_application_id");
    }
}
