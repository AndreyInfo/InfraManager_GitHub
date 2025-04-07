using InfraManager.DAL.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class BuildingConfigurationBase: IEntityTypeConfiguration<Building>
{
    protected abstract string UniqueNameConstraint { get; }
    protected abstract string PrimaryKey { get; }
    protected abstract string IXBuildingIMObjID { get; }
    protected abstract string IXBuildingOrganizationID { get; }
    protected abstract string IDDefaultValueSQL { get; }
    protected abstract string IMObjIDDefaultValueSQL { get; }

    public void Configure(EntityTypeBuilder<Building> builder)
    { 
        builder.HasKey(e => e.ID).HasName(PrimaryKey);

        builder.HasIndex(x => new { x.Name, x.OrganizationID }, UniqueNameConstraint).IsUnique();
        builder.HasIndex(e => e.IMObjID, IXBuildingIMObjID);
        builder.HasIndex(e => e.OrganizationID, IXBuildingOrganizationID);

        builder.Property(e => e.ID)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql(IDDefaultValueSQL);

        builder.Property(e => e.IMObjID)
            .HasDefaultValueSql(IMObjIDDefaultValueSQL);

        builder.Property(e => e.Name).HasMaxLength(2550).IsRequired(false);
        builder.Property(e => e.City).HasMaxLength(50).IsRequired(false);
        builder.Property(e => e.Region).HasMaxLength(50).IsRequired(false);
        builder.Property(e => e.Area).HasMaxLength(50).IsRequired(false);
        builder.Property(e => e.Street).HasMaxLength(50).IsRequired(false);
        builder.Property(e => e.Index).HasMaxLength(8).IsRequired(false);
        builder.Property(e => e.Note).HasMaxLength(255).IsRequired(false);
        builder.Property(e => e.Housing).HasMaxLength(2).IsRequired(false);
        builder.Property(e => e.HousePart).HasMaxLength(5).IsRequired(false);
        builder.Property(e => e.Image).HasMaxLength(255).IsRequired(false);
        builder.Property(e => e.WiringScheme).HasMaxLength(255).IsRequired(false);
        builder.Property(e => e.TimeZoneID).HasMaxLength(250).IsRequired(false);

        builder.Property(e => e.ExternalID)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValueSql("('')");
        
        builder.Property(e => e.House)
            .HasMaxLength(7)
            .IsUnicode(false);

        OnConfigurePartial(builder);
    }
    
    protected abstract void OnConfigurePartial(EntityTypeBuilder<Building> builder);
}