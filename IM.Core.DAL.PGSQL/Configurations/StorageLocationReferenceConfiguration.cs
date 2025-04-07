using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations;

internal class StorageLocationReferenceConfiguration : StorageLocationReferenceConfigurationBase
{
    protected override string KeyName => "pk_storage_location_reference";

    protected override void ConfigureDataBase(EntityTypeBuilder<StorageLocationReference> builder)
    {
        builder.ToTable("storage_location_reference", Options.Scheme);

        builder.Property(c => c.StorageLocationID).HasColumnName("storage_location_id");
        builder.Property(c => c.ObjectID).HasColumnName("object_id");
        builder.Property(c => c.ObjectClassID).HasColumnName("object_class_id");
    }
}