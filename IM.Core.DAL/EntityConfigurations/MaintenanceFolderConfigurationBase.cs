using InfraManager.DAL.MaintenanceWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class MaintenanceFolderConfigurationBase : IEntityTypeConfiguration<MaintenanceFolder>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string FolderFK { get; }
    protected abstract string UINameInParent { get; }
    protected abstract string UIRootFolder { get; }

    public void Configure(EntityTypeBuilder<MaintenanceFolder> builder)
    {
        builder.HasKey(e => e.ID).HasName(PrimaryKeyName);

        builder.HasIndex(c => new { c.Name, c.ParentID }, UINameInParent).IsUnique();
        builder.HasIndex(c => c.Name, UIRootFolder).IsUnique();

        builder.Property(c => c.Name).HasMaxLength(250).IsRequired(true);

        builder.HasOne(d => d.Parent)
            .WithMany(p => p.SubFolders)
            .HasForeignKey(d => d.ParentID)
            .HasConstraintName(FolderFK)
            .OnDelete(DeleteBehavior.ClientCascade);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<MaintenanceFolder> builder);
}
