using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset.Subclasses;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

public class CDAndDVDDrivesConfiguration : CDAndDVDDrivesConfigurationBase
{
    protected override string PrimaryKeyName => "pk_cd_and_dvd_drives";

    protected override void ConfigureDatabase(EntityTypeBuilder<CDAndDVDDrives> builder)
    {
        builder.ToTable("cd_and_dvd_drives", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.WriteSpeed).HasColumnName("write_speed");
        builder.Property(x => x.ReadSpeed).HasColumnName("read_speed");
        builder.Property(x => x.DriveCapabilities).HasColumnName("drive_capabilities");
        builder.Property(e => e.ComplementaryID).HasColumnName("complementary_id");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
    }
}