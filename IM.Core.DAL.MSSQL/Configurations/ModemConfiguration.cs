using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.Asset.Subclasses;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

public class ModemConfiguration : ModemConfigurationBase
{
    protected override string PrimaryKeyName => "PK_Modem";

    protected override void ConfigureDatabase(EntityTypeBuilder<Modem> builder)
    {
        builder.ToTable("Modem", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID").HasDefaultValueSql("newid()");

        builder.Property(x => x.DataRate).HasColumnName("DataRate");
        builder.Property(x => x.ModemTechnology).HasColumnName("ModemTechnology");
        builder.Property(x => x.ConnectorType).HasColumnName("ConnectorType");
        builder.Property(e => e.ComplementaryID).HasColumnName("ComplementaryID");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
    }
}
