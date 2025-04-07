using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;

public class SoftwareLicenseModelConfiguration : SoftwareLicenseModelConfigurationBase
{
    protected override string PrimaryKeyName => "pk_software_licence_model";

    protected override string RemovedIX => "ix_software_licence_model_removed";
    protected override string ManufacturerIDIX => "ix_software_licence_model_manufacturer_id";
    protected override string SoftwareModelIDIX => "ix_software_licence_model_software_model_id";
    protected override string ProductCatalogTypeIDIX => "ix_software_licence_model_product_catalog_type_id";

    protected override string ProductCatalogTypeIDFK => "fk_software_licence_model_product_catalog_type";

    protected override void ConfigureDatabase(EntityTypeBuilder<SoftwareLicenseModel> builder)
    {
        builder.ToTable("software_licence_model", Options.Scheme);

        builder.Property(x => x.IMObjID).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Note).HasColumnName("note");
        builder.Property(x => x.Code).HasColumnName("code");
        builder.Property(x => x.IsFull).HasColumnName("is_full");
        builder.Property(x => x.CanBuy).HasColumnName("can_buy");
        builder.Property(x => x.Removed).HasColumnName("removed");
        builder.Property(x => x.ExternalID).HasColumnName("external_id");
        builder.Property(x => x.ManufacturerID).HasColumnName("manufacturer_id");
        builder.Property(x => x.ProductNumber).HasColumnName("product_number");
        builder.Property(x => x.LimitInHours).HasColumnName("limit_in_hours");
        builder.Property(x => x.SoftwareModelID).HasColumnName("software_model_id");
        builder.Property(x => x.DowngradeAvailable).HasColumnName("downgrade_available");
        builder.Property(x => x.SoftwareLicenceTypeID).HasColumnName("software_licence_type");
        builder.Property(x => x.ProductCatalogTypeID).HasColumnName("product_catalog_type_id");
        builder.Property(x => x.SoftwareLicenseScheme).HasColumnName("software_licence_scheme");
        builder.Property(x => x.SoftwareExecutionCount).HasColumnName("software_execution_count");
        builder.Property(x => x.SoftwareLicenceSchemeEnum).HasColumnName("software_licence_scheme_enum");
        builder.Property(x => x.SoftwareExecutionCountIsDefined).HasColumnName("software_execution_count_is_defined");
        
        builder.HasXminRowVersion(x => x.RowVersion);
    }
}

