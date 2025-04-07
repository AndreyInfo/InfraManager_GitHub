using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations;

internal class StorageLocationConfiguration : StorageLocationConfigurationBase
{
    protected override string KeyName => "pk_storage_location";

    protected override string StorageLocationReferancesFK => "fk_storage_location_reference_storage_location";

    protected override void ConfigureDataBase(EntityTypeBuilder<StorageLocation> builder)
    {
        builder.ToTable("storage_location", Options.Scheme);

        builder.Property(c => c.ID).HasColumnName("id");
        builder.Property(c => c.Name).HasColumnName("name");
        builder.Property(c => c.UserID).HasColumnName("user_id");
        builder.Property(c => c.ExternalID).HasColumnName("external_identifier");
        builder.HasXminRowVersion(c => c.RowVersion);
    }
}