using InfraManager.DAL.ConfigurationUnits;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class ConfigurationUnitBaseConfigurationBase : IEntityTypeConfiguration<ConfigurationUnitBase>
{
    protected abstract string KeyName { get; }
    protected abstract string CriticalityFK { get; }
    protected abstract string InfrastructureSegmentFK { get; }
    protected abstract string LifeCycleStateFK { get; }
    protected abstract string ProductCatalogTypeFK { get; }

    public void Configure(EntityTypeBuilder<ConfigurationUnitBase> builder)
    {
        builder.HasKey(e => e.ID).HasName(KeyName);

        builder.Property(e => e.ID);
        builder.Property(e => e.Number).IsRequired(true).ValueGeneratedOnAdd();
        builder.Property(e => e.Name).HasMaxLength(250).IsRequired(true);
        builder.Property(e => e.RowVersion).IsRequired(true);

        builder.HasOne(d => d.Criticality)
            .WithMany()
            .HasForeignKey(d => d.CriticalityID)
            .HasConstraintName(CriticalityFK);

        builder.HasOne(d => d.InfrastructureSegment)
            .WithMany()
            .HasForeignKey(d => d.InfrastructureSegmentID)
            .HasConstraintName(InfrastructureSegmentFK);

        builder.HasOne(d => d.LifeCycleState)
            .WithMany()
            .HasForeignKey(d => d.LifeCycleStateID)
            .HasConstraintName(LifeCycleStateFK);

        builder.HasOne(d => d.Type)
            .WithMany()
            .IsRequired(true)
            .HasForeignKey(d => d.ProductCatalogTypeID)
            .HasConstraintName(ProductCatalogTypeFK);

        ConfigureDataBase(builder);
    }
    protected abstract void ConfigureDataBase(EntityTypeBuilder<ConfigurationUnitBase> builder);
}
