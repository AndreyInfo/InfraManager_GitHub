using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal class BuidingSubnetConfiguration : BuidingSubnetConfigurationBase
{
    protected override string KeyName => "PK_BuildingSubnet";

    protected override string BuildingFK => "FK_BuildingSubnet_Building";

    protected override void ConfigureDataBase(EntityTypeBuilder<BuildingSubnet> builder)
    {
        builder.ToTable("BuildingSubnet", "dbo");

        builder.Property(c => c.ID).HasColumnName("ID");
        builder.Property(c => c.Subnet).HasColumnName("Subnet");
        builder.Property(c => c.BuildingID).HasColumnName("BuildingID");
    }
}
