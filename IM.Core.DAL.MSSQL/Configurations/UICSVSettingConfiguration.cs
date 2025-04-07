using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Import;
using InfraManager.DAL.Import.CSV;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class UICSVSettingConfiguration : UICSVSettingConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_UICSVSetting";

        protected override string ForeignKeyName => "FK_UICSVSetting_UICSVConfiguration";

        protected override void ConfigureDatabase(EntityTypeBuilder<UICSVSetting> builder)
        {
            builder.ToTable("UICSVSetting", "dbo");

            builder.Property(x => x.ID)
                .HasColumnName("ID");
            builder.Property(x => x.CSVConfigurationID)
                .HasColumnName("CSVConfigurationID");
            builder.Property(x => x.Path)
                .HasColumnName("Path");
            builder.Property(x => x.Removed).HasColumnName("Removed");
        }
    }
}
