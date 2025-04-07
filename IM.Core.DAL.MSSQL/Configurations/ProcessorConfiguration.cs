using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.Asset.Subclasses;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

public partial class ProcessorConfiguration : ProcessorConfigurationBase
{
    protected override string PrimaryKeyName => "PK_Processor";

    protected override void ConfigureDatabase(EntityTypeBuilder<Processor> builder)
    {
        builder.ToTable("Processor", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID").HasDefaultValueSql("newid()");

        builder.Property(x => x.MaxClockSpeed).HasColumnName("MaxClockSpeed");
        builder.Property(x => x.CurrentClockSpeed).HasColumnName("CurrentClockSpeed");
        builder.Property(x => x.L2cacheSize).HasColumnName("L2CacheSize");
        builder.Property(x => x.SocketDesignation).HasColumnName("SocketDesignation");
        builder.Property(x => x.Platform).HasColumnName("Platform");
        builder.Property(x => x.NumberOfCores).HasColumnName("NumberOfCores");
        builder.Property(e => e.ComplementaryID).HasColumnName("ComplementaryID");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
    }
}
