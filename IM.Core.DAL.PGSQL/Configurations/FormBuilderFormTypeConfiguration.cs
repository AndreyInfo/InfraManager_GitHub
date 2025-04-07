using InfraManager.DAL.FormBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;

namespace IM.Core.DAL.PGSQL.Configurations
{
    internal class FormBuilderFormTypeConfiguration : FormBuilderFormConfigurationBase
    {
        protected override string PK => "form_builder_form_pkey";
        protected override string FK_FormTab => "workflow_activity_form_id";
        

        protected override void ConfigureDatabase(EntityTypeBuilder<Form> builder)
        {
            builder.ToTable("form_builder_form", Options.Scheme);
            
            builder.Property(e => e.ID).HasColumnName("id");
            builder.Property(e => e.FieldsIsRequired).HasColumnName("fields_is_required");
            builder.Property(e => e.Identifier).HasColumnName("identifier");
            builder.Property(e => e.ClassID).HasColumnName("class_id");
            builder.Property(e => e.Width).HasColumnName("width");
            builder.Property(e => e.Height).HasColumnName("height");
            builder.Property(e => e.MinorVersion).HasColumnName("minor_version");
            builder.Property(e => e.MajorVersion).HasColumnName("major_version");
            builder.Property(e => e.Description).HasColumnName("description");
            builder.Property(e => e.Status).HasColumnName("status");
            builder.Property(e => e.LastIndex).HasColumnName("last_index");
            builder.Property(e => e.UtcChanged).HasColumnName("utc_changed");
            builder.Property(e => e.ProductTypeID).HasColumnName("product_type_id");
            builder.Property(e => e.Name).HasColumnName("name");
            builder.Property(e => e.ObjectID).HasColumnName("object_id");
            builder.Property(e => e.MainID).HasColumnName("main_id").HasDefaultValueSql("gen_random_uuid()");
            builder.HasXminRowVersion(e => e.RowVersion);
        }
    }
}