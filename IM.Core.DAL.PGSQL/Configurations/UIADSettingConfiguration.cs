using IM.Core.DAL.Postgres;
using Inframanager.DAL.ActiveDirectory.Import;
using Inframanager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class UIADSettingConfiguration : UIADSettingConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_uiad_setting";

        protected override void ConfigureDatabase(EntityTypeBuilder<UIADSetting> builder)
        {
            builder.ToTable("uiad_setting", Options.Scheme);
            builder.Property(x => x.ID).HasColumnName("id");
            builder.Property(x => x.ADConfigurationID).HasColumnName("ad_configuration_id");
            builder.Property(x => x.Removed).HasColumnName("removed");
        }
    }
}