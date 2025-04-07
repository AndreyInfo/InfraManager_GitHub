using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class ServiceContractFeatureConfigurationBase : IEntityTypeConfiguration<ServiceContractFeature>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string ProductCatalogTypeForeignKey { get; }

    public void Configure(EntityTypeBuilder<ServiceContractFeature> builder)
    {
        builder.HasKey(c=> new { c.ProductCatalogTypeID, c.Feature} ).HasName(PrimaryKeyName);

        builder.Property(c=> c.ProductCatalogTypeID).ValueGeneratedOnAdd();

        builder.HasOne(c=> c.ProductCatalogType)
            .WithMany(c => c.ServiceContractFeatures)
            .HasForeignKey(c => c.ProductCatalogTypeID)
            .HasConstraintName(ProductCatalogTypeForeignKey)
            .OnDelete(DeleteBehavior.Cascade);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<ServiceContractFeature> builder);
}
