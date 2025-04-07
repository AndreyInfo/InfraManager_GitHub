using InfraManager.DAL.Database.Import;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations
{
    internal class UidbFieldsConfiguration : UIDBFieldsConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_UIDBFileds";

        protected override void ConfigureDatabase(EntityTypeBuilder<UIDBFields> builder)
        {
            builder.ToTable("UIDBFields", Options.Scheme);

            builder.Property(x => x.ID).HasColumnName("ID");

            builder.Property(x => x.ConfigurationID).HasColumnName("ConfigurationID");

            builder.Property(x => x.FieldID).HasColumnName("FieldID");

            builder.Property(x => x.Value).HasColumnName("Value");
        }
    }
}