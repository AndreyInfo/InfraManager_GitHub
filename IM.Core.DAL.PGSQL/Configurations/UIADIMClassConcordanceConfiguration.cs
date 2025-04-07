using IM.Core.DAL.Postgres;
using Inframanager.DAL.ActiveDirectory.Import;
using Inframanager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class UIADIMClassConcordanceConfiguration : UIADIMClassConcordanceConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_uiad_im_class_concordance";

        protected override void ConfigureDatabase(EntityTypeBuilder<UIADIMClassConcordance> builder)
        {
            builder.ToTable("uiad_im_class_concordance", Options.Scheme);

            builder.Property(x => x.ConfigurationID).HasColumnName("ad_configuration_id");

            builder.Property(x => x.ClassID).HasColumnName("ad_class_id");

            builder.Property(x => x.IMClassID).HasColumnName("im_class_id");

        }
    }
}