using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

internal sealed class ProductCatalogCategoryConfiguration : ProductCatalogCategoryConfigurationBase
{
    protected override string PrimaryKeyName => "pk_product_catalog_category";
    protected override string ParentForeignKey => "fk_product_catalog_category_product_catalog_category";
    protected override string ParentProductCatalogCategoryIDIndexName => "ix_product_catalog_category_parent_product_catalog_category_id";
    protected override string ParentIsNullAndNoRemovedUI => "ui_name_product_catalog_category_parent_null";
    protected override string ParentIDAndNoRemovedUI => "ui_name_product_catalog_category_parent_id";
    protected override string ParentProductCatalogCategoryIDRemovedIndexName => "ix_product_catalog_category_parent_product_catalog_category_id_";

    protected override void ConfigureDatabase(EntityTypeBuilder<ProductCatalogCategory> entity)
    {
        entity.ToTable("product_catalog_category", Options.Scheme);

        entity.Property(e => e.ID).HasColumnName("id");
        entity.Property(e => e.Name).HasColumnName("name");
        entity.Property(e => e.Removed).HasColumnName("removed");
        entity.Property(e => e.IconName).HasColumnName("icon_name");
        entity.Property(e => e.Icon).HasColumnType("bytea").HasColumnName("icon");
        entity.Property(e => e.ParentProductCatalogCategoryID).HasColumnName("parent_product_catalog_category_id");

        entity.HasXminRowVersion(x => x.RowVersion);
    }
}