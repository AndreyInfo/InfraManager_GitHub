using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;
internal sealed class SoftwareLicenseModelConfiguration : SoftwareLicenseModelConfigurationBase
{
    protected override string PrimaryKeyName => "PK_SoftwareLicenseModel";
    
    protected override string RemovedIX => "IX_SoftwareLicenceModel_Removed";
    protected override string ManufacturerIDIX => "IX_SoftwareLicenceModel_ManufacturerID";
    protected override string SoftwareModelIDIX => "IX_SoftwareLicenceModel_SoftwareModelID";
    protected override string ProductCatalogTypeIDIX => "IX_SoftwareLicenceModel_ProductCatalogTypeID";

    protected override string ProductCatalogTypeIDFK => "FK_SoftwareLicenceModel_ProductCatalogType";

    protected override void ConfigureDatabase(EntityTypeBuilder<SoftwareLicenseModel> builder)
    {
        builder.ToTable("SoftwareLicenceModel", Options.Scheme);

        builder.Property(x => x.IMObjID).HasColumnName("ID");
        builder.Property(x => x.Name).HasColumnName("Name");
        builder.Property(x => x.Note).HasColumnName("Note");
        builder.Property(x => x.Code).HasColumnName("Code");
        builder.Property(x => x.CanBuy).HasColumnName("CanBuy");
        builder.Property(x => x.IsFull).HasColumnName("IsFull");
        builder.Property(x => x.Removed).HasColumnName("Removed");
        builder.Property(x => x.ExternalID).HasColumnName("ExternalID");
        builder.Property(x => x.LimitInHours).HasColumnName("LimitInHours");
        builder.Property(x => x.ProductNumber).HasColumnName("ProductNumber");
        builder.Property(x => x.ManufacturerID).HasColumnName("ManufacturerID");
        builder.Property(x => x.SoftwareModelID).HasColumnName("SoftwareModelID");
        builder.Property(x => x.ProductCatalogTypeID).HasColumnName("ProductCatalogTypeID");
        builder.Property(x => x.DowngradeAvailable).HasColumnName("DowngradeAvailable");
        builder.Property(x => x.SoftwareLicenceTypeID).HasColumnName("SoftwareLicenceType");
        builder.Property(x => x.SoftwareLicenseScheme).HasColumnName("SoftwareLicenceScheme");
        builder.Property(x => x.SoftwareExecutionCount).HasColumnName("SoftwareExecutionCount");
        builder.Property(x => x.SoftwareLicenceSchemeEnum).HasColumnName("SoftwareLicenceSchemeEnum");
        builder.Property(x => x.SoftwareExecutionCountIsDefined).HasColumnName("SoftwareExecutionCountIsDefined");

        builder.Property(x => x.RowVersion)
            .IsRowVersion()
            .HasColumnName("RowVersion");
    }
}

