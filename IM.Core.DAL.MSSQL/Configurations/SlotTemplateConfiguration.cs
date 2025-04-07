using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;
internal sealed class SlotTemplateConfiguration : SlotTemplateConfigurationBase
{
    protected override string PrimaryKey => "PK_SlotTemplate";

    protected override string SlotTypeForeignKey => "FK_SlotTemplate_SlotType";

    protected override string UniqueKeyObjectIDNumber => "UK_SlotTemplate_Number";

    protected override string ObjectIDIndexName => "IX_SlotTemplate_ObjectID";

    protected override void ConfigureDatabase(EntityTypeBuilder<SlotTemplate> builder)
    {
        builder.ToTable("SlotTemplate", Options.Scheme);

        builder.Property(x => x.ObjectID).HasColumnName("ObjectID");
        builder.Property(x => x.ObjectClassID).HasColumnName("ObjectClassID");
        builder.Property(x => x.Number).HasColumnName("Number");
        builder.Property(x => x.SlotTypeID).HasColumnName("SlotTypeID");
    }
}
