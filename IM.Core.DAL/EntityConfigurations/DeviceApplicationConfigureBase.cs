using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;
public abstract class DeviceApplicationConfigureBase : IEntityTypeConfiguration<DeviceApplication>
{
    protected abstract string PrimaryKeyName { get; } 
    protected abstract string ProductCatalogTypeForeignKey { get; } 
    protected abstract string LifeCycleStateForeignKey { get; } 
    protected abstract string CriticalityForeignKey { get; } 
    protected abstract string InfrastructureSegmentForeignKey { get; } 
    public void Configure(EntityTypeBuilder<DeviceApplication> builder)
    {
        builder.HasKey(c=> c.ID).HasName(PrimaryKeyName);

        builder.Property(c=> c.Name).IsRequired(true).HasMaxLength(250);
        builder.Property(c=> c.Note).IsRequired(true).HasMaxLength(500);

        builder.HasOne(c=> c.ProductCatalogType)
            .WithMany()
            .HasForeignKey(c=> c.ProductCatalogTypeID)
            .HasConstraintName(ProductCatalogTypeForeignKey)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.LifeCycleState)
            .WithMany()
            .HasForeignKey(c => c.LifeCycleStateID)
            .HasConstraintName(LifeCycleStateForeignKey)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.Criticality)
            .WithMany()
            .HasForeignKey(c => c.CriticalityID)
            .HasConstraintName(CriticalityForeignKey)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.InfrastructureSegment)
            .WithMany()
            .HasForeignKey(c => c.InfrastructureSegmentID)
            .HasConstraintName(InfrastructureSegmentForeignKey)
            .OnDelete(DeleteBehavior.NoAction);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<DeviceApplication> builder);
}
