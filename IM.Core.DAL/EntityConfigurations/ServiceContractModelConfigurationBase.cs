using System;
using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class ServiceContractModelConfigurationBase : IEntityTypeConfiguration<ServiceContractModel>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string ProductCatalogTypeForeignKeyName { get; }


    public void Configure(EntityTypeBuilder<ServiceContractModel> builder)
    {
        builder.HasKey(x => x.IMObjID).HasName(PrimaryKeyName);

        builder.Property(x => x.Name).IsRequired(true).HasMaxLength(500);
        builder.Property(x => x.ContractSubject).IsRequired(true).HasMaxLength(500);
        builder.Property(x => x.Note).IsRequired(true).HasMaxLength(1000);
        builder.Property(x => x.ExternalID).IsRequired(true).HasMaxLength(500);

        builder.IsMarkableForDelete();
        builder.HasOne(x => x.ProductCatalogType)
            .WithMany()
            .HasForeignKey(x => x.ProductCatalogTypeID)
            .HasPrincipalKey(x => x.IMObjID)
            .HasConstraintName(ProductCatalogTypeForeignKeyName);
        
        ConfigureDatabase(builder);
    }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<ServiceContractModel> builder);

}