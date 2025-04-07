using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.FormBuilder;
using InfraManager.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.PGSQL.Configurations
{
    internal class FormBuilderFieldOptionsConfiguration : FormBuilderDynamicOptionsConfigurationBase
    {
        protected override string PK => "form_builder_field_options_pkey";

        protected override void ConfigureDatabase(EntityTypeBuilder<DynamicOptions> builder)
        {
            builder.ToTable("form_builder_field_options", Options.Scheme);

            builder.Property(e => e.ID).HasColumnName("id");
            builder.Property(e => e.Constant).HasColumnName("constant");
            builder.Property(e => e.OperationID).HasColumnName("operation_id");
            builder.Property(e => e.ActionID).HasColumnName("action_id");
            builder.Property(e => e.ParentIdentifier).HasColumnName("parent_identifier");
            builder.Property(e => e.WorkflowActivityFormFieldID).HasColumnName("workflow_activity_form_field_id");
            builder.HasXminRowVersion(e => e.RowVersion);
        }
    }
}