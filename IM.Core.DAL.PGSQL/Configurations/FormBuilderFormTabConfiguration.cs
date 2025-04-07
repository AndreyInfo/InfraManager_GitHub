using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.FormBuilder;
using InfraManager.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations
{
    internal class FormBuilderFormTabConfiguration : FormBuilderFormTabConfigurationBase
    {
        protected override string PK => "form_builder_form_tabs_pkey";
        protected override string FK_Fields => "fk_tabs_field_form_tab";

        protected override void ConfigureDatabase(EntityTypeBuilder<FormTab> builder)
        {
            builder.ToTable("form_builder_form_tabs", Options.Scheme);
            
            builder.Property(e => e.ID).HasColumnName("id");
            builder.Property(e => e.Name).HasColumnName("name");
            builder.Property(e => e.Type).HasColumnName("type");
            builder.Property(e => e.Icon).HasColumnName("icon");
            builder.Property(e => e.Identifier).HasColumnName("identifier");
            builder.Property(e => e.Order).HasColumnName("order");
            builder.Property(e => e.FormID).HasColumnName("workflow_activity_form_id");
            builder.Property(e => e.Model).HasColumnName("model");
            builder.HasXminRowVersion(x => x.RowVersion);
        }
    }
}