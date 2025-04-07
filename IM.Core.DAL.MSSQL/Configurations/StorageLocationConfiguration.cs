using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Location;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal class StorageLocationConfiguration : StorageLocationConfigurationBase
{
    protected override string KeyName => "PK_StorageLocation";

    protected override string StorageLocationReferancesFK => "FK_StorageLocationReference_StorageLocation";

    protected override void ConfigureDataBase(EntityTypeBuilder<StorageLocation> builder)
    {
        builder.ToTable("StorageLocation", "dbo");

        builder.Property(c => c.ID).HasColumnName("ID");
        builder.Property(c => c.Name).HasColumnName("Name");
        builder.Property(c => c.UserID).HasColumnName("UserID");
        builder.Property(c => c.ExternalID).HasColumnName("ExternalIdentifier");
        builder.Property(c => c.RowVersion).IsRowVersion().HasColumnName("RowVersion");
    }
}
