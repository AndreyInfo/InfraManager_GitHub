using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.Asset.Subclasses;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

public class MemoryConfiguration : MemoryConfigurationBase
{
    protected override string PrimaryKeyName => "PK_Memory";

    protected override void ConfigureDatabase(EntityTypeBuilder<Memory> builder)
    {
        builder.ToTable("Memory", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID").HasDefaultValueSql("newid()");

        builder.Property(x => x.Capacity).HasColumnName("Capacity");
        builder.Property(x => x.DeviceLocator).HasColumnName("DeviceLocator");
        builder.Property(e => e.ComplementaryID).HasColumnName("ComplementaryID");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
    }
}
