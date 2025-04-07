using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Import.ServiceCatalogue;
using InfraManager.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace IM.Core.DAL.Postgres.Configurations
{
    public class ServiceCatalogueImportCSVConfigurationConfiguration : ServiceCatalogueImportCSVConfigurationConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_service_catalogue_import_csv_configuration";
        protected override string UI_Name => "ui_service_catalogue_import_csv_configuration_name";
        protected override void OnConfigurePartial(EntityTypeBuilder<ServiceCatalogueImportCSVConfiguration> builder)
        {
            builder.ToTable("service_catalogue_import_csv_configuration", Options.Scheme);

            builder.Property(x => x.ID)
                 .HasColumnName("id");
            builder.Property(x => x.Name)
                .HasColumnName("name");
            builder.Property(x => x.Note)
                .HasColumnName("note");
            builder.Property(x => x.Delimeter)
                .HasColumnName("delimiter");
            builder.HasXminRowVersion(x => x.RowVersion);
        }
    }
}
