using InfraManager.DAL.AccessManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace InfraManager.DAL.EntityConfigurations;

public abstract class ObjectAccessConfigurationBase : IEntityTypeConfiguration<ObjectAccess>
{
    protected abstract string KeyName { get; }

    public void Configure(EntityTypeBuilder<ObjectAccess> builder)
    {
        builder.HasKey(c => c.ID).HasName(KeyName);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<ObjectAccess> builder);
}
