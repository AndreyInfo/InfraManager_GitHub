using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset.Subclasses;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

public class ModemConfiguration : ModemConfigurationBase
{
    protected override string PrimaryKeyName => "pk_modem";

    protected override void ConfigureDatabase(EntityTypeBuilder<Modem> builder)
    {
        builder.ToTable("modem", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.DataRate).HasColumnName("data_rate");
        builder.Property(x => x.ModemTechnology).HasColumnName("modem_technology");
        builder.Property(x => x.ConnectorType).HasColumnName("connector_type");
        builder.Property(e => e.ComplementaryID).HasColumnName("complementary_id");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
    }
}
