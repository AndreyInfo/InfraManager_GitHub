using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Import.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace IM.Core.DAL.Microsoft.SqlServer.Configurations
{
    public class ServiceCatalogueImportCSVConfigurationConfiguration : ServiceCatalogueImportCSVConfigurationConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_ServiceCatalogueImportCSVConfiguration";
        protected override string UI_Name => "UI_ServiceCatalogueImportCSVConfiguration_Name";
        protected override void OnConfigurePartial(EntityTypeBuilder<ServiceCatalogueImportCSVConfiguration> builder)
        {
            builder.ToTable("ServiceCatalogueImportCSVConfiguration", "dbo");

            builder.Property(x => x.ID)
                .HasColumnName("ID");
            builder.Property(x => x.Name)
                .HasColumnName("Name");
            builder.Property(x => x.Note)
                .HasColumnName("Note");
            builder.Property(x => x.Delimeter)
                .HasColumnName("Delimiter");
            builder.Property(x => x.RowVersion)
                .HasColumnName("RowVersion")
                .IsRowVersion()
                .IsRequired(true)
                .HasColumnType("timestamp");
        }
    }
}
