using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Import.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace IM.Core.DAL.Microsoft.SqlServer.Configurations
{
    public class ServiceCatalogueImportSettingConfiguration : ServiceCatalogueImportSettingConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_ServiceCatalogueImportSetting";

        protected override string ForeignKeyName => "FK_ServiceCatalogueImportSetting_ServiceCatalogueImportCSVConfiguration";
        protected override string UI_Name => "UI_ServiceCatalogueImportSetting_Name";

        protected override void OnConfigurePartial(EntityTypeBuilder<ServiceCatalogueImportSetting> builder)
        {
            builder.ToTable("ServiceCatalogueImportSetting", "dbo");

            builder.Property(x => x.ID)
                .HasColumnName("ID");
            builder.Property(x => x.Name)
                .HasColumnName("Name");
            builder.Property(x => x.Note)
                .HasColumnName("Note");
            builder.Property(x => x.ServiceCatalogueImportCSVConfigurationID)
                .HasColumnName("ServiceCatalogueImportCSVConfigurationID");
            builder.Property(x => x.Path)
                .HasColumnName("Path");
            builder.Property(x => x.RowVersion)
                .HasColumnName("RowVersion")
                .IsRowVersion()
                .IsRequired(true)
                .HasColumnType("timestamp");
        }
    }
}
