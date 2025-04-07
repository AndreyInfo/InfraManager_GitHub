using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Import.ITAsset;
using InfraManager.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;
public class ITAssetImportCSVConfigurationConfiguration : ITAssetImportCSVConfigurationConfigurationBase
{
    protected override string PrimaryKeyName => "pk_it_asset_import_csv_configuration";
    protected override string UKName => "uk_it_asset_import_csv_configuration_name";

    protected override void ConfigureDataBase(EntityTypeBuilder<ITAssetImportCSVConfiguration> builder)
    {
        builder.ToTable("it_asset_import_csv_configuration", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Note).HasColumnName("note");
        builder.Property(x => x.Delimeter).HasColumnName("delimiter");
        builder.HasXminRowVersion(x => x.RowVersion);
    }
}
