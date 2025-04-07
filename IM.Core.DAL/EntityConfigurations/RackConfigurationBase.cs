using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class RackConfigurationBase : IEntityTypeConfiguration<Rack>
{
    protected abstract string KeyName { get; }
    protected abstract string RoomFK { get; }
    protected abstract string CabinetTypeFK { get; }
    protected abstract string DefaultValueExternalID{ get; }
    protected abstract string DefaultValueIMObjID { get; }
    protected abstract string IndexIMObjID { get; }
    protected abstract string IndexRoomID { get; }


    public void Configure(EntityTypeBuilder<Rack> builder)
    {
        builder.HasKey(e => e.ID).HasName(KeyName);

        builder.HasIndex(e => e.RoomID, IndexRoomID);
        builder.HasIndex(e => e.IMObjID, IndexIMObjID);

        builder.Property(e => e.ID).ValueGeneratedNever();
        builder.Property(e => e.Name).HasMaxLength(255).IsRequired(false);
        builder.Property(e => e.Note).HasMaxLength(255).IsRequired(false);
        builder.Property(e => e.FillingScheme).HasMaxLength(255).IsRequired(false);
        builder.Property(e => e.Drawing).HasMaxLength(255).IsRequired(false);

        builder.Property(e => e.ExternalID)
            .IsRequired(true)
            .HasMaxLength(50)
            .HasDefaultValueSql(DefaultValueExternalID);

        builder.Property(e => e.IMObjID)
            .HasDefaultValueSql(DefaultValueIMObjID);


        builder.HasOne(d => d.Room)
            .WithMany()
            .HasForeignKey(c => c.RoomID)
            .HasConstraintName(RoomFK);

        //TODO настроить FK
        builder.HasOne(d => d.Floor)
            .WithMany()
            .HasForeignKey(c => c.FloorID);

        builder.HasOne(d => d.Building)
            .WithMany()
            .HasForeignKey(c => c.BuildingID);

        builder.HasOne(d => d.Model)
            .WithMany()
            .HasForeignKey(c=> c.TypeID)
            .HasConstraintName(CabinetTypeFK);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<Rack> builder);
}
