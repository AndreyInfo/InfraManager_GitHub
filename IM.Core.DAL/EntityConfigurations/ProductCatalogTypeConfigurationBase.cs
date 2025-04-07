using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class ProductCatalogTypeConfigurationBase : IEntityTypeConfiguration<ProductCatalogType>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string LifeCycleForeignKeyName { get; }
    protected abstract string ProductCatalogCategoryForeignKeyName { get; }
    protected abstract string ProductCatalogTemplateForeignKeyName { get; }
    protected abstract string ProductCatalogFormForeignKeyName { get; }
    protected abstract string ProductCatalogCategoryIDIndexName { get; }
    protected abstract string LifeCycleIDIndexName { get; }
    protected abstract string ProductCatalogCategoryIDRemovedIndexName { get; }
    protected abstract string NameProductCatalogCategoryIDIndexName { get; }
    protected abstract string DefaultValueID { get; }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<ProductCatalogType> entity);
    
    public void Configure(EntityTypeBuilder<ProductCatalogType> entity)
    {
        entity.HasKey(x => x.IMObjID).HasName(PrimaryKeyName);

        entity.HasIndex(x => x.LifeCycleID, LifeCycleIDIndexName);
        entity.HasIndex(e => e.ProductCatalogCategoryID, ProductCatalogCategoryIDIndexName);
        entity.HasIndex(e => new { e.ProductCatalogCategoryID, e.Removed }, ProductCatalogCategoryIDRemovedIndexName);
        entity.HasIndex(x => new { x.Name, x.ProductCatalogCategoryID, }, NameProductCatalogCategoryIDIndexName).IsUnique();

        entity.Property(e => e.IMObjID).HasDefaultValueSql(DefaultValueID);
        entity.Property(e => e.Name).IsRequired(true).HasMaxLength(500);
        entity.Property(e => e.ExternalID).IsRequired(true).HasMaxLength(250);
        entity.Property(e => e.ExternalName).IsRequired(true).HasMaxLength(250);
        entity.Property(e => e.IconName).IsRequired(false).HasMaxLength(50);

        entity.IsMarkableForDelete();

        entity.HasOne(d => d.LifeCycle)
            .WithMany(p => p.ProductCatalogType)
            .HasForeignKey(d => d.LifeCycleID)
            .HasPrincipalKey(x => x.ID)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName(LifeCycleForeignKeyName);

        entity.HasOne(d => d.ProductCatalogCategory)
            .WithMany(p => p.ProductCatalogTypes)
            .HasForeignKey(d => d.ProductCatalogCategoryID)
            .HasPrincipalKey(x => x.ID)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName(ProductCatalogCategoryForeignKeyName);

        entity.HasOne(d => d.ProductCatalogTemplate)
            .WithMany(p => p.ProductCatalogTypes)
            .HasForeignKey(d => d.ProductCatalogTemplateID)
            .HasPrincipalKey(x => x.ID)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName(ProductCatalogTemplateForeignKeyName);

        entity.HasOne(d => d.Form)
            .WithMany()
            .HasForeignKey(d => d.FormID)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName(ProductCatalogFormForeignKeyName);

        ConfigureDatabase(entity);
    }
}