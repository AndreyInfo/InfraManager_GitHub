using InfraManager.DAL.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class BuidingSubnetConfigurationBase : IEntityTypeConfiguration<BuildingSubnet>
{
    protected abstract string KeyName { get; }
    protected abstract string BuildingFK { get; }
    public void Configure(EntityTypeBuilder<BuildingSubnet> builder)
    {
        builder.HasKey(c => c.ID).HasName(KeyName);

        builder.Property(c => c.ID).ValueGeneratedOnAdd();

        builder.Property(c => c.Subnet).IsRequired(true).HasMaxLength(50);

        builder.HasOne(c => c.Building)
            .WithMany()
            .HasForeignKey(c => c.BuildingID)
            .HasConstraintName(BuildingFK);
        
        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<BuildingSubnet> builder);
}
