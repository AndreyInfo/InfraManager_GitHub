using InfraManager.DAL.Asset.Subclasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class MotherboardConfigurationBase : IEntityTypeConfiguration<Motherboard>
{
    protected abstract string PrimaryKeyName { get; }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<Motherboard> builder);

    public void Configure(EntityTypeBuilder<Motherboard> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.Property(x => x.PrimaryBusType).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.SecondaryBusType).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.ExpansionSlots).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.RamSlots).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.MotherboardSize).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.MotherboardChipset).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.MaximumSpeed).IsRequired(false).HasMaxLength(255);
        builder.Property(e => e.ComplementaryID).IsRequired(false);
        builder.Property(e => e.PeripheralDatabaseID).IsRequired(false);

        ConfigureDatabase(builder);
    }
}