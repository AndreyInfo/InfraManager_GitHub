using InfraManager.DAL.Dashboards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class DashboardConfigurationBase : IEntityTypeConfiguration<Dashboard>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string FolderForeignKeyName { get; }
    protected abstract string UINameByFolderID { get; }

    public void Configure(EntityTypeBuilder<Dashboard> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);
        
        builder.HasIndex(c => new { c.Name, c.DashboardFolderID }, UINameByFolderID).IsUnique();

        builder.Property(x => x.Name)
            .IsRequired(true)
            .HasMaxLength(250);

        builder.HasOne(x => x.Folder)
            .WithMany()
            .HasForeignKey(x => x.DashboardFolderID)
            .HasConstraintName(FolderForeignKeyName);

        ConfigureDatabase(builder);
    }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<Dashboard> builder);
}

