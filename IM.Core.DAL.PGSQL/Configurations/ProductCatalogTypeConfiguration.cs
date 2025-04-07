using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

internal sealed class ProductCatalogTypeConfiguration : ProductCatalogTypeConfigurationBase
{
    protected override string PrimaryKeyName => "pk_product_catalog_type";
    protected override string DefaultValueID => "gen_random_uuid()";
    protected override string LifeCycleForeignKeyName => "fk_product_catalog_type_life_cycle";
    protected override string ProductCatalogFormForeignKeyName => "product_catalog_type_form_fk";
    protected override string ProductCatalogCategoryForeignKeyName => "fk_product_catalog_type_product_catalog_category";
    protected override string ProductCatalogTemplateForeignKeyName => "fk_product_catalog_type_product_catalog_template";
    protected override string LifeCycleIDIndexName => "ix_product_catalog_type_life_cycle_id";
    protected override string ProductCatalogCategoryIDIndexName => "ix_product_catalog_type_product_catalog_category_id";
    protected override string ProductCatalogCategoryIDRemovedIndexName => "ix_product_catalog_type_product_catalog_category_id_removed";
    protected override string NameProductCatalogCategoryIDIndexName => "ui_name_product_catalog_type_category_id";


    protected override void ConfigureDatabase(EntityTypeBuilder<ProductCatalogType> entity)
    {
        entity.ToTable("product_catalog_type", Options.Scheme);

        entity.Property(e => e.IMObjID).HasColumnName("id");
        entity.Property(e => e.Name).HasColumnName("name");
        entity.Property(e => e.CanBuy).HasColumnName("can_buy");
        entity.Property(e => e.FormID).HasColumnName("form_id");
        entity.Property(e => e.Removed).HasColumnName("removed");
        entity.Property(e => e.IconName).HasColumnName("icon_name");
        entity.Property(e => e.IsLogical).HasColumnName("is_logical");
        entity.Property(e => e.ExternalID).HasColumnName("external_code");
        entity.Property(e => e.LifeCycleID).HasColumnName("life_cycle_id");
        entity.Property(e => e.ExternalName).HasColumnName("external_name");
        entity.Property(e => e.Icon).HasColumnType("bytea").HasColumnName("icon");
        entity.Property(e => e.IsAccountingAsset).HasColumnName("is_accounting_asset");
        entity.Property(e => e.ProductCatalogTemplateID).HasColumnName("product_catalog_template_id");
        entity.Property(e => e.ProductCatalogCategoryID).HasColumnName("product_catalog_category_id");

        entity.HasXminRowVersion(e => e.RowVersion);
    }
}