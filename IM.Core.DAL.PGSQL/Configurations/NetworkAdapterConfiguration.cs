using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset.Subclasses;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

public class NetworkAdapterConfiguration : NetworkAdapterConfigurationBase
{
    protected override string PrimaryKeyName => "pk_network_adapter";

    protected override void ConfigureDatabase(EntityTypeBuilder<NetworkAdapter> builder)
    {
        builder.ToTable("network_adapter", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.MaxSpeed).HasColumnName("max_speed");
        builder.Property(x => x.InterfaceType).HasColumnName("interface_type");
        builder.Property(e => e.ComplementaryID).HasColumnName("complementary_id");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
    }
}
