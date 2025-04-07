using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Import.ITAsset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;
public class ITAssetImportCSVConfigurationConfiguration : ITAssetImportCSVConfigurationConfigurationBase
{
    protected override string PrimaryKeyName => "PK_ITAssetImportCSVConfiguration";
    protected override string UKName => "UK_ITAssetImportCSVConfiguration_Name";

    protected override void ConfigureDataBase(EntityTypeBuilder<ITAssetImportCSVConfiguration> builder)
    {
        builder.ToTable("ITAssetImportCSVConfiguration", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.Name).HasColumnName("Name");
        builder.Property(x => x.Note).HasColumnName("Note");
        builder.Property(x => x.Delimeter).HasColumnName("Delimiter");
        builder.Property(x => x.RowVersion)
            .HasColumnName("RowVersion")
            .IsRowVersion()
            .IsRequired(true)
            .HasColumnType("timestamp");
    }
}
