using InfraManager.DAL.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class RoomConfigurationBase: IEntityTypeConfiguration<Room>
{
    protected abstract string UniqueNameConstraint { get; }
    protected abstract string IXRoomIMObjID { get; }
    protected abstract string IXRoomFloorID { get; }
    protected abstract string PrimaryKey { get; }
    protected abstract string FloorFK { get; }
    protected abstract string IDDefaultValueSQL { get; }
    protected abstract string IMObjIDDefaultValueSQL { get; }

    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(e => e.ID).HasName(PrimaryKey);

        builder.HasIndex(x => new { x.Name, x.FloorID }, UniqueNameConstraint).IsUnique();
        builder.HasIndex(e => e.IMObjID, IXRoomIMObjID);
        builder.HasIndex(e => e.FloorID, IXRoomFloorID);

        builder.Property(e => e.ID)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql(IDDefaultValueSQL)
            ;

        builder.Property(e => e.IMObjID).HasDefaultValueSql(IMObjIDDefaultValueSQL);
        builder.Property(e => e.Name).HasMaxLength(255).IsRequired(false);
        builder.Property(e => e.Note).HasMaxLength(255).IsRequired(false);
        builder.Property(e => e.ServiceZone).HasMaxLength(50).IsRequired(false);
        builder.Property(e => e.Key).HasMaxLength(50).IsRequired(false);
        builder.Property(e => e.Size).HasMaxLength(50).IsRequired(false);
        builder.Property(e => e.LocationPoint).HasMaxLength(50).IsRequired(false);
        builder.Property(e => e.Plan).HasMaxLength(255).IsRequired(false);
        
        builder.Property(e => e.ExternalID)
            .IsRequired(true)
            .HasMaxLength(50)
            .HasDefaultValueSql("('')");

        builder.HasOne(d => d.Floor)
                .WithMany(x => x.Rooms)
                .HasForeignKey(x => x.FloorID)
                .HasConstraintName(FloorFK);

        //TODO добавить FK
        builder.HasOne(d => d.RoomType)
           .WithMany()
           .HasForeignKey(e => e.TypeID);

        OnConfigurePartial(builder);
    }
    
    protected abstract void OnConfigurePartial(EntityTypeBuilder<Room> builder);
}