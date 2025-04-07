using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset.Subclasses;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

public class FloppydriveConfiguration : FloppydriveConfigurationBase
{
    protected override string PrimaryKeyName => "pk_floppy_drives";

    protected override void ConfigureDatabase(EntityTypeBuilder<Floppydrive> builder)
    {
        builder.ToTable("floppy_drives", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.Heads).HasColumnName("heads");
        builder.Property(x => x.Cylinders).HasColumnName("cylinders");
        builder.Property(x => x.Sectors).HasColumnName("sectors");
        builder.Property(e => e.ComplementaryID).HasColumnName("complementary_id");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
    }
}
