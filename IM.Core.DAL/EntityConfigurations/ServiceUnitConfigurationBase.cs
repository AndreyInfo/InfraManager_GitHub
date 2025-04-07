using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class ServiceUnitConfigurationBase : IEntityTypeConfiguration<ServiceUnit>
{
    protected abstract string KeyName { get; }

    public void Configure(EntityTypeBuilder<ServiceUnit> builder)
    {
        builder.HasKey(c => c.ID).HasName(KeyName);

        builder.Property(c => c.Name).HasMaxLength(250).IsRequired();

        //TODO добавить FK
        builder.HasOne(c => c.ResponsibleUser)
             .WithMany()
             .HasForeignKey(c => c.ResponsibleID)
             .HasPrincipalKey(c => c.IMObjID);

        //TODO добавить FK
        builder.HasMany(c => c.OrganizationItemGroups)
            .WithOne(c => c.ServiceUnit)
            .HasForeignKey(c => c.ID);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<ServiceUnit> builder);
}
