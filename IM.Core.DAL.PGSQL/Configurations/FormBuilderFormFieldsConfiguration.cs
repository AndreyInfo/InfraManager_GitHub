using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.FormBuilder;
using InfraManager.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations
{
    internal class FormBuilderFormFieldsConfiguration : FormBuilderFormFieldConfigurationBase
    {
        protected override string PK => "form_builder_form_tabs_fields_pkey";
        protected override string OptionsForeignKey => "fk_field_options_field";
        protected override string GroupForeignKey => "fk_group_field";
        protected override string ColumnForeignKey => "fk_column_field";

        protected override void ConfigureDatabase(EntityTypeBuilder<FormField> builder)
        {
            builder.ToTable("form_builder_form_tabs_fields", Options.Scheme);

            builder.Property(e => e.ID).HasColumnName("id");
            builder.Property(e => e.Model).HasColumnName("model");
            builder.Property(e => e.Type).HasColumnName("type");
            builder.Property(e => e.CategoryName).HasColumnName("category_name");
            builder.Property(e => e.Identifier).HasColumnName("identifier");
            builder.Property(e => e.Order).HasColumnName("order");
            builder.Property(e => e.TabID).HasColumnName("workflow_activity_form_tab_id");
            builder.Property(e => e.SpecialFields).HasColumnName("special_fields");
            builder.Property(e => e.GroupFieldID).HasColumnName("group_field_id");
            builder.Property(e => e.ColumnFieldID).HasColumnName("column_field_id");
            builder.HasXminRowVersion(e => e.RowVersion);
        }
    }
}