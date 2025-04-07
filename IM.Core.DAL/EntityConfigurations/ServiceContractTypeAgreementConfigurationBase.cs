using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class ServiceContractTypeAgreementConfigurationBase : IEntityTypeConfiguration<ServiceContractTypeAgreement>
{
    protected abstract string PrimaryKey { get; }
    protected abstract string ProductCatalogForeignKey { get; }
    protected abstract string AgreementLifeCycleForeignKey { get; }
    public void Configure(EntityTypeBuilder<ServiceContractTypeAgreement> builder)
    {
        builder.HasKey(c=> new { c.ProductCatalogTypeID, c.AgreementLifeCycleID }).HasName(PrimaryKey);

        builder.Property(c=> c.ProductCatalogTypeID).ValueGeneratedOnAdd();

        builder.HasOne(c=> c.LifeCycle)
            .WithMany()
            .HasForeignKey(c=> c.AgreementLifeCycleID)
            .HasConstraintName(AgreementLifeCycleForeignKey)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.ProductCatalogType)
            .WithOne(c => c.ServiceContractTypeAgreement)
            .HasForeignKey<ServiceContractTypeAgreement>(c => c.ProductCatalogTypeID)
            .HasConstraintName(ProductCatalogForeignKey)
            .OnDelete(DeleteBehavior.Cascade);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<ServiceContractTypeAgreement> builder);
}
