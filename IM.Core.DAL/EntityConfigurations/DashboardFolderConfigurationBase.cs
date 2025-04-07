using InfraManager.DAL.Dashboards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class DashboardFolderConfigurationBase : IEntityTypeConfiguration<DashboardFolder>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string FolderForeignKeyName { get; }
    protected abstract string FolderNameInParentFolderUI { get; }
    protected abstract string NameParentIsNullUI { get; }

    public void Configure(EntityTypeBuilder<DashboardFolder> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.HasIndex(c => c.Name, NameParentIsNullUI).IsUnique();
        builder.HasIndex(c => new {c.Name, c.ParentDashboardFolderID }, FolderNameInParentFolderUI).IsUnique();

        builder.HasOne(x => x.Parent)
                .WithMany()
                .HasForeignKey(c => c.ParentDashboardFolderID)
                .HasConstraintName(FolderForeignKeyName);

        builder.Property(x => x.Name)
            .IsRequired(true)
            .HasMaxLength(250);

        ConfigureDatabase(builder);
    }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<DashboardFolder> builder);
}

