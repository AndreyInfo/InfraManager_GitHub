using IM.Core.DAL.Postgres;
using Inframanager.DAL.ActiveDirectory.Import;
using Inframanager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class UIADConfigurationConfiguration : UIADConfigurationConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_uiad_configuration";

        protected override void ConfigureDatabase(EntityTypeBuilder<UIADConfiguration> builder)
        {
            builder.ToTable("uiad_configuration", Options.Scheme);

            builder.Property(x => x.ID).HasColumnName("id");

            builder.Property(x => x.Name).HasColumnName("name");

            builder.Property(x => x.ShowUsersInADTree).HasColumnName("show_users_in_ad_tree");

            builder.Property(x => x.Note).HasColumnName("note");
        }
    }
}