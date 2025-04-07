using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

internal sealed class ServiceContractModelConfiguration : ServiceContractModelConfigurationBase
{
    protected override string PrimaryKeyName => "pk_service_contract_model";
    protected override string ProductCatalogTypeForeignKeyName => "fk_service_contract_model_product_catalog_type";

    protected override void ConfigureDatabase(EntityTypeBuilder<ServiceContractModel> builder)
    {
        builder.ToTable("service_contract_model", Options.Scheme);
        
        builder.Property(x => x.IMObjID).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Note).HasColumnName("note");
        builder.Property(x => x.CanBuy).HasColumnName("can_buy");
        builder.Property(x => x.Removed).HasColumnName("removed");
        builder.Property(x => x.ExternalID).HasColumnName("external_id");
        builder.Property(x => x.ManufacturerID).HasColumnName("manufacturer_id");
        builder.Property(x => x.ContractSubject).HasColumnName("contract_subject");
        builder.Property(x => x.UpdateAvailable).HasColumnName("update_available");
        builder.Property(x => x.ProductCatalogTypeID).HasColumnName("product_catalog_type_id");

        builder.HasXminRowVersion(x => x.RowVersion);
    }
}