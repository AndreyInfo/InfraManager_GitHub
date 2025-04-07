using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;

internal sealed class ServiceContractFeatureConfiguration : ServiceContractFeatureConfigurationBase
{
    protected override string PrimaryKeyName => "service_contract_feature_pkey";
    protected override string ProductCatalogTypeForeignKey => "fk_service_contract_feature_product_catalog_type_id";

    protected override void ConfigureDataBase(EntityTypeBuilder<ServiceContractFeature> builder)
    {
        builder.ToTable("service_contract_feature", Options.Scheme);

        builder.Property(c => c.Feature).HasColumnName("feature");
        builder.Property(c => c.ProductCatalogTypeID).HasColumnName("product_catalog_type_id");
    }
}
