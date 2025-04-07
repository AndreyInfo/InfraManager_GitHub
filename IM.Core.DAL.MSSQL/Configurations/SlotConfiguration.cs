using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using IM.Core.DAL.Microsoft.SqlServer;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class SlotConfiguration : SlotConfigurationBase
{
    protected override string PrimaryKeyName => "PK_Slot";

    protected override string SlotTypeForeignKey => "FK_Slot_SlotType";

    protected override string AdapterForeignKey => "FK_Slot_Adapter";

    protected override string UniqueKeyObjectIDNumber => "UK_SlotTemplate_Number";

    protected override string ObjectIDIndexName => "IX_Slot_ObjectID";

    protected override void AdditionalConfig(EntityTypeBuilder<Slot> builder)
    {
        builder.ToTable("Slot", Options.Scheme);

        builder.Property(x => x.ObjectID).HasColumnName("ObjectID");
        builder.Property(x => x.ObjectClassID).HasColumnName("ObjectClassID");
        builder.Property(x => x.Number).HasColumnName("Number");
        builder.Property(x => x.SlotTypeID).HasColumnName("SlotTypeID");
        builder.Property(x => x.AdapterID).HasColumnName("AdapterID");
        builder.Property(x => x.Note).HasColumnName("Note");
    }
}
