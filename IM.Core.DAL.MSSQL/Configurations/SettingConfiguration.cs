using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal class SettingConfiguration : SettingConfigurationBase
{
    protected override string KeyName => "PK_Setting";

    protected override void ConfigureDataBase(EntityTypeBuilder<Setting> builder)
    {
        builder.ToTable("Setting", "dbo");

        builder.Property(x => x.Id).HasColumnName("ID");
        builder.Property(x => x.Value).HasColumnName("Value").HasColumnType("image");
        builder.Property(x => x.RowVersion).IsRowVersion().HasColumnName("RowVersion");
    }
}
