using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class OrganizationConfigurationBase : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            builder.HasKey(e => e.ID).HasName(PrimaryKeyName);

            builder.Property(e => e.ExternalId).IsRequired(false).HasMaxLength(500);
            builder.Property(e => e.Name).IsRequired(false).HasMaxLength(255);
            builder.Property(e => e.Note).IsRequired(false).HasMaxLength(255);

            builder.HasQueryFilter(Organization.ExceptEmptyOrganization);

            ConfigureDataProvider(builder);
        }

        protected abstract string PrimaryKeyName { get; }

        protected abstract void ConfigureDataProvider(EntityTypeBuilder<Organization> builder);
    }
}
