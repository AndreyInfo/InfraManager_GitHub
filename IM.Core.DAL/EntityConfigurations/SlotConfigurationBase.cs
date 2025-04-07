using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class SlotConfigurationBase : IEntityTypeConfiguration<Slot>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string SlotTypeForeignKey { get; }
    protected abstract string AdapterForeignKey { get; }
    protected abstract string UniqueKeyObjectIDNumber { get; }
    protected abstract string ObjectIDIndexName { get; }

    protected abstract void AdditionalConfig(EntityTypeBuilder<Slot> builder);
    
    public void Configure(EntityTypeBuilder<Slot> builder)
    {
        builder.HasKey(x => new { x.ObjectID, x.Number }).HasName(PrimaryKeyName);

        builder.HasIndex(x => new { x.ObjectID, x.Number }).IsUnique().HasDatabaseName(UniqueKeyObjectIDNumber);
        builder.HasIndex(x => x.ObjectID, ObjectIDIndexName);

        builder.HasOne(x => x.SlotType)
            .WithMany()
            .HasForeignKey(x => x.SlotTypeID)
            .HasConstraintName(SlotTypeForeignKey);

        builder.HasOne(x => x.Adapter)
            .WithMany()
            .HasForeignKey(x => x.AdapterID)
            .HasConstraintName(AdapterForeignKey);

        AdditionalConfig(builder);

    }
}