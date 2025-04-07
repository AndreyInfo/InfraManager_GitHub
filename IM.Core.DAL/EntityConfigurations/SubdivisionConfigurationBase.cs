using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class SubdivisionConfigurationBase : IEntityTypeConfiguration<Subdivision>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string InxedUniqueName { get; }

    public void Configure(EntityTypeBuilder<Subdivision> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.HasIndex(c => new { c.Name, c.OrganizationID, c.SubdivisionID }, InxedUniqueName).IsUnique();

        builder.Property(x => x.Name).IsRequired(true).HasMaxLength(255);
        builder.Property(x => x.Note).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.ExternalID).IsRequired(false).HasMaxLength(500);

        builder.HasQueryFilter(Subdivision.ExceptEmptySubdivision);

        builder
            .HasOne(x => x.Organization)
            .WithMany(x => x.Subdivisions)
            .HasForeignKey(x => x.OrganizationID);

        builder.HasOne(x => x.ParentSubdivision).WithMany(x => x.ChildSubdivisions)
            .HasForeignKey(x => x.SubdivisionID);

        builder.HasMany(x => x.ChildSubdivisions)
            .WithOne(x => x.ParentSubdivision)
            .HasForeignKey(x => x.SubdivisionID);

        ConfigureDataProvider(builder);
    }

    

    protected abstract void ConfigureDataProvider(EntityTypeBuilder<Subdivision> builder);
}
