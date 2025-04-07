using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace IM.Core.DAL.PGSQL.Configurations;

internal sealed class SlotConfiguration : SlotConfigurationBase
{
    protected override string PrimaryKeyName => "pk_slot";

    protected override string SlotTypeForeignKey => "fk_slot_slot_type";

    protected override string AdapterForeignKey => "fk_slot_adapter";

    protected override string UniqueKeyObjectIDNumber => "uk_slot_number";

    protected override string ObjectIDIndexName => "is_slot_object_id";

    protected override void AdditionalConfig(EntityTypeBuilder<Slot> builder)
    {
        builder.ToTable("slot", Options.Scheme);

        builder.Property(x => x.ObjectID).HasColumnName("object_id");
        builder.Property(x => x.ObjectClassID).HasColumnName("object_class_id");
        builder.Property(x => x.Number).HasColumnName("number");
        builder.Property(x => x.SlotTypeID).HasColumnName("slot_type_id");
        builder.Property(x => x.AdapterID).HasColumnName("adapter_id");
        builder.Property(x => x.Note).HasColumnName("note");
    }
}
