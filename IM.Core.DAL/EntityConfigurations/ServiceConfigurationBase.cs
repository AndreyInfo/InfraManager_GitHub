using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;
public abstract class ServiceConfigurationBase : IEntityTypeConfiguration<Service>
{
    protected abstract string KeyName { get; }
    protected abstract string NameUI { get; }
    protected abstract string CategoryIDIX { get; }
    protected abstract string ServiceCategortFK { get; }

    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.HasKey(x => x.ID).HasName(KeyName);
        
        builder.HasIndex(c => new { c.Name, c.CategoryID }, NameUI).IsUnique();
        builder.HasIndex(c => c.CategoryID, CategoryIDIX);

        builder.Property(x => x.Note).IsRequired(false);
        builder.Property(x => x.IconName)
            .IsRequired(false)
            .HasMaxLength(250);
        
        builder.Property(x => x.ExternalID)
            .IsRequired(true)
            .HasMaxLength(250);

        builder.Property(x => x.Name)
            .HasMaxLength(250)
            .IsRequired(true);


        builder.HasOne(x => x.Category)
            .WithMany(c=> c.Services)
            .HasForeignKey(x => x.CategoryID)
            .HasConstraintName(ServiceCategortFK);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<Service> builder);
}
