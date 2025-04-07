using IM.Core.DAL.Postgres;
using InfraManager.DAL.Database.Import;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class UIDBFieldsConfiguration : UIDBFieldsConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_dbui_fields";

        protected override void ConfigureDatabase(EntityTypeBuilder<UIDBFields> builder)
        {
            builder.ToTable("uidb_fields", Options.Scheme);

            builder.Property(x => x.ID).HasColumnName("id");

            builder.Property(x => x.ConfigurationID).HasColumnName("configuration_id");

            builder.Property(x => x.FieldID).HasColumnName("field_id");

            builder.Property(x => x.Value).HasColumnName("value");
        }
    }
}