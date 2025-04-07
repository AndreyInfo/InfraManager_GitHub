using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class SoftwareLicenseModelConfigurationBase : IEntityTypeConfiguration<SoftwareLicenseModel>
{
    protected abstract string PrimaryKeyName { get; }

    protected abstract string RemovedIX { get; }
    protected abstract string ManufacturerIDIX { get; }
    protected abstract string SoftwareModelIDIX { get; }
    protected abstract string ProductCatalogTypeIDIX { get; }

    protected abstract string ProductCatalogTypeIDFK { get; }

    public void Configure(EntityTypeBuilder<SoftwareLicenseModel> builder)
    {
        builder.HasKey(x => x.IMObjID).HasName(PrimaryKeyName);


        builder.HasIndex(c => c.Removed, RemovedIX);
        builder.HasIndex(c => c.ManufacturerID, ManufacturerIDIX);
        builder.HasIndex(c => c.SoftwareModelID, SoftwareModelIDIX);
        builder.HasIndex(c => c.ProductCatalogTypeID, ProductCatalogTypeIDIX);


        builder.Property(x => x.Name)
            .HasMaxLength(500)
            .IsRequired(true);

        builder.Property(x => x.ExternalID)
            .HasMaxLength(250)
            .IsRequired(true);

        builder.Property(x => x.Code)
            .HasMaxLength(50)
            .IsRequired(true);

        builder.Property(x => x.Note)
            .HasMaxLength(255)
            .IsRequired(true);

        builder.Property(x => x.ProductNumber)
            .HasMaxLength(250)
            .IsRequired(true);


        builder.HasOne(x => x.ProductCatalogType)
            .WithMany()
            .HasForeignKey(x => x.ProductCatalogTypeID)
            .HasConstraintName(ProductCatalogTypeIDFK);

        //TODO добавить FK
        builder.HasOne(x => x.Manufacturer)
            .WithMany()
            .HasForeignKey(x => x.ManufacturerID)
            .HasPrincipalKey(x => x.ImObjID);

        ConfigureDatabase(builder);
    }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<SoftwareLicenseModel> builder);

}

