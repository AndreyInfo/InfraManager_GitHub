using InfraManager.DAL.ServiceDesk.WorkOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class WorkOrderTemplateConfigurationBase : IEntityTypeConfiguration<WorkOrderTemplate>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string TypeForeignKeyName { get; }
    protected abstract string FolderForeignKeyName { get; }
    protected abstract string PriorityForeignKeyName { get; }
    protected abstract string FormForeignKey { get; }
    
    public void Configure(EntityTypeBuilder<WorkOrderTemplate> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.Property(x => x.Name).HasMaxLength(1000).IsRequired(true);
        builder.Property(x => x.UserField1).HasMaxLength(500).IsRequired(false);
        builder.Property(x => x.UserField2).HasMaxLength(500).IsRequired(false);
        builder.Property(x => x.UserField3).HasMaxLength(500).IsRequired(false);
        builder.Property(x => x.UserField4).HasMaxLength(500).IsRequired(false);
        builder.Property(x => x.UserField5).HasMaxLength(500).IsRequired(false);

        ConfigureDatabase(builder);

        builder
            .HasOne(x => x.Priority)
            .WithMany()
            .HasForeignKey(x => x.WorkOrderPriorityID)
            .HasConstraintName(PriorityForeignKeyName);

        builder
            .HasOne(x => x.Type)
            .WithMany()
            .HasForeignKey(x => x.WorkOrderTypeID)
            .HasConstraintName(TypeForeignKeyName);

        builder
            .HasOne(x => x.Folder)
            .WithMany(x=> x.Templates)
            .HasForeignKey(x => x.FolderID)
            .HasConstraintName(FolderForeignKeyName);

        //TODO добавить FK
        builder.HasOne(x => x.Initiator)
            .WithMany()
            .HasForeignKey(x => x.InitiatorID)
            .HasPrincipalKey(x=> x.IMObjID);

        builder.HasOne(x => x.Form)
              .WithMany()
              .HasForeignKey(c => c.FormID)
              .HasConstraintName(FormForeignKey)
              .IsRequired(false);
    }

    protected abstract void ConfigureDatabase(
        EntityTypeBuilder<WorkOrderTemplate> builder);
}
