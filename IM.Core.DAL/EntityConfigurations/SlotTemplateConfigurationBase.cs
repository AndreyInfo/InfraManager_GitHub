using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;
public abstract class SlotTemplateConfigurationBase : IEntityTypeConfiguration<SlotTemplate>
{
    protected abstract string PrimaryKey { get; }
    protected abstract string SlotTypeForeignKey { get; }
    protected abstract string UniqueKeyObjectIDNumber { get; }
    protected abstract string ObjectIDIndexName { get; }

    public void Configure(EntityTypeBuilder<SlotTemplate> builder)
    {
        builder.HasKey(x => new { x.ObjectID, x.Number }).HasName(PrimaryKey);

        builder.HasIndex(x => new {x.ObjectID, x.Number}).IsUnique().HasDatabaseName(UniqueKeyObjectIDNumber);
        builder.HasIndex(x => x.ObjectID, ObjectIDIndexName);

        builder.HasOne(x => x.SlotType)
            .WithMany()
            .HasForeignKey(x => x.SlotTypeID)
            .HasConstraintName(SlotTypeForeignKey);

        ConfigureDatabase(builder);
    }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<SlotTemplate> builder);
}
