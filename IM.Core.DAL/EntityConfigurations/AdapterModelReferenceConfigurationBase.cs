using Inframanager.DAL.ProductCatalogue.AdapterReference;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class AdapterModelReferenceConfigurationBase : IEntityTypeConfiguration<AdapterModelReference>
    {
        protected abstract string PrimaryKeyName { get; }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<AdapterModelReference> entity);

        public void Configure(EntityTypeBuilder<AdapterModelReference> entity)
        {
            entity.HasKey(x => new {x.AdapterModelID, x.ParentAdapterModelID}).HasName(PrimaryKeyName);

            entity.HasOne(x => x.ParentModel)
                .WithMany()
                .HasForeignKey(x => x.ParentAdapterModelID);

            entity.HasOne(x => x.Model)
                .WithOne()
                .HasForeignKey<AdapterModelReference>(x => x.AdapterModelID);

            ConfigureDatabase(entity);
        }
    }
}