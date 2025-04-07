using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class ProductCatalogTypeConfiguration : ProductCatalogTypeConfigurationBase
{
    protected override string PrimaryKeyName => "PK_ProductCatalogType";
    protected override string DefaultValueID => "NEWID()";
    protected override string LifeCycleForeignKeyName => "FK_ProductCatalogType_LifeCycle";
    protected override string ProductCatalogFormForeignKeyName => "FK_ProductCatalogType_FormID";
    protected override string ProductCatalogCategoryForeignKeyName => "FK_ProductCatalogType_ProductCatalogCategory";
    protected override string ProductCatalogTemplateForeignKeyName => "FK_ProductCatalogType_ProductCatalogTemplate";
    protected override string LifeCycleIDIndexName => "IX_ProductCatalogType_LifeCycleID";
    protected override string ProductCatalogCategoryIDIndexName => "IX_ProductCatalogType_ProductCatalogCategoryID";
    protected override string ProductCatalogCategoryIDRemovedIndexName => "IX_ProductCatalogType_ProductCatalogCategoryID_Removed";
    protected override string NameProductCatalogCategoryIDIndexName => "UI_Name_ProductCatalogType_CategoryID";


    protected override void ConfigureDatabase(EntityTypeBuilder<ProductCatalogType> entity)
    {
        entity.ToTable("ProductCatalogType", Options.Scheme);
        
        entity.Property(e => e.IMObjID).HasColumnName("ID");
        entity.Property(e => e.Name).HasColumnName("Name");
        entity.Property(e => e.CanBuy).HasColumnName("CanBuy");
        entity.Property(e => e.FormID).HasColumnName("FormID");
        entity.Property(e => e.Removed).HasColumnName("Removed");
        entity.Property(e => e.IconName).HasColumnType("IconName");
        entity.Property(e => e.IsLogical).HasColumnName("IsLogical");
        entity.Property(e => e.LifeCycleID).HasColumnName("LifeCycleID");
        entity.Property(e => e.ExternalID).HasColumnName("ExternalCode");
        entity.Property(e => e.ExternalName).HasColumnName("ExternalName");
        entity.Property(e => e.Icon).HasColumnType("image").HasColumnName("Icon");
        entity.Property(e => e.IsAccountingAsset).HasColumnName("IsAccountingAsset");
        entity.Property(e => e.ProductCatalogCategoryID).HasColumnName("ProductCatalogCategoryID");
        entity.Property(e => e.ProductCatalogTemplateID).HasColumnName("ProductCatalogTemplateID");
        
        entity.Property(e => e.RowVersion).HasColumnName("RowVersion").IsRowVersion();
    }
}
