using InfraManager.DAL.Import.ITAsset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;
public abstract class ITAssetImportSettingConfigurationBase : IEntityTypeConfiguration<ITAssetImportSetting>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string UKName { get; }
    protected abstract string FKITAssetImportCSVConfiguration { get; }

    public void Configure(EntityTypeBuilder<ITAssetImportSetting> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.Property(x => x.Name).HasMaxLength(250).IsRequired(true);
        builder.HasIndex(x => x.Name, UKName).IsUnique();

        builder.Property(x => x.Note).IsRequired(false).HasMaxLength(500);

        builder.Property(x => x.Path).IsRequired(true).HasMaxLength(500);

        builder.Property(x => x.DefaultModelID).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.DefaultLocationID).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.WorkflowID).IsRequired(false).HasMaxLength(50);

        builder.HasOne(x => x.ITAssetImportCSVConfiguration)
           .WithMany()
           .HasForeignKey(x => x.ITAssetImportCSVConfigurationID)
           .HasConstraintName(FKITAssetImportCSVConfiguration)
           .OnDelete(DeleteBehavior.SetNull);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<ITAssetImportSetting> builder);
}
