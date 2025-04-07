using InfraManager.DAL.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class SettingConfigurationBase : IEntityTypeConfiguration<Setting>
{
    protected abstract string KeyName { get; }
    public void Configure(EntityTypeBuilder<Setting> builder)
    {
        builder.HasKey(x => x.Id).HasName(KeyName);

        builder.Property(x => x.Id).ValueGeneratedNever();

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<Setting> builder);
}
