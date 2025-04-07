using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.Asset.Subclasses;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

public class MotherboardConfiguration : MotherboardConfigurationBase
{
    protected override string PrimaryKeyName => "PK_Motherboard";

    protected override void ConfigureDatabase(EntityTypeBuilder<Motherboard> builder)
    {
        builder.ToTable("Motherboard", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID").HasDefaultValueSql("newid()");

        builder.Property(x => x.PrimaryBusType).HasColumnName("PrimaryBusType");
        builder.Property(x => x.SecondaryBusType).HasColumnName("SecondaryBusType");
        builder.Property(x => x.ExpansionSlots).HasColumnName("ExpansionSlots");
        builder.Property(x => x.RamSlots).HasColumnName("RAMSlots");
        builder.Property(x => x.MotherboardSize).HasColumnName("MotherboardSize");
        builder.Property(x => x.MotherboardChipset).HasColumnName("MotherboardChipset");
        builder.Property(x => x.MaximumSpeed).HasColumnName("MaximumSpeed");
        builder.Property(e => e.ComplementaryID).HasColumnName("ComplementaryID");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
    }
}
