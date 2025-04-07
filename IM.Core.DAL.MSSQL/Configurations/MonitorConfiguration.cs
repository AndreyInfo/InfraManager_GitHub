using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.Asset.Subclasses;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

public class MonitorConfiguration : MonitorConfigurationBase
{
    protected override string PrimaryKeyName => "PK_Monitor";

    protected override void ConfigureDatabase(EntityTypeBuilder<Monitor> builder)
    {
        builder.ToTable("Monitor", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID").HasDefaultValueSql("newid()");

        builder.Property(x => x.Resolution).HasColumnName("Resolution");
        builder.Property(x => x.FontResolution).HasColumnName("FontResolution");
        builder.Property(x => x.Diagonal).HasColumnName("Diagonal");
        builder.Property(e => e.ComplementaryID).HasColumnName("ComplementaryID");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
    }
}
