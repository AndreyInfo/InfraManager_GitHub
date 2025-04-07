using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Import.ITAsset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;
public class ITAssetImportCSVConfigurationConcordanceFieldConfiguration : ITAssetImportCSVConfigurationConcordanceFieldConfigurationBase
{
    protected override string PrimaryKeyName => "PK_ITAssetImportCSVConfigurationFieldConcordance";

    protected override string ForeignKeyName => "FK_ITAssetImportCSVConfigurationFieldConcordance_ITAssetImportCSVConfiguration";

    protected override void ConfigureDataBase(EntityTypeBuilder<ITAssetImportCSVConfigurationFieldConcordance> builder)
    {
        builder.ToTable("ITAssetImportCSVConfigurationFieldConcordance", Options.Scheme);

        builder.Property(x => x.ITAssetImportCSVConfigurationID).HasColumnName("ITAssetImportCSVConfigurationID");
        builder.Property(x => x.Field).HasColumnName("Field");
        builder.Property(x => x.Expression).HasColumnName("Expression");
    }
}
