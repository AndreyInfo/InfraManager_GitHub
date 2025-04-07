using IM.Core.DAL.Postgres;
using Inframanager.DAL.ActiveDirectory.Import;
using Inframanager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class UIADClassConfiguration : UIADClassConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_uiad_class";

        protected override void ConfigureDatabase(EntityTypeBuilder<UIADClass> builder)
        {
            builder.ToTable("uiad_class", Options.Scheme);

            builder.Property(x => x.ID).HasColumnName("id");

            builder.Property(x => x.Name).HasColumnName("name");
        }
    }
}