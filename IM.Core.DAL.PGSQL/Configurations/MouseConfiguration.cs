using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset.Subclasses;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

public class MouseConfiguration : MouseConfigurationBase
{
    protected override string PrimaryKeyName => "pk_mouse";

    protected override void ConfigureDatabase(EntityTypeBuilder<Mouse> builder)
    {
        builder.ToTable("mouse", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.ConnectorType).HasColumnName("connector_type");
        builder.Property(x => x.NumberButtons).HasColumnName("number_buttons");
        builder.Property(e => e.ComplementaryID).HasColumnName("complementary_id");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
    }
}
