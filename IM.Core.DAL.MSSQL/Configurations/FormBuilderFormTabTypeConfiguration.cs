using InfraManager.DAL;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.FormBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations
{
    internal class FormBuilderFormTabTypeConfiguration : FormBuilderFormTabConfigurationBase
    {
        protected override string PK => "PK_WorkflowActivityFormTab";
        protected override string FK_Fields => "FK_WorkflowActivityFormField_WorkflowActivityFormTab";

        protected override void ConfigureDatabase(EntityTypeBuilder<FormTab> builder)
        {
            builder.ToTable("WorkflowActivityFormTab", "dbo");

            builder.Property(x => x.FormID).HasColumnName("WorkflowActivityFormID");
            builder.Property(x => x.ID).HasColumnName("ID");
            builder.Property(x => x.Identifier).HasColumnName("Identifier");
            builder.Property(x => x.Name).HasColumnName("Name");
            builder.Property(x => x.Type).HasColumnName("Type");
            builder.Property(x => x.Icon).HasColumnName("Icon");
            builder.Property(x => x.Model).HasColumnName("Model");
            builder.Property(x => x.Order).HasColumnName("Order");
            builder.Property(e => e.RowVersion).HasColumnName("RowVersion").IsRowVersion();
        }
    }
}
