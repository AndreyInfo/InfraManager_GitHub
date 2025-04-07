using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Import.ServiceCatalogue;
using InfraManager.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations
{
    public class ServiceCatalogueImportSettingConfiguration : ServiceCatalogueImportSettingConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_service_catalogue_import_setting";

        protected override string ForeignKeyName => "fk_service_catalogue_import_setting_service_catalogue_import_cs";
        protected override string UI_Name => "ui_service_catalogue_import_setting_name";

        protected override void OnConfigurePartial(EntityTypeBuilder<ServiceCatalogueImportSetting> builder)
        {
            builder.ToTable("service_catalogue_import_setting", Options.Scheme);

            builder.Property(x => x.ID)
                .HasColumnName("id");
            builder.Property(x => x.Name)
                .HasColumnName("name");
            builder.Property(x => x.Note)
                .HasColumnName("note");
            builder.Property(x => x.ServiceCatalogueImportCSVConfigurationID)
                .HasColumnName("service_catalogue_import_csv_configuration_id");
            builder.Property(x => x.Path)
                .HasColumnName("path");
            builder.HasXminRowVersion(x => x.RowVersion);


        }
    }
}