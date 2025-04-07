using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Import.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace IM.Core.DAL.Postgres.Configurations
{
    public class ServiceCatalogueImportCSVConfigurationConcordanceConfiguration : ServiceCatalogueImportCSVConfigurationConcordanceConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_service_catalogue_import_csv_configuration_concordance";

        protected override string ForeignKeyName => "fk_service_catalogue_import_csv_configuration_concordance_servi";

        protected override void OnConfigurePartial(EntityTypeBuilder<ServiceCatalogueImportCSVConfigurationConcordance> builder)
        {
            builder.ToTable("service_catalogue_import_csv_configuration_concordance", Options.Scheme);

            builder.Property(x => x.ServiceCatalogueImportCSVConfigurationID)
                .HasColumnName("service_catalogue_import_csv_configuration_id");
            builder.Property(x => x.Field)
                .HasColumnName("field");
            builder.Property(x => x.Expression)
                .HasColumnName("expression");
        }
    }
}
