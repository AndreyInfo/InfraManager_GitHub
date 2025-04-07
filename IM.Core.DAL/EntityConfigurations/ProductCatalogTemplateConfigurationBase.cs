using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class ProductCatalogTemplateConfigurationBase: IEntityTypeConfiguration<ProductCatalogTemplate>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string ParentForeignKeyName { get; }


    public void Configure(EntityTypeBuilder<ProductCatalogTemplate> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.Property(x => x.Name).IsRequired(true).HasMaxLength(250);

        builder.HasOne(c => c.ParentTemplate)
            .WithMany(c => c.ProductCatalogTemplates)
            .HasForeignKey(x => x.ParentID)
            .HasPrincipalKey(x => x.ID)
            .HasConstraintName(ParentForeignKeyName);
        
        ConfigureDatabase(builder);
    }
    protected abstract void ConfigureDatabase(EntityTypeBuilder<ProductCatalogTemplate> entity);
}