using Inframanager.DAL.ActiveDirectory.Import;
using Inframanager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class UIADIMFieldConcordanceConfiguration : UIADIMFieldConcordanceConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_UIADIMFieldConcordance";

        protected override void ConfigureDatabase(EntityTypeBuilder<UIADIMFieldConcordance> builder)
        {
            builder.ToTable("UIADIMFieldConcordance", "dbo");

            builder.Property(x => x.ConfigurationID).HasColumnName("ADConfigurationID");

            builder.Property(x => x.ClassID).HasColumnName("AdClassID");

            builder.Property(x => x.IMFieldID).HasColumnName("IMFieldID");

            builder.Property(x => x.Expression).HasColumnName("Expression");
        }
    }
}