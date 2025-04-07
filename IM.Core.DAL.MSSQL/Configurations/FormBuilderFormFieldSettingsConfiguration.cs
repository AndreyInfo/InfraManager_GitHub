using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.FormBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;

internal class FormBuilderFormFieldSettingsConfiguration : FormBuilderFormFieldSettingsConfigurationBase
{
    protected override string PrimaryKeyName => "PK_WorkflowActivityFormFieldSettings";
    protected override string UserForeignKeyName => "FK_WorkflowActivityFormFieldSettings_User";
    protected override string FieldForeignKeyName => "FK_WorkflowActivityFormFieldSettings_Field";
    protected override string UniqueKeyName => "UQ_WorkflowActivityFormFieldSettings";
    protected override void ConfigureDatabase(EntityTypeBuilder<FormFieldSettings> builder)
    {
        builder.ToTable("WorkflowActivityFormFieldSettings", "dbo");

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.UserID).HasColumnName("UserID");
        builder.Property(x => x.FieldID).HasColumnName("FieldID");
        builder.Property(x => x.Width).HasColumnName("Width");
    }
}