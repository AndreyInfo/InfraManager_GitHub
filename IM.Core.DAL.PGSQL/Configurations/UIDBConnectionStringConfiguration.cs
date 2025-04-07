using IM.Core.DAL.Postgres;
using InfraManager.DAL.Database.Import;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class UIDBConnectionStringConfiguration : UIDBConnectionStringConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_connection_string";

        protected override void ConfigureDatabase(EntityTypeBuilder<UIDBConnectionString> builder)
        {
            builder.ToTable("uidb_connection_strings", Options.Scheme);

            builder.Property(x => x.ID).HasColumnName("id");

            builder.Property(x => x.SettingsID).HasColumnName("uidb_settings_id");

            builder.Property(x => x.ConnectionString).HasColumnName("connection_string");

            builder.Property(x => x.ImportSourceType).HasColumnName("import_source_type");
        }
    }
}