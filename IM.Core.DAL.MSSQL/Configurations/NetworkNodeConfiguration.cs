using InfraManager.DAL.ConfigurationUnits;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;

internal sealed class NetworkNodeConfiguration : NetworkNodeConfigurationBase
{
    protected override string NetworkDeviceFK => "FK_NetworkNode_NetworkDevice";
    protected override string TerminalDeviceFK => "FK_NetworkNode_TerminalDevice";
    protected override string DeviceApplicationFK => "FK_NetworkNode_DeviceApplication";

    protected override void ConfigureDataBase(EntityTypeBuilder<NetworkNode> builder)
    {
        builder.ToTable("NetworkNode", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.IPAddress).HasColumnName("IPAddress");
        builder.Property(x => x.IPMask).HasColumnName("IPMask");
        builder.Property(x => x.NetworkDeviceID).HasColumnName("NetworkDeviceID");
        builder.Property(x => x.TerminalDeviceID).HasColumnName("TerminalDeviceID");
        builder.Property(x => x.DeviceApplicationID).HasColumnName("DeviceApplicationID");
    }
}
