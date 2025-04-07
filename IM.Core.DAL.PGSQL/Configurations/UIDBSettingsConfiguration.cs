using IM.Core.DAL.Postgres;
using InfraManager.DAL.Database.Import;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class UIDBSettingsConfiguration : UIDBSettingsConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_ui_db_settings";

        protected override void ConfigureDatabase(EntityTypeBuilder<UIDBSettings> builder)
        {
            builder.ToTable("uidb_settings", Options.Scheme);

            builder.Property(x => x.ID).HasColumnName("id");

            builder.Property(x => x.DBConfigurationID).HasColumnName("db_configuration_id");

            builder.Property(x => x.DatabaseName).HasColumnName("database_name");

            builder.Property(x => x.Removed).HasColumnName("removed");
        }
    }
}