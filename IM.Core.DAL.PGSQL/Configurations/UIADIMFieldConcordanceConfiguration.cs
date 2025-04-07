using IM.Core.DAL.Postgres;
using Inframanager.DAL.ActiveDirectory.Import;
using Inframanager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class UIADIMFieldConcordanceConfiguration : UIADIMFieldConcordanceConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_uiad_im_field_concordance";

        protected override void ConfigureDatabase(EntityTypeBuilder<UIADIMFieldConcordance> builder)
        {
            builder.ToTable("uiad_im_field_concordance", Options.Scheme);

            builder.Property(x => x.ConfigurationID).HasColumnName("ad_configuration_id");

            builder.Property(x => x.ClassID).HasColumnName("ad_class_id");

            builder.Property(x => x.IMFieldID).HasColumnName("im_field_id");

            builder.Property(x => x.Expression).HasColumnName("expression");
        }
    }
}