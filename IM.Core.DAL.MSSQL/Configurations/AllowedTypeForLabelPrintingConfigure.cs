using InfraManager.DAL.Asset.DeviceMonitors;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;

internal sealed class AllowedTypeForLabelPrintingConfigure : AllowedTypeForLabelPrintingConfigureBase
{
    protected override string PrimaryKeyName => "PK_AllowedTypesForLabelPrinting";

    protected override void ConfigureDataBase(EntityTypeBuilder<AllowedTypeForLabelPrinting> builder)
    {
        builder.ToTable("AllowedTypeForLabelPrinting", Options.Scheme);

        builder.Property(c => c.ID).HasColumnName("ID");
        builder.Property(c => c.ObjectID).HasColumnName("ObjectID");
        builder.Property(c => c.ClassID).HasColumnName("ClassID");
        builder.Property(c => c.Name).HasColumnName("Name");
    }
}
