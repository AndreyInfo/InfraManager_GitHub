using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset.Subclasses;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace IM.Core.DAL.PGSQL.Configurations;

public class StorageConfiguration : StorageConfigurationBase
{
    protected override string PrimaryKeyName => "pk_storage";

    protected override void ConfigureDatabase(EntityTypeBuilder<Storage> builder)
    {
        builder.ToTable("storage", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.FormattedCapacity).HasColumnName("formatted_capacity");
        builder.Property(x => x.RecordingSurfaces).HasColumnName("recording_surfaces");
        builder.Property(x => x.InterfaceType).HasColumnName("interface_type");
        builder.Property(x => x.StorageClassID).HasColumnName("storage_class_id");
        builder.Property(x => x.StorageID).HasColumnName("storage_id");
        builder.Property(e => e.ComplementaryID).HasColumnName("complementary_id");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
    }
}
