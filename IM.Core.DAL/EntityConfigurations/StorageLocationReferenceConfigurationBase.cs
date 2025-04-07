using InfraManager.DAL.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class StorageLocationReferenceConfigurationBase : IEntityTypeConfiguration<StorageLocationReference>
{
    protected abstract string KeyName { get; }
    public void Configure(EntityTypeBuilder<StorageLocationReference> builder)
    {
        builder.HasKey(c => new { c.StorageLocationID, c.ObjectID }).HasName(KeyName);
        
        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<StorageLocationReference> builder);
}
