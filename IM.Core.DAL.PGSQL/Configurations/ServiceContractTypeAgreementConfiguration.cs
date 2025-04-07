using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;
internal sealed class ServiceContractTypeAgreementConfiguration : ServiceContractTypeAgreementConfigurationBase
{
    protected override string PrimaryKey => "pk_service_contract_type_agreement";

    protected override string ProductCatalogForeignKey => "fk_service_contract_type_agreement_product_catalog_type_id";

    protected override string AgreementLifeCycleForeignKey => "fk_service_contract_type_agreement_agreement_life_cycle_id";

    protected override void ConfigureDataBase(EntityTypeBuilder<ServiceContractTypeAgreement> builder)
    {
        builder.ToTable("service_contract_type_agreement", Options.Scheme);

        builder.Property(c => c.ProductCatalogTypeID).HasColumnName("product_catalog_type_id");
        builder.Property(c => c.AgreementLifeCycleID).HasColumnName("agreement_life_cycle_id");
    }
}
