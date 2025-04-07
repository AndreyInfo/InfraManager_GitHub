using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.Asset.Subclasses;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

public class NetworkAdapterConfiguration : NetworkAdapterConfigurationBase
{
    protected override string PrimaryKeyName => "PK_NetworkAdapter";

    protected override void ConfigureDatabase(EntityTypeBuilder<NetworkAdapter> builder)
    {
        builder.ToTable("NetworkAdapter", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID").HasDefaultValueSql("newid()");

        builder.Property(x => x.MaxSpeed).HasColumnName("MaxSpeed");
        builder.Property(x => x.InterfaceType).HasColumnName("InterfaceType");
        builder.Property(e => e.ComplementaryID).HasColumnName("ComplementaryID");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
    }
}
