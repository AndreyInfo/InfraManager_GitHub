using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.FormBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;

internal class FormBuilderFormFieldSettingsConfiguration : FormBuilderFormFieldSettingsConfigurationBase
{
    protected override string PrimaryKeyName => "pk_form_builder_form_tabs_fields_settings";
    protected override string UserForeignKeyName => "fk_form_builder_form_tabs_fields_settings_user";
    protected override string FieldForeignKeyName => "fk_form_builder_form_tabs_fields_settings_field";
    protected override string UniqueKeyName => "uq_form_builder_form_tabs_fields_settings";
    protected override void ConfigureDatabase(EntityTypeBuilder<FormFieldSettings> builder)
    {
        builder.ToTable("form_builder_form_tabs_fields_settings", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.UserID).HasColumnName("user_id");
        builder.Property(x => x.FieldID).HasColumnName("field_id");
        builder.Property(x => x.Width).HasColumnName("width");
    }
}