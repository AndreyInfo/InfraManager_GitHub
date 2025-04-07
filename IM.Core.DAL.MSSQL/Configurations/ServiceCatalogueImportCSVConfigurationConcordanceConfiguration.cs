using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Import.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;


namespace IM.Core.DAL.Microsoft.SqlServer.Configurations
{
    public class ServiceCatalogueImportCSVConfigurationConcordanceConfiguration : ServiceCatalogueImportCSVConfigurationConcordanceConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_ServiceCatalogueImportCSVConfigurationConcordance";

        protected override string ForeignKeyName => "FK_ServiceCatalogueImportCSVConfigurationConcordance_ServiceCatalogueImportCSVConfiguration";

        protected override void OnConfigurePartial(EntityTypeBuilder<ServiceCatalogueImportCSVConfigurationConcordance> builder)
        {
            builder.ToTable("ServiceCatalogueImportCSVConfigurationConcordance", "dbo");

            builder.Property(x => x.ServiceCatalogueImportCSVConfigurationID)
                .HasColumnName("ServiceCatalogueImportCSVConfigurationID");
            builder.Property(x => x.Field)
                .HasColumnName("Field");
            builder.Property(x => x.Expression)
                .HasColumnName("Expression");
        }
    }
}
