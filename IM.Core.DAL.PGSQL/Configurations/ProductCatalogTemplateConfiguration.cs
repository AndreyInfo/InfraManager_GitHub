using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations;

internal sealed class ProductCatalogTemplateConfiguration : ProductCatalogTemplateConfigurationBase
{
    protected override string PrimaryKeyName => "pk_product_catalog_template";

    protected override string ParentForeignKeyName => "fk_product_catalog_template_product_catalog_template";

    protected override void ConfigureDatabase(EntityTypeBuilder<ProductCatalogTemplate> builder)
    {
        builder.ToTable("product_catalog_template", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.ParentID).HasColumnName("parent_id");
        builder.Property(x => x.ClassID).HasColumnName("class_id");
    }
}