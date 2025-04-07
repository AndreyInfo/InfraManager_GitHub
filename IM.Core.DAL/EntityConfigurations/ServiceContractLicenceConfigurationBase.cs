using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class ServiceContractLicenceConfigurationBase : IEntityTypeConfiguration<ServiceContractLicence>
{
    protected abstract string KeyName { get; }
    protected abstract string ServiceContractFK { get; }
    protected abstract string SoftwareModelFK { get; }
    protected abstract string ProductCatalogTypeFK { get; }
    protected abstract string SoftwareLicenceFK { get; }
    protected abstract string IndexByProductCatalogTypeID { get; }
    protected abstract string IndexByServiceContractID { get; }
    protected abstract string IndexBySoftwareLicenceID { get; }
    protected abstract string IndexBySoftwareLicenceModelID { get; }

    public void Configure(EntityTypeBuilder<ServiceContractLicence> builder)
    {
        builder.HasKey(e => e.ID).HasName(KeyName);

        builder.HasIndex(e => e.ProductCatalogTypeID, IndexByProductCatalogTypeID);
        builder.HasIndex(e => e.ServiceContractID, IndexByServiceContractID);
        builder.HasIndex(e => e.SoftwareLicenceID, IndexBySoftwareLicenceID);
        builder.HasIndex(e => e.SoftwareLicenceModelID, IndexBySoftwareLicenceModelID);

        builder.Property(e => e.Name).IsRequired(true).HasMaxLength(250);

        builder.HasOne(d => d.ServiceContract)
            .WithMany()
            .HasForeignKey(d => d.ServiceContractID)
            .HasConstraintName(ServiceContractFK);

        builder.HasOne(d => d.SoftwareModel)
            .WithMany()
            .HasForeignKey(d => d.SoftwareModelID)
            .HasConstraintName(SoftwareModelFK);

        builder.HasOne(d => d.ProductCatalogType)
            .WithMany()
            .HasForeignKey(d => d.ProductCatalogTypeID)
            .HasConstraintName(ProductCatalogTypeFK);

        builder.HasOne(d => d.SoftwareLicence)
            .WithMany()
            .HasForeignKey(d => d.SoftwareLicenceID)
            .HasConstraintName(SoftwareLicenceFK);

        ConfigureDataBase(builder);
    }
    protected abstract void ConfigureDataBase(EntityTypeBuilder<ServiceContractLicence> builder);

}
