using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class SideOrganizationConfigurationBase : IEntityTypeConfiguration<SideOrganization>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract void ConfigureDatabase(EntityTypeBuilder<SideOrganization> builder);

    public void Configure(EntityTypeBuilder<SideOrganization> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.Property(x => x.Name).IsRequired(true).HasMaxLength(255);
        builder.Property(x => x.Phone).IsRequired(false).HasMaxLength(200);
        builder.Property(x => x.Fax).IsRequired(false).HasMaxLength(200);
        builder.Property(x => x.Mail).IsRequired(false).HasMaxLength(200);
        builder.Property(x => x.Note).IsRequired(false).HasMaxLength(500);
        builder.Property(x => x.RowVersion).IsRequired();

        ConfigureDatabase(builder);
    }
}