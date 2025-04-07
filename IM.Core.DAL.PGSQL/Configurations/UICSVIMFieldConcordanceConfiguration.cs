using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Import.CSV;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class UICSVIMFieldConcordanceConfiguration : UICSVIMFieldConcordanceConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_uicsv_im_field_concordance";

        protected override string ForeignKeyName => "fk_uicsv_im_field_concordance_uicsv_configuration";

        protected override void ConfigureDatabase(EntityTypeBuilder<UICSVIMFieldConcordance> builder)
        {
            builder.ToTable("uicsv_im_field_concordance", Options.Scheme);

            builder.Property(x => x.CSVConfigurationID)
                .HasColumnName("csv_configuration_id");
            builder.Property(x => x.IMFieldID)
                .HasColumnName("im_field_id");
            builder.Property(x => x.Expression)
                .HasColumnName("expression");
        }
    }
}
