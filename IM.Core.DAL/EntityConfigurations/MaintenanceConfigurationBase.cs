using InfraManager.DAL.MaintenanceWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class MaintenanceConfigurationBase : IEntityTypeConfiguration<Maintenance>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string UINameInFolder { get; }
    protected abstract string WorkOrderTemplateForeignKey { get; }

    public void Configure(EntityTypeBuilder<Maintenance> builder)
    {
        builder.HasKey(e => e.ID).HasName(PrimaryKeyName);

        builder.HasIndex(c=> new { c.FolderID, c.Name }, UINameInFolder).IsUnique();

        builder.Property(x => x.Name).HasMaxLength(250).IsRequired(true);
        builder.Property(x => x.Note)
            .HasMaxLength(250)
            .IsRequired(true)
            .HasDefaultValueSql("('')");

        builder.HasOne(d => d.WorkOrderTemplate)
             .WithMany()
             .HasForeignKey(d => d.WorkOrderTemplateID)
             .HasConstraintName(WorkOrderTemplateForeignKey)
             .OnDelete(DeleteBehavior.ClientCascade);

        //TODO добавить FK
        builder.HasOne(d => d.Folder)
             .WithMany(c => c.Maintenances)
             .HasForeignKey(d => d.FolderID);

        ConfigureDatabase(builder);
    }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<Maintenance> builder);
}
