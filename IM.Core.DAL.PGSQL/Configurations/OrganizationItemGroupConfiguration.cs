using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

internal class OrganizationItemGroupConfiguration : OrganizationItemGroupConfigurationBase
{
    protected override string KeyName => "pk_organization_item_group";

    protected override void ConfigureDataBase(EntityTypeBuilder<OrganizationItemGroup> builder)
    {
        builder.ToTable("organization_item_group", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.ItemID).HasColumnName("organization_item_id");
        builder.Property(x => x.ItemClassID).HasColumnName("organization_item_class_id");
    }
}