using Inframanager.DAL.ActiveDirectory.Import;
using Inframanager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class UIADIMClassConcordanceConfiguration : UIADIMClassConcordanceConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_UIADIMClassConcordance";

        protected override void ConfigureDatabase(EntityTypeBuilder<UIADIMClassConcordance> builder)
        {
            builder.ToTable("UIADIMClassConcordance", "dbo");

            builder.Property(x => x.ConfigurationID).HasColumnName("ADConfigurationID");

            builder.Property(x => x.ClassID).HasColumnName("AdClassID");

            builder.Property(x => x.IMClassID).HasColumnName("IMClassID");

        }
    }
}