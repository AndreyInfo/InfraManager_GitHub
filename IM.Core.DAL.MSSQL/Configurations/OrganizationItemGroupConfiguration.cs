using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal class OrganizationItemGroupConfiguration : OrganizationItemGroupConfigurationBase
{
    protected override string KeyName => "PK_OrganizationItemGroup";

    protected override void ConfigureDataBase(EntityTypeBuilder<OrganizationItemGroup> builder)
    {
        builder.ToTable("OrganizationItemGroup", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.ItemID).HasColumnName("OrganizationItemID");
        builder.Property(x => x.ItemClassID).HasColumnName("OrganizationItemClassID");
    }
}
