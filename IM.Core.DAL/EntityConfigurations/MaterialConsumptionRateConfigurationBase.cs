using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class MaterialConsumptionRateConfigurationBase : IEntityTypeConfiguration<MaterialConsumptionRate>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string MaterialModelForeignKeyName { get; }
    
    protected abstract string IndexByMaterialModelID { get; }
    protected abstract string IndexByDeviceCategoryID { get; }

    public void Configure(EntityTypeBuilder<MaterialConsumptionRate> entity)
    {
        entity.HasKey(e => e.ID).HasName(PrimaryKeyName);

        entity.HasIndex(e => e.MaterialModelID, IndexByMaterialModelID);
        entity.HasIndex(e => e.DeviceCategoryID, IndexByDeviceCategoryID);

        entity.Property(e => e.DeviceModelID).IsRequired().HasMaxLength(50);

        entity.HasOne(x => x.Model)
            .WithMany()
            .HasForeignKey(x => x.MaterialModelID)
            .HasConstraintName(MaterialModelForeignKeyName);

        ConfigureDataBase(entity);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<MaterialConsumptionRate> entity);

}