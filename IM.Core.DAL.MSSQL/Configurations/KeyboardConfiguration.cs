using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.Asset.Subclasses;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

public class KeyboardConfiguration : KeyboardConfigurationBase
{
    protected override string PrimaryKeyName => "PK_Keyboard";

    protected override void ConfigureDatabase(EntityTypeBuilder<Keyboard> builder)
    {
        builder.ToTable("Keyboard", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID").HasDefaultValueSql("newid()");

        builder.Property(x => x.DelayPeriod).HasColumnName("DelayPeriod");
        builder.Property(x => x.NumberKeys).HasColumnName("NumberKeys");
        builder.Property(x => x.Layout).HasColumnName("Layout");
        builder.Property(x => x.ConnectorType).HasColumnName("ConnectorType");
        builder.Property(e => e.ComplementaryID).HasColumnName("ComplementaryID");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
    }
}
