using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.FormBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;

public class FormBuilderDynamicOptionsConfiguration : FormBuilderDynamicOptionsConfigurationBase
{
    protected override string PK => "FormBuilderFieldOptions";

    protected override void ConfigureDatabase(EntityTypeBuilder<DynamicOptions> builder)
    {
        builder.ToTable("WorkflowFieldOptions", "dbo");
        
        builder.Property(e => e.ID).HasColumnName("ID");
        builder.Property(e => e.Constant).HasColumnName("Constant");
        builder.Property(e => e.OperationID).HasColumnName("OperationID");
        builder.Property(e => e.ActionID).HasColumnName("ActionID");
        builder.Property(e => e.ParentIdentifier).HasColumnName("ParentIdentifier");
        builder.Property(e => e.WorkflowActivityFormFieldID).HasColumnName("WorkflowActivityFormFieldID");
        builder.Property(e => e.RowVersion).HasColumnName("RowVersion").IsRowVersion();
    }
}