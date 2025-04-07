using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class MaterialConfigurationBase : IEntityTypeConfiguration<Material>
{
    protected abstract string KeyName { get; }
    protected abstract string StorageLocationFK { get; }
    protected abstract string MaterialModelFK { get; }
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.HasKey(c => c.MaterialID).HasName(KeyName);

        builder.Property(c => c.DeviceID).HasMaxLength(255);
        builder.Property(c => c.Device).HasMaxLength(50);
        builder.Property(c => c.Note).HasMaxLength(500);
        builder.Property(c => c.Document).HasMaxLength(50);

        builder.HasOne(c => c.StorageLocation)
            .WithMany()
            .HasForeignKey(c => c.StorageLocationID)
            .HasConstraintName(StorageLocationFK);

        builder.HasOne(c => c.Model)
            .WithMany()
            .HasForeignKey(c => c.MaterialModelID)
            .HasConstraintName(MaterialModelFK);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<Material> builder);
}
