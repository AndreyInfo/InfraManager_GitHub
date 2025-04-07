using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;
internal sealed class SlotTemplateConfiguration : SlotTemplateConfigurationBase
{
    protected override string PrimaryKey => "pk_slot_template";

    protected override string SlotTypeForeignKey => "fk_slot_template_slot_type";

    protected override string UniqueKeyObjectIDNumber => "uk_slot_template_number";

    protected override string ObjectIDIndexName => "ix_slot_template_object_id";

    protected override void ConfigureDatabase(EntityTypeBuilder<SlotTemplate> builder)
    {
        builder.ToTable("slot_template", Options.Scheme);

        builder.Property(x => x.ObjectID).HasColumnName("object_id");
        builder.Property(x => x.ObjectClassID).HasColumnName("object_class_id");
        builder.Property(x => x.Number).HasColumnName("number");
        builder.Property(x => x.SlotTypeID).HasColumnName("slot_type_id");
    }
}
