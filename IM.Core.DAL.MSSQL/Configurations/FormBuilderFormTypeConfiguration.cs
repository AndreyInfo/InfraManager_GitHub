using InfraManager.DAL.FormBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraManager.DAL.EntityConfigurations;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class FormBuilderFormTypeConfiguration : FormBuilderFormConfigurationBase
    {
        protected override string PK => "PK_WorkflowActivityForm";
        protected override string FK_FormTab => "FK_WorkflowActivityFormField_WorkflowActivityFormTab";

        protected override void ConfigureDatabase(EntityTypeBuilder<Form> builder)
        {
            builder.ToTable("WorkflowActivityForm", "dbo");
            
            builder.Property(e => e.ID).HasColumnName("ID");
            builder.Property(e => e.FieldsIsRequired).HasColumnName("FieldsIsRequired");
            builder.Property(e => e.Identifier).HasColumnName("Identifier");
            builder.Property(e => e.ClassID).HasColumnName("ClassID");
            builder.Property(e => e.Width).HasColumnName("Width");
            builder.Property(e => e.Height).HasColumnName("Height");
            builder.Property(e => e.MinorVersion).HasColumnName("MinorVersion");
            builder.Property(e => e.MajorVersion).HasColumnName("MajorVersion");
            builder.Property(e => e.Description).HasColumnName("Description");
            builder.Property(e => e.Status).HasColumnName("Status");
            builder.Property(e => e.LastIndex).HasColumnName("LastIndex");
            builder.Property(e => e.UtcChanged).HasColumnName("UtcChanged");
            builder.Property(e => e.ProductTypeID).HasColumnName("ProductTypeID");
            builder.Property(e => e.Name).HasColumnName("Name");
            builder.Property(e => e.ObjectID).HasColumnName("ObjectID");
            builder.Property(e => e.MainID).HasColumnName("MainID").HasDefaultValueSql("gen_random_uuid()");
            builder.Property(e => e.RowVersion).HasColumnName("RowVersion").IsRowVersion();
        }
    }
}
