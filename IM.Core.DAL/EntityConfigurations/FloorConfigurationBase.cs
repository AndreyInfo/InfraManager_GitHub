using InfraManager.DAL.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class FloorConfigurationBase : IEntityTypeConfiguration<Floor>
{
    protected abstract string PrimaryKey { get; }
    protected abstract string UniqueNameConstraint { get; }
    protected abstract string BuildingFK { get; }
    protected abstract string IMObjIDDefaultValueSQL { get; }
    protected abstract string IDDefaultValueSQL { get; }
    protected abstract string IXFloorIMObjID { get; }
    protected abstract string IXFloorBuildingID{ get; }
    
    public void Configure(EntityTypeBuilder<Floor> builder)
    {
        builder.HasKey(e => e.ID).HasName(PrimaryKey);

        builder.HasIndex(x => new { x.Name, x.BuildingID }, UniqueNameConstraint).IsUnique();
        builder.HasIndex(e => e.IMObjID, IXFloorIMObjID);
        builder.HasIndex(e => e.BuildingID, IXFloorBuildingID);

        builder.Property(c => c.ID)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql(IDDefaultValueSQL);

        builder.Property(e => e.IMObjID).HasDefaultValueSql(IMObjIDDefaultValueSQL);
        builder.Property(e => e.Name).HasMaxLength(255).IsRequired(false);
        builder.Property(e => e.Note).HasMaxLength(1000).IsRequired(false);
        builder.Property(e => e.FloorDrawing).HasMaxLength(255).IsRequired(false);
        builder.Property(e => e.MethodNamingRoom).HasMaxLength(20).IsRequired(false);
        builder.Property(e => e.Name).HasMaxLength(255).IsRequired(false);
        builder.Property(e => e.ExternalID)
            .IsRequired(true)
            .HasMaxLength(50)
            .HasDefaultValueSql("('')");

        builder.HasOne(d => d.Building)
            .WithMany(d => d.Floors)
            .HasForeignKey(e => e.BuildingID)
            .HasConstraintName(BuildingFK);

        OnConfigurePartial(builder);
    }
    
    protected abstract void OnConfigurePartial(EntityTypeBuilder<Floor> builder);
}