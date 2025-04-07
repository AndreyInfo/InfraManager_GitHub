using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;

internal sealed class ServiceContractLicenceConfiguration : ServiceContractLicenceConfigurationBase
{
    protected override string KeyName => "pk_service_contract_licence";

    protected override string ServiceContractFK => "fk_service_contract_licence_service_contract";

    protected override string SoftwareModelFK => "fk_service_contract_licence_software_model";

    protected override string ProductCatalogTypeFK => "fk_service_contract_licence_product_catalog_type_id";

    protected override string SoftwareLicenceFK => "fk_service_contract_licence_software_licence_id";

    protected override string IndexByProductCatalogTypeID => "ix_service_contract_licence_product_catalog_type_id";

    protected override string IndexByServiceContractID => "ix_service_contract_licence_service_contract_id";

    protected override string IndexBySoftwareLicenceID => "ix_service_contract_licence_software_licence_id";

    protected override string IndexBySoftwareLicenceModelID => "ix_service_contract_licence_software_licence_model_id";

    protected override void ConfigureDataBase(EntityTypeBuilder<ServiceContractLicence> builder)
    {
        builder.ToTable("service_contract_licence", Options.Scheme);

        builder.Property(e => e.ID).HasColumnName("id");
        builder.Property(e => e.ServiceContractID).HasColumnName("service_contract_id");
        builder.Property(e => e.SoftwareModelID).HasColumnName("software_model_id");
        builder.Property(e => e.LicenceType).HasColumnName("licence_type");
        builder.Property(e => e.LicenceSchemeEnum).HasColumnName("licence_scheme_enum");
        builder.Property(e => e.Count).HasColumnName("count");
        builder.Property(e => e.CanDowngrade).HasColumnName("can_downgrade");
        builder.Property(e => e.IsFull).HasColumnName("is_full");
        builder.Property(e => e.ProductCatalogTypeID).HasColumnName("product_catalog_type_id");
        builder.Property(e => e.SoftwareLicenceModelID).HasColumnName("software_licence_model_id");
        builder.Property(e => e.SoftwareLicenceID).HasColumnName("software_licence_id");
        builder.Property(e => e.Cost).HasColumnName("cost");
        builder.Property(e => e.Name).HasColumnName("name");
        builder.Property(e => e.LimitInDays).HasColumnName("limit_in_days");
        builder.Property(e => e.LicenceScheme).HasColumnName("licence_scheme");
        builder.HasXminRowVersion(e => e.RowVersion);
    }
}
