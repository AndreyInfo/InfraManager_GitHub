using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using InfraManager.DAL.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

internal class SettingConfiguration : SettingConfigurationBase
{
    protected override string KeyName => "setting_pkey";

    protected override void ConfigureDataBase(EntityTypeBuilder<Setting> builder)
    {
        builder.ToTable("setting", Options.Scheme);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Value).HasColumnName("value").HasColumnType("bytea");
        builder.HasXminRowVersion(e => e.RowVersion);
    }
}