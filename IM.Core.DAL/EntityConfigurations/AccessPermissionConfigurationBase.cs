using InfraManager.DAL.AccessManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class AccessPermissionConfigurationBase : IEntityTypeConfiguration<AccessPermission>
{
    protected abstract string KeyName { get; }
    public void Configure(EntityTypeBuilder<AccessPermission> builder)
    {
        builder.HasKey(x => x.ID).HasName(KeyName);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<AccessPermission> builder);
}
