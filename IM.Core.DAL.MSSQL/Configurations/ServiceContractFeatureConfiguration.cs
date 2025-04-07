using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;

internal sealed class ServiceContractFeatureConfiguration : ServiceContractFeatureConfigurationBase
{
    protected override string PrimaryKeyName => "PK_ServiceContractFeature";
    protected override string ProductCatalogTypeForeignKey => "FK_ServiceContractFeature_ProductCatalogTypeID";


    protected override void ConfigureDataBase(EntityTypeBuilder<ServiceContractFeature> builder)
    {
        builder.ToTable("ServiceContractFeature", Options.Scheme);

        builder.Property(c=> c.Feature).HasColumnName("Feature");
        builder.Property(c=> c.ProductCatalogTypeID).HasColumnName("ProductCatalogTypeID");
    }
}
