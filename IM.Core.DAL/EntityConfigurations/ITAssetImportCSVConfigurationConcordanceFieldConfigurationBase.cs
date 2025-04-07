using InfraManager.DAL.Import.ITAsset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;
public abstract class ITAssetImportCSVConfigurationConcordanceFieldConfigurationBase : IEntityTypeConfiguration<ITAssetImportCSVConfigurationFieldConcordance>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string ForeignKeyName { get; }

    public void Configure(EntityTypeBuilder<ITAssetImportCSVConfigurationFieldConcordance> builder)
    {
        builder.HasKey(x => new { x.ITAssetImportCSVConfigurationID, x.Field }).HasName(PrimaryKeyName);

        builder.Property(x => x.Field).IsRequired(true).HasMaxLength(50);
        builder.Property(x => x.Expression).IsRequired(true).HasMaxLength(2048);

        builder.HasOne(x => x.Configuration)
            .WithMany()
            .HasForeignKey(e => e.ITAssetImportCSVConfigurationID)
            .HasConstraintName(ForeignKeyName)
            .OnDelete(DeleteBehavior.Cascade);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<ITAssetImportCSVConfigurationFieldConcordance> builder);
}
