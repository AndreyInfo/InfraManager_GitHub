using InfraManager.DAL.Import.ITAsset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;
public abstract class ITAssetImportCSVConfigurationConfigurationBase : IEntityTypeConfiguration<ITAssetImportCSVConfiguration>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string UKName { get; }

    public void Configure(EntityTypeBuilder<ITAssetImportCSVConfiguration> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.Property(e => e.Name).HasMaxLength(250).IsRequired(true);
        builder.HasIndex(x => x.Name, UKName).IsUnique();

        builder.Property(e => e.Note).IsRequired(true).HasMaxLength(500);

        builder.Property(x => x.Delimeter)
            .IsRequired(true)
            .HasMaxLength(1);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<ITAssetImportCSVConfiguration> builder);
}
