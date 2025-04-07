using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal class StorageLocationReferenceConfiguration : StorageLocationReferenceConfigurationBase
{
    protected override string KeyName => "PK_StorageLocationReference";

    protected override void ConfigureDataBase(EntityTypeBuilder<StorageLocationReference> builder)
    {
        builder.ToTable("StorageLocationReference", "dbo");

        builder.Property(c => c.StorageLocationID).HasColumnName("StorageLocationID");
        builder.Property(c => c.ObjectID).HasColumnName("ObjectID");
        builder.Property(c => c.ObjectClassID).HasColumnName("ObjectClassID");
    }
}
