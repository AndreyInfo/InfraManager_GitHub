using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset.Subclasses;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

public class KeyboardConfiguration : KeyboardConfigurationBase
{
    protected override string PrimaryKeyName => "pk_keyboard";

    protected override void ConfigureDatabase(EntityTypeBuilder<Keyboard> builder)
    {
        builder.ToTable("keyboard", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.DelayPeriod).HasColumnName("delay_period");
        builder.Property(x => x.NumberKeys).HasColumnName("number_keys");
        builder.Property(x => x.Layout).HasColumnName("layout");
        builder.Property(x => x.ConnectorType).HasColumnName("connector_type");
        builder.Property(e => e.ComplementaryID).HasColumnName("complementary_id");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
    }
}
