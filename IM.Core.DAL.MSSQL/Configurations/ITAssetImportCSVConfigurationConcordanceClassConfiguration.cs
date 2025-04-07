using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Import.ITAsset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;
public class ITAssetImportCSVConfigurationConcordanceClassConfiguration : ITAssetImportCSVConfigurationConcordanceClassConfigurationBase
{
    protected override string PrimaryKeyName => "PK_ITAssetImportCSVConfigurationClassConcordance";

    protected override string ForeignKeyName => "FK_ITAssetImportCSVConfigurationClassConcordance_ITAssetImportCSVConfiguration";

    protected override void ConfigureDataBase(EntityTypeBuilder<ITAssetImportCSVConfigurationClassConcordance> builder)
    {
        builder.ToTable("ITAssetImportCSVConfigurationClassConcordance", Options.Scheme);

        builder.Property(x => x.ITAssetImportCSVConfigurationID).HasColumnName("ITAssetImportCSVConfigurationID");
        builder.Property(x => x.Field).HasColumnName("Field");
        builder.Property(x => x.Expression).HasColumnName("Expression");
    }
}
