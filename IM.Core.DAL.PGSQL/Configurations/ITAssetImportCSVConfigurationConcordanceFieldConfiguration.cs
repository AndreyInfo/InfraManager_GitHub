using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Import.ITAsset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;
public class ITAssetImportCSVConfigurationConcordanceFieldConfiguration : ITAssetImportCSVConfigurationConcordanceFieldConfigurationBase
{
    protected override string PrimaryKeyName => "pk_it_asset_import_csv_field_configuration_concordance";

    protected override string ForeignKeyName => "fk_it_asset_import_csv_field_configuration_concordance_import_csv";

    protected override void ConfigureDataBase(EntityTypeBuilder<ITAssetImportCSVConfigurationFieldConcordance> builder)
    {
        builder.ToTable("it_asset_import_csv_configuration_field_concordance", Options.Scheme);

        builder.Property(x => x.ITAssetImportCSVConfigurationID).HasColumnName("it_asset_import_csv_configuration_id");
        builder.Property(x => x.Field).HasColumnName("field");
        builder.Property(x => x.Expression).HasColumnName("expression");
    }
}
