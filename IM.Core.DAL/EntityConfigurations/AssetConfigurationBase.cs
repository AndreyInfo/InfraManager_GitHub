using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class AssetConfigurationBase : IEntityTypeConfiguration<Asset.Asset>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string FixedAssetForeignKeyName { get; }
    protected abstract string SupplierForeignKeyName { get; }
    protected abstract string ServiceCenterForeignKeyName { get; }
    protected abstract string ServiceContractForeignKeyName { get; }
    protected abstract string StorageLocationForeignKeyName { get; }
    protected abstract string UserForeignKeyName { get; }
    protected abstract string LifeCycleStateForeignKeyName { get; }
    protected abstract string LifeCycleStateIDIndexName { get; }
    protected abstract string FixedAssetIDIndexName { get; }
    protected abstract string AssetIDIndexName { get; }
    protected abstract string OwnerIDIndexName { get; }
    protected abstract string UtilizerIDIndexName { get; }
    protected abstract string UserIDIndexName { get; }
    protected abstract string SupplierIDIndexName { get; }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<Asset.Asset> builder);


    public void Configure(EntityTypeBuilder<Asset.Asset> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.Property(x => x.DeviceID).ValueGeneratedNever();
        
        builder.Property(x => x.Agreement).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.Founding).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.UserField1).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.UserField2).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.UserField3).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.UserField4).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.UserField5).IsRequired(false).HasMaxLength(255);

        // todo: FK есть в БД. Настроить навигационное свойство, когда мигрирует сущность FixedAsset.
        // builder.HasOne<FixedAsset>()
        //     .WithMany()
        //     .HasForeignKey(x => x.FixedAssetID)
        //     .HasPrincipalKey(x => x.ID)
        //     .IsRequired(false)
        //     .HasConstraintName(FixedAssetForeignKeyName);

        builder.HasOne(x => x.LifeCycleState)
            .WithMany()
            .HasForeignKey(x => x.LifeCycleStateID)
            .HasPrincipalKey(x => x.ID)
            .IsRequired(false)
            .HasConstraintName(LifeCycleStateForeignKeyName);

        builder.HasOne(x => x.ServiceCenter)
            .WithMany()
            .HasForeignKey(x => x.ServiceCenterID)
            .HasPrincipalKey(x => x.ID)
            .IsRequired(false)
            .HasConstraintName(ServiceCenterForeignKeyName);

        builder.HasOne(x => x.ServiceContract)
            .WithMany()
            .HasForeignKey(x => x.ServiceContractID)
            .HasPrincipalKey(x => x.ID)
            .HasConstraintName(ServiceContractForeignKeyName);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserID)
            .HasPrincipalKey(x => x.ID)
            .IsRequired(false)
            .HasConstraintName(UserForeignKeyName);

        builder.HasOne(x => x.StorageLocation)
            .WithMany()
            .HasForeignKey(x => x.StorageID)
            .HasPrincipalKey(x => x.ID)
            .IsRequired(false)
            .HasConstraintName(StorageLocationForeignKeyName);

        builder.HasOne(x => x.Supplier)
            .WithMany()
            .HasForeignKey(x => x.SupplierID)
            .HasPrincipalKey(x => x.ID)
            .IsRequired(false)
            .HasConstraintName(SupplierForeignKeyName);

        builder.HasOne(x => x.Utilizer)
            .WithMany()
            .HasForeignKey(x => x.UtilizerID)
            .HasPrincipalKey(x => x.IMObjID)
            .IsRequired(false);

        builder.HasOne(x => x.Owner)
            .WithMany()
            .HasForeignKey(x => x.OwnerID)
            .HasPrincipalKey(x => x.IMObjID)
            .IsRequired(false);

        builder.HasIndex(x => x.LifeCycleStateID).HasDatabaseName(LifeCycleStateIDIndexName);
        builder.HasIndex(x => x.FixedAssetID).HasDatabaseName(FixedAssetIDIndexName);
        builder.HasIndex(x => x.ID).HasDatabaseName(AssetIDIndexName);
        builder.HasIndex(x => x.OwnerID).HasDatabaseName(OwnerIDIndexName);
        builder.HasIndex(x => x.UtilizerID).HasDatabaseName(UtilizerIDIndexName);
        builder.HasIndex(x => x.UserID).HasDatabaseName(UserIDIndexName);
        builder.HasIndex(x => x.SupplierID).HasDatabaseName(SupplierIDIndexName);
            
        ConfigureDatabase(builder);
    }
}