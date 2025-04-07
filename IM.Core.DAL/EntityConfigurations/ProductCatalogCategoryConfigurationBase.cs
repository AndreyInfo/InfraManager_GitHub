using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class ProductCatalogCategoryConfigurationBase : IEntityTypeConfiguration<ProductCatalogCategory>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string ParentForeignKey { get; }
    protected abstract string ParentProductCatalogCategoryIDIndexName { get; }
    protected abstract string ParentProductCatalogCategoryIDRemovedIndexName { get; }
    protected abstract string ParentIsNullAndNoRemovedUI { get; }
    protected abstract string ParentIDAndNoRemovedUI { get; }


    public void Configure(EntityTypeBuilder<ProductCatalogCategory> entity)
    {
        entity.HasKey(x => x.ID).HasName(PrimaryKeyName);

        entity.IsMarkableForDelete();

        entity.HasIndex(x => x.ParentProductCatalogCategoryID, ParentProductCatalogCategoryIDIndexName);
        entity.HasIndex(x => new { x.ParentProductCatalogCategoryID, x.Removed }, ParentProductCatalogCategoryIDRemovedIndexName);
        entity.HasIndex(x => x.Name, ParentIsNullAndNoRemovedUI).IsUnique();
        entity.HasIndex(x => new { x.Name, x.ParentProductCatalogCategoryID }, ParentIDAndNoRemovedUI).IsUnique();

        entity.Property(e => e.ID).ValueGeneratedNever();

        entity.Property(e => e.Name)
            .HasMaxLength(500)
            .IsRequired(true);

        //TODO OnDelete => Cascade
        entity.HasOne(d => d.ParentProductCatalogCategory)
            .WithMany(x => x.SubCategories)
            .HasForeignKey(d => d.ParentProductCatalogCategoryID)
            .HasPrincipalKey(x => x.ID)
            .HasConstraintName(ParentForeignKey);

        ConfigureDatabase(entity);
    }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<ProductCatalogCategory> entity);

}