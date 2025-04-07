using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.FormBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;

public class FormBuilderFormFieldConfiguration : FormBuilderFormFieldConfigurationBase
{
    protected override string PK => "PK_WorkflowActivityFormField";
    protected override string OptionsForeignKey => "FK_WorkflowActivityFormField_WorkflowActivityFormTab";
    protected override string GroupForeignKey => "FK_GroupField";
    protected override string ColumnForeignKey => "FK_ColumnField";

    protected override void ConfigureDatabase(EntityTypeBuilder<FormField> builder)
    {
        builder.ToTable("WorkflowActivityFormField", "dbo");

        builder.Property(e => e.ID).HasColumnName("ID");
        builder.Property(e => e.Model).HasColumnName("Model");
        builder.Property(e => e.Type).HasColumnName("Type");
        builder.Property(e => e.CategoryName).HasColumnName("CategoryName");
        builder.Property(e => e.Identifier).HasColumnName("Identifier");
        builder.Property(e => e.Order).HasColumnName("Order");
        builder.Property(e => e.TabID).HasColumnName("WorkflowActivityFormTabID");
        builder.Property(e => e.SpecialFields).HasColumnName("SpecialFields");
        builder.Property(e => e.GroupFieldID).HasColumnName("GroupFieldID");
        builder.Property(e => e.ColumnFieldID).HasColumnName("ColumnFieldID");
        builder.Property(e => e.RowVersion).HasColumnName("RowVersion").IsRowVersion();
    }
}