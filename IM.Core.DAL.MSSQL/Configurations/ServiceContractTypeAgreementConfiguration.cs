using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;
internal sealed class ServiceContractTypeAgreementConfiguration : ServiceContractTypeAgreementConfigurationBase
{
    protected override string PrimaryKey => "PK_ServiceContractTypeAgreement";

    protected override string ProductCatalogForeignKey => "FK_ServiceContractTypeAgreement_ProductCatalogTypeID";

    protected override string AgreementLifeCycleForeignKey => "FK_ServiceContractTypeAgreement_AgreementLifeCycleID";

    protected override void ConfigureDataBase(EntityTypeBuilder<ServiceContractTypeAgreement> builder)
    {
        builder.ToTable("ServiceContractTypeAgreement", Options.Scheme);

        builder.Property(c=> c.ProductCatalogTypeID).HasColumnName("ProductCatalogTypeID");
        builder.Property(c=> c.AgreementLifeCycleID).HasColumnName("AgreementLifeCycleID");
    }
}
