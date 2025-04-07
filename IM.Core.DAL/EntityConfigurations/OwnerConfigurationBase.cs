using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class OwnerConfigurationBase : IEntityTypeConfiguration<Owner>
    {
        public void Configure(EntityTypeBuilder<Owner> builder)
        {
            builder.HasKey(x => x.IMObjID);
            builder.Property(x => x.Name).IsRequired(false).HasMaxLength(100);
            ConfigureDataProvider(builder);
        }

        protected abstract void ConfigureDataProvider(EntityTypeBuilder<Owner> builder);
    }
}
