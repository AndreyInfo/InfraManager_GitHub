using InfraManager.DAL.Import.CSV;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class UICSVSettingConfiguration : UICSVSettingConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_ui_setting";

        protected override string ForeignKeyName => "fk_uicsv_setting_uicsv_configuration";

        protected override void ConfigureDatabase(EntityTypeBuilder<UICSVSetting> builder)
        {
            builder.ToTable("uicsv_setting", Options.Scheme);

            builder.Property(x => x.ID).HasColumnName("id");
            builder.Property(x => x.CSVConfigurationID).HasColumnName("csv_configuration_id");
            builder.Property(x => x.Path).HasColumnName("path");
            builder.Property(x=>x.Removed).HasColumnName("removed");
        }
    }
}