using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class WorkOrderTemplateFolderConfigurationBase : IEntityTypeConfiguration<WorkOrderTemplateFolder>
{
    protected abstract string ForeignKeySubFolders { get; }
    protected abstract string ForeignKeyTemplates { get; }
    protected abstract string KeyName { get; }
    protected abstract string DefaultValueID { get; }
    public void Configure(EntityTypeBuilder<WorkOrderTemplateFolder> builder)
    {
        builder.HasKey(c => c.ID).HasName(KeyName);

        builder.Property(c => c.Name).IsRequired(true).HasMaxLength(250);

        builder.Property(c => c.ID)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql(DefaultValueID);

        builder.HasOne(c => c.Parent)
               .WithMany(c => c.SubFolder)
               .HasForeignKey(c => c.ParentID)
               .HasConstraintName(ForeignKeySubFolders);       

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<WorkOrderTemplateFolder> builder); 
}
