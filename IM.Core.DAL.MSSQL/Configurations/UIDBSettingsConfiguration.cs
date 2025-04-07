using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.Database.Import;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class UIDBSettingsConfiguration : UIDBSettingsConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_UIDBSettings";

        protected override void ConfigureDatabase(EntityTypeBuilder<UIDBSettings> builder)
        {
            builder.ToTable("UIDBSettings", Options.Scheme);

            builder.Property(x => x.ID).HasColumnName("ID");

            builder.Property(x => x.DBConfigurationID).HasColumnName("DBConfigurationId");

            builder.Property(x => x.DatabaseName).HasColumnName("DatabaseName");

            builder.Property(x => x.Removed).HasColumnName("Removed");
        }
    }
}