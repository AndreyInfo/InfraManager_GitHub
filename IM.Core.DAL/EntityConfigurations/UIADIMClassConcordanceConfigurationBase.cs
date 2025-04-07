using Inframanager.DAL.ActiveDirectory.Import;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inframanager.DAL.EntityConfigurations
{
    public abstract class UIADIMClassConcordanceConfigurationBase : IEntityTypeConfiguration<UIADIMClassConcordance>
    {
        protected abstract string PrimaryKeyName { get; }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<UIADIMClassConcordance> entity);

        public void Configure(EntityTypeBuilder<UIADIMClassConcordance> entity)
        {
            entity.HasKey(x => new {ID = x.ConfigurationID, x.ClassID, x.IMClassID}).HasName(PrimaryKeyName);

            entity.Property(x => x.ConfigurationID).IsRequired(true);

            entity.Property(x => x.ClassID).IsRequired(true);


            entity.HasOne(x => x.Class)
                .WithOne()
                .HasForeignKey<UIADIMClassConcordance>(x => x.ClassID);
            ConfigureDatabase(entity);
        }
    }
}