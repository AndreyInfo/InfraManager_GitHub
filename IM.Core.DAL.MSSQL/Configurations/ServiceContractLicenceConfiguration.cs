using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;

internal sealed class ServiceContractLicenceConfiguration : ServiceContractLicenceConfigurationBase
{
    protected override string KeyName => "PK_ServiceContractLicence";

    protected override string ServiceContractFK => "FK_ServiceContractLicence_ServiceContract";

    protected override string SoftwareModelFK => "FK_ServiceContractLicence_SoftwareModel";

    protected override string ProductCatalogTypeFK => "FK_ServiceContractLicence_ProductCatalogTypeID";

    protected override string SoftwareLicenceFK => "FK_ServiceContractLicence_SoftwareLicenceID";

    protected override string IndexByProductCatalogTypeID => "IX_ServiceContractLicence_ProductCatalogTypeID";

    protected override string IndexByServiceContractID => "IX_ServiceContractLicence_ServiceContractID";

    protected override string IndexBySoftwareLicenceID => "IX_ServiceContractLicence_SoftwareLicenceID";

    protected override string IndexBySoftwareLicenceModelID => "IX_ServiceContractLicence_SoftwareLicenceModelID";

    protected override void ConfigureDataBase(EntityTypeBuilder<ServiceContractLicence> builder)
    {
        builder.ToTable("ServiceContractLicence", Options.Scheme);

        builder.Property(e => e.ID).HasColumnName("ID");
        builder.Property(e => e.ServiceContractID).HasColumnName("ServiceContractID");
        builder.Property(e => e.SoftwareModelID).HasColumnName("SoftwareModelID");
        builder.Property(e => e.LicenceType).HasColumnName("LicenceType");
        builder.Property(e => e.LicenceSchemeEnum).HasColumnName("LicenceSchemeEnum");
        builder.Property(e => e.Count).HasColumnName("Count");
        builder.Property(e => e.CanDowngrade).HasColumnName("CanDowngrade");
        builder.Property(e => e.IsFull).HasColumnName("IsFull");
        builder.Property(e => e.ProductCatalogTypeID).HasColumnName("ProductCatalogTypeID");
        builder.Property(e => e.SoftwareLicenceModelID).HasColumnName("SoftwareLicenceModelID");
        builder.Property(e => e.SoftwareLicenceID).HasColumnName("SoftwareLicenceID");
        builder.Property(e => e.Cost).HasColumnName("Cost");
        builder.Property(e => e.Name).HasColumnName("Name");
        builder.Property(e => e.LimitInDays).HasColumnName("LimitInDays");
        builder.Property(e => e.LicenceScheme).HasColumnName("LicenceScheme");
        builder.Property(e => e.RowVersion)
            .IsRequired()
            .IsRowVersion()
            .HasColumnType("timestamp")
            .HasColumnName("RowVersion"); ;
    }
}
