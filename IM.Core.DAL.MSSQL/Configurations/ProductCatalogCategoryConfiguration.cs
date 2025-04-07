using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class ProductCatalogCategoryConfiguration : ProductCatalogCategoryConfigurationBase
{
    protected override string PrimaryKeyName => "PK_ProductCatalogCategory";
    protected override string ParentForeignKey => "FK_ProductCatalogCategory_ProductCatalogCategory";
    protected override string ParentProductCatalogCategoryIDIndexName => "IX_ProductCatalogCategory_ParentProductCatalogCategoryID";
    protected override string ParentIsNullAndNoRemovedUI => "UI_Name_ProductCatalogCategory_Parent_IS_NULL";
    protected override string ParentIDAndNoRemovedUI => "UI_Name_ProductCatalogCategory_ParentID";
    protected override string ParentProductCatalogCategoryIDRemovedIndexName => "IX_ProductCatalogCategory_ParentProductCatalogCategoryID_Removed";

    protected override void ConfigureDatabase(EntityTypeBuilder<ProductCatalogCategory> entity)
    {
        entity.ToTable("ProductCatalogCategory", Options.Scheme);
       
        entity.Property(e => e.ID).HasColumnName("ID");
        entity.Property(e => e.Name).HasColumnName("Name");
        entity.Property(e => e.Removed).HasColumnName("Removed");
        entity.Property(e => e.IconName).HasColumnName("IconName");
        entity.Property(e => e.Icon).HasColumnType("image").HasColumnName("Icon");
        entity.Property(e => e.ParentProductCatalogCategoryID).HasColumnName("ParentProductCatalogCategoryID");

        entity.Property(e => e.RowVersion).HasColumnName("RowVersion").IsRowVersion();
    }
}
