using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations;

internal class BuidingSubnetConfiguration : BuidingSubnetConfigurationBase
{
    protected override string KeyName => "pk_building_subnet";

    protected override string BuildingFK => "fk_building_subnet_building";

    protected override void ConfigureDataBase(EntityTypeBuilder<BuildingSubnet> builder)
    {
        builder.ToTable("building_subnet", Options.Scheme);


        builder.Property(c => c.ID).HasColumnName("id");
        builder.Property(c => c.Subnet).HasColumnName("subnet");
        builder.Property(c => c.BuildingID).HasColumnName("building_id");
    }
}