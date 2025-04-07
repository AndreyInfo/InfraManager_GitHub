using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.Database.Import;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class UIDBConnectionStringConfiguration : UIDBConnectionStringConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_UIDBConnectionStrings";

        protected override void ConfigureDatabase(EntityTypeBuilder<UIDBConnectionString> builder)
        {
            builder.ToTable("UIDBConnectionStirngs", Options.Scheme);

            builder.Property(x => x.ID).HasColumnName("ID");

            builder.Property(x => x.SettingsID).HasColumnName("UIDBSettingsID");

            builder.Property(x => x.ConnectionString).HasColumnName("ConnectionString");

            builder.Property(x => x.ImportSourceType).HasColumnName("ImportSourceType");
        }
    }
}