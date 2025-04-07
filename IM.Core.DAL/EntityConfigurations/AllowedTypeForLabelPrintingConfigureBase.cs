using InfraManager.DAL.Asset.DeviceMonitors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;
public abstract class AllowedTypeForLabelPrintingConfigureBase : IEntityTypeConfiguration<AllowedTypeForLabelPrinting>
{
    protected abstract string PrimaryKeyName { get; }

    public void Configure(EntityTypeBuilder<AllowedTypeForLabelPrinting> builder)
    {
        builder.HasKey(c => c.ID).HasName(PrimaryKeyName);

        builder.Property(c=> c.Name).IsRequired(true).HasMaxLength(255);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<AllowedTypeForLabelPrinting> builder);
}
