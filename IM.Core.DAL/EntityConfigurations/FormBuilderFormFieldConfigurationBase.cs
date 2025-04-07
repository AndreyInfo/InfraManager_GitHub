using System;
using InfraManager.DAL.Events;
using InfraManager.DAL.FormBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class FormBuilderFormFieldConfigurationBase : IEntityTypeConfiguration<FormField>
{    
    protected abstract string PK { get; }
    protected abstract string OptionsForeignKey { get; }
    protected abstract string GroupForeignKey { get; }
    protected abstract string ColumnForeignKey { get; }

    public void Configure(EntityTypeBuilder<FormField> builder)
    {
        builder.HasKey(x => x.ID).HasName(PK);

        builder.Property(x => x.CategoryName).IsRequired(true).HasMaxLength(100);
        builder.Property(x => x.Identifier).IsRequired(true).HasMaxLength(100);
        builder.Property(x => x.Type).IsRequired(true).HasMaxLength(50)
            .HasConversion(
                v => v.ToString(),
                s => Enum.Parse<FieldTypes>(s));
        builder.Property(x => x.SpecialFields).IsRequired(true);
        builder.Property(x => x.Model).IsRequired(true).HasMaxLength(50);
        builder.Property(x => x.GroupFieldID).IsRequired(false);
        builder.Property(x => x.ColumnFieldID).IsRequired(false);

        builder.HasMany(x => x.Options).WithOne().HasForeignKey(x => x.WorkflowActivityFormFieldID)
            .HasConstraintName(OptionsForeignKey);

        builder.HasMany(x => x.Grouped).WithOne().HasForeignKey(x => x.GroupFieldID).HasConstraintName(GroupForeignKey)
            .IsRequired(false);
        
        builder.HasMany(x => x.Columns).WithOne().HasForeignKey(x => x.ColumnFieldID)
            .HasConstraintName(ColumnForeignKey).IsRequired(false);

        ConfigureDatabase(builder);
    }
    
    protected abstract void ConfigureDatabase(EntityTypeBuilder<FormField> builder);
}