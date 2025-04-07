using Inframanager.DAL.ActiveDirectory.Import;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inframanager.DAL.EntityConfigurations
{
    public abstract class UIADClassConfigurationBase : IEntityTypeConfiguration<UIADClass>
    {
        protected abstract string PrimaryKeyName { get; }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<UIADClass> entity);

        public void Configure(EntityTypeBuilder<UIADClass> entity)
        {
            entity.HasKey(x => x.ID).HasName(PrimaryKeyName);

            entity.Property(x => x.ID).IsRequired(true);

            entity.Property(x => x.Name).IsRequired(true).HasMaxLength(250);


            ConfigureDatabase(entity);
        }
    }
}