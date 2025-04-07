using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset.Subclasses;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

public class MotherboardConfiguration : MotherboardConfigurationBase
{
    protected override string PrimaryKeyName => "pk_motherboard";

    protected override void ConfigureDatabase(EntityTypeBuilder<Motherboard> builder)
    {
        builder.ToTable("motherboard", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.PrimaryBusType).HasColumnName("primary_bus_type");
        builder.Property(x => x.SecondaryBusType).HasColumnName("secondary_bus_type");
        builder.Property(x => x.ExpansionSlots).HasColumnName("expansion_slots");
        builder.Property(x => x.RamSlots).HasColumnName("ram_slots");
        builder.Property(x => x.MotherboardSize).HasColumnName("motherboard_size");
        builder.Property(x => x.MotherboardChipset).HasColumnName("motherboard_chipset");
        builder.Property(x => x.MaximumSpeed).HasColumnName("maximum_speed");
        builder.Property(e => e.ComplementaryID).HasColumnName("complementary_id");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
    }
}
