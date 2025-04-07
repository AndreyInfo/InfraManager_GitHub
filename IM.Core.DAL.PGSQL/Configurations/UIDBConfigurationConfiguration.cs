using IM.Core.DAL.Postgres;
using InfraManager.DAL.Database.Import;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class UIDBConfigurationConfiguration : UIDBConfigurationConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_uidb_configurations";

        protected override void ConfigureDatabase(EntityTypeBuilder<UIDBConfiguration> builder)
        {
            builder.ToTable("uidb_configurations", Options.Scheme);

            builder.Property(x => x.ID).HasColumnName("id");

            builder.Property(x => x.Name).HasColumnName("name");

            builder.Property(x => x.Note).HasColumnName("note");

            builder.Property(x => x.OrganizationTableName).HasColumnName("organization_table_name");

            builder.Property(x => x.SubdivisionTableName).HasColumnName("subdivision_table_name");

            builder.Property(x => x.UserTableName).HasColumnName("user_table_name");
        }
    }
}