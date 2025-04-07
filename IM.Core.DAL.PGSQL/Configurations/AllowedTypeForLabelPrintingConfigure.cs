using InfraManager.DAL.Asset.DeviceMonitors;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;

internal sealed class AllowedTypeForLabelPrintingConfigure : AllowedTypeForLabelPrintingConfigureBase
{
    protected override string PrimaryKeyName => "pk_allowed_types_for_label_printing";

    protected override void ConfigureDataBase(EntityTypeBuilder<AllowedTypeForLabelPrinting> builder)
    {
        builder.ToTable("allowed_type_for_label_printing", Options.Scheme);

        builder.Property(c => c.ID).HasColumnName("id");
        builder.Property(c => c.ObjectID).HasColumnName("object_id");
        builder.Property(c => c.ClassID).HasColumnName("class_id");
        builder.Property(c => c.Name).HasColumnName("name");
    }
}
