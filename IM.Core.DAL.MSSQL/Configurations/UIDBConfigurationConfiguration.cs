using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.Database.Import;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class UIDBConfigurationConfiguration : UIDBConfigurationConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_UIDBConfigurations";

        protected override void ConfigureDatabase(EntityTypeBuilder<UIDBConfiguration> builder)
        {
            builder.ToTable("UIDBConfigurations", Options.Scheme);

            builder.Property(x => x.ID).HasColumnName("ID");

            builder.Property(x => x.Name).HasColumnName("Name");

            builder.Property(x => x.Note).HasColumnName("Note");

            builder.Property(x => x.OrganizationTableName).HasColumnName("OrganizationTableName");

            builder.Property(x => x.SubdivisionTableName).HasColumnName("SubdivisionTableName");

            builder.Property(x => x.UserTableName).HasColumnName("UserTableName");
        }
    }
}