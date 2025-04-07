using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset.Subclasses;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

public class PrinterConfiguration : PrinterConfigurationBase
{
    protected override string PrimaryKeyName => "PK_Printer";

    protected override void ConfigureDatabase(EntityTypeBuilder<Printer> builder)
    {
        builder.ToTable("Printer", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID").HasDefaultValueSql("newid()");

        builder.Property(x => x.ConnectorType).HasColumnName("ConnectorType");
        builder.Property(e => e.ComplementaryID).HasColumnName("ComplementaryID");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
    }
}
