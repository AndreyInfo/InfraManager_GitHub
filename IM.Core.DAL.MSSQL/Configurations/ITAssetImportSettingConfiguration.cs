using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Import.ITAsset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;
public class ITAssetImportSettingConfiguration : ITAssetImportSettingConfigurationBase
{
    protected override string PrimaryKeyName => "PK_ITAssetImportSetting";
    protected override string UKName => "UK_ITAssetImportSetting_Name";
    protected override string FKITAssetImportCSVConfiguration => "FK_ITAssetImportSetting_ITAssetImportCSVConfiguration";

    protected override void ConfigureDataBase(EntityTypeBuilder<ITAssetImportSetting> builder)
    {
        builder.ToTable("ITAssetImportSetting", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.Name).HasColumnName("Name");
        builder.Property(x => x.Note).HasColumnName("Note");
        builder.Property(x => x.ITAssetImportCSVConfigurationID).HasColumnName("ITAssetImportCSVConfigurationID");
        builder.Property(x => x.Path).HasColumnName("Path");

        builder.Property(x => x.CreateModelAutomatically).HasColumnName("CreateModelAutomatically");
        builder.Property(x => x.DefaultModelID).HasColumnName("DefaultModelID");
        builder.Property(x => x.MissingModelInSource).HasColumnName("MissingModelInSource");
        builder.Property(x => x.MissingTypeInSource).HasColumnName("MissingTypeInSource");
        builder.Property(x => x.UnsuccessfulAttemptToCreateAutomatically).HasColumnName("UnsuccessfulAttemptToCreateAutomatically");
        builder.Property(x => x.AddToWorkplaceOfUser).HasColumnName("AddToWorkplaceOfUser");
        builder.Property(x => x.DefaultLocationID).HasColumnName("DefaultWorkplaceID");
        builder.Property(x => x.DefaultLocationNotSpecifiedID).HasColumnName("DefaultLocationNotSpecifiedID");
        builder.Property(x => x.DefaultLocationNotFoundID).HasColumnName("DefaultLocationNotFoundID");
        builder.Property(x => x.CreateDeviation).HasColumnName("CreateDeviation");
        builder.Property(x => x.CreateMessages).HasColumnName("CreateMessages");
        builder.Property(x => x.CreateSummaryMessages).HasColumnName("CreateSummaryMessages");
        builder.Property(x => x.WorkflowID).HasColumnName("WorkflowID");
        builder.Property(x => x.NetworkAndTerminalIdenParam).HasColumnName("NetworkAndTerminalIdenParam");
        builder.Property(x => x.AdapterAndPeripheralIdenParam).HasColumnName("AdapterAndPeripheralIdenParam");
        builder.Property(x => x.CUIdenParam).HasColumnName("CUIdenParam");

        builder.Property(x => x.RowVersion).HasColumnName("RowVersion").IsRowVersion().IsRequired(true).HasColumnType("timestamp");
    }
}
