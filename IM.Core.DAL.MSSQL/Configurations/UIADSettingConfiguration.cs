using Inframanager.DAL.ActiveDirectory.Import;
using Inframanager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class UIADSettingConfiguration : UIADSettingConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_UIADSetting";

        protected override void ConfigureDatabase(EntityTypeBuilder<UIADSetting> builder)
        {
            builder.ToTable("UIADSetting", "dbo");
            builder.Property(x => x.ID).HasColumnName("ID");
            builder.Property(x => x.ADConfigurationID).HasColumnName("ADConfigurationID");
            builder.Property(x => x.Removed).HasColumnName("Removed");
        }
    }
}