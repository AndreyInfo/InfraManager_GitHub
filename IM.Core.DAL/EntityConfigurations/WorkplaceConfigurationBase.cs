using InfraManager.DAL.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class WorkplaceConfigurationBase: IEntityTypeConfiguration<Workplace>
{
    protected abstract string UniqueNameConstraint { get; }
    
    protected abstract string IXWorkplaceIMObjID { get; }

    protected abstract string IXWorkplaceRoomID { get; }

    protected abstract string PrimaryKey { get;  }

    protected abstract string WorkplacesFK { get;  }

    protected abstract string DFID{ get;  }

    protected abstract string DFIMObjID { get;  }


    public void Configure(EntityTypeBuilder<Workplace> builder)
    {
        builder.HasKey(e => e.ID).HasName(PrimaryKey);

        builder.HasIndex(e => new { e.Name, e.RoomID }, UniqueNameConstraint).IsUnique();
        builder.HasIndex(e => e.IMObjID, IXWorkplaceIMObjID);
        builder.HasIndex(e => e.RoomID, IXWorkplaceRoomID);

        builder.Property(e => e.ID)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql(DFID);

        builder.Property(e => e.IMObjID)
            .HasDefaultValueSql(DFIMObjID);

        builder.Property(e => e.Name)
            .HasMaxLength(255)
            .IsRequired(false);
        
        builder.Property(e => e.Note)
            .IsRequired(false)
            .HasMaxLength(1000);

        builder.Property(e => e.ExternalID)
            .IsRequired(true)
            .HasMaxLength(50)
            .HasDefaultValueSql("('')");

        builder.HasOne(d => d.Room)
           .WithMany(x => x.Workplaces)
           .HasForeignKey(x => x.RoomID)
           .HasConstraintName(WorkplacesFK);

        OnConfigurePartial(builder);
    }
    
    protected abstract void OnConfigurePartial(EntityTypeBuilder<Workplace> builder);
}