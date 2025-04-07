using InfraManager.DAL.Asset;
using InfraManager.DAL.ConfigurationData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class DataEntityConfigurationBase : IEntityTypeConfiguration<DataEntity>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string DeviceApplicationForeignKeyName { get; }
    protected abstract string VolumeForeignKeyName { get; }
    protected abstract string InfrastructureSegmentForeignKeyName { get; }
    protected abstract string CriticalityForeignKeyName { get; }
    protected abstract string LifeCycleStateForeignKeyName { get; }
    protected abstract string TypeForeignKeyName { get; }

    public void Configure(EntityTypeBuilder<DataEntity> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.Property(x => x.Name).IsRequired(true).HasMaxLength(250);
        builder.Property(x => x.Note).IsRequired(true).HasMaxLength(500);
        builder.Property(x => x.RowVersion).IsRequired(true);

        builder.HasOne<DeviceApplication>()
            .WithMany()
            .HasForeignKey(x => x.DeviceApplicationID)
            .HasPrincipalKey(x => x.ID)
            .IsRequired(false)
            .HasConstraintName(DeviceApplicationForeignKeyName);

        // todo:  FK есть в БД. Настроить навигационное свойство когда мигрирует Volume.
        // builder.HasOne<Volume>()
        //     .WithMany()
        //     .HasForeignKey(x => x.VolumeID)
        //     .HasPrincipalKey(x => x.)
        //     .IsRequired(false)
        //     .HasConstraintName(VolumeForeignKeyName);

        builder.HasOne<InfrastructureSegment>()
            .WithMany()
            .HasForeignKey(x => x.InfrastructureSegmentID)
            .HasPrincipalKey(x => x.ID)
            .IsRequired(false)
            .HasConstraintName(InfrastructureSegmentForeignKeyName);

        builder.HasOne<Criticality>()
            .WithMany()
            .HasForeignKey(x => x.CriticalityID)
            .HasPrincipalKey(x => x.ID)
            .IsRequired(false)
            .HasConstraintName(CriticalityForeignKeyName);

        builder.HasOne(x => x.LifeCycleState)
            .WithMany()
            .HasForeignKey(x => x.LifeCycleStateID)
            .HasPrincipalKey(x => x.ID)
            .IsRequired(false)
            .HasConstraintName(LifeCycleStateForeignKeyName);

        builder.HasOne(x => x.Type)
            .WithMany()
            .HasForeignKey(x => x.ProductCatalogTypeID)
            .HasPrincipalKey(x => x.IMObjID)
            .IsRequired(true)
            .HasConstraintName(TypeForeignKeyName);
        
        ConfigureDatabase(builder);
    }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<DataEntity> builder);
}