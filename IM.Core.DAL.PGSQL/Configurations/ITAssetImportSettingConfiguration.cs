using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Import.ITAsset;
using InfraManager.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;
public class ITAssetImportSettingConfiguration : ITAssetImportSettingConfigurationBase
{
    protected override string PrimaryKeyName => "pk_it_asset_import_setting";
    protected override string UKName => "uk_it_asset_import_setting_name";
    protected override string FKITAssetImportCSVConfiguration => "fk_it_asset_import_setting_it_asset_import_csv";

    protected override void ConfigureDataBase(EntityTypeBuilder<ITAssetImportSetting> builder)
    {
        builder.ToTable("it_asset_import_setting", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Note).HasColumnName("note");
        builder.Property(x => x.ITAssetImportCSVConfigurationID).HasColumnName("it_asset_import_csv_configuration_id");
        builder.Property(x => x.Path).HasColumnName("path");
        builder.Property(x => x.CreateModelAutomatically).HasColumnName("create_model_automatically");
        builder.Property(x => x.DefaultModelID).HasColumnName("default_model_id");
        builder.Property(x => x.MissingModelInSource).HasColumnName("missing_model_in_source");
        builder.Property(x => x.MissingTypeInSource).HasColumnName("missing_type_in_source");
        builder.Property(x => x.UnsuccessfulAttemptToCreateAutomatically).HasColumnName("unsuccessful_attempt_to_create_automatically");
        builder.Property(x => x.AddToWorkplaceOfUser).HasColumnName("add_to_workplace_of_user");
        builder.Property(x => x.DefaultLocationID).HasColumnName("default_location_id");
        builder.Property(x => x.DefaultLocationNotSpecifiedID).HasColumnName("default_location_not_specified_id");
        builder.Property(x => x.DefaultLocationNotFoundID).HasColumnName("default_location_not_found_id");
        builder.Property(x => x.CreateDeviation).HasColumnName("create_deviation");
        builder.Property(x => x.CreateMessages).HasColumnName("create_messages");
        builder.Property(x => x.CreateSummaryMessages).HasColumnName("create_summary_messages");
        builder.Property(x => x.WorkflowID).HasColumnName("workflow_id");
        builder.Property(x => x.NetworkAndTerminalIdenParam).HasColumnName("network_and_terminal_iden_param");
        builder.Property(x => x.AdapterAndPeripheralIdenParam).HasColumnName("adapter_and_peripheral_iden_param");
        builder.Property(x => x.CUIdenParam).HasColumnName("cu_iden_param");
        builder.HasXminRowVersion(x => x.RowVersion);
    }
}
