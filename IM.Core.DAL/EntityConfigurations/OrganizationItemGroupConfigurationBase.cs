using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class OrganizationItemGroupConfigurationBase : IEntityTypeConfiguration<OrganizationItemGroup>
{
    protected abstract string KeyName { get; }
    
    public void Configure(EntityTypeBuilder<OrganizationItemGroup> builder)
    {
        builder.HasKey(x => new { x.ID, ItemId = x.ItemID }).HasName(KeyName);

        builder.Property(x => x.ID).ValueGeneratedNever();
        builder.Property(x => x.ItemID).ValueGeneratedNever();
        builder.Property(x => x.ItemClassID);

        // TODO добавить FK
        builder.HasOne(x => x.AccessPermission)
            .WithMany(x => x.OrganizationItems)
            .HasForeignKey(x => x.ID);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<OrganizationItemGroup> builder);
}
