using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class ProductCatalogTemplateConfiguration : ProductCatalogTemplateConfigurationBase
{
    protected override string PrimaryKeyName => "PK_ProductCatalogTemplate";
    protected override string ParentForeignKeyName => "FK_ProductCatalogTemplate_ProductCatalogTemplate";

    protected override void ConfigureDatabase(EntityTypeBuilder<ProductCatalogTemplate> builder)
    {
        builder.ToTable("ProductCatalogTemplate", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.Name).HasColumnName("Name");
        builder.Property(x => x.ParentID).HasColumnName("ParentID");
        builder.Property(x => x.ClassID).HasColumnName("ClassID");
    }
}
