using Inframanager.DAL.ProductCatalogue.Synonyms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class SynonymConfigurationBase : IEntityTypeConfiguration<Synonym>
    {
        protected abstract string PrimaryKeyName { get; }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<Synonym> entity);

        public void Configure(EntityTypeBuilder<Synonym> entity)
        {
            entity.HasKey(x => x.ID).HasName(PrimaryKeyName);

            entity.Property(x => x.ClassID).IsRequired(true);

            entity.Property(x => x.ModelID).IsRequired(true);

            entity.Property(x => x.ModelName).IsRequired(true).HasMaxLength(255);

            entity.Property(x => x.ModelProducer).IsRequired(true).HasMaxLength(255);

            entity.HasOne(x => x.AdapterType)
                .WithOne()
                .HasForeignKey<Synonym>(x => x.ModelID);

            entity.HasOne(x => x.PeripheralType)
                .WithOne()
                .HasForeignKey<Synonym>(x => x.ModelID);

            entity.HasIndex(x => x.ClassID);


            ConfigureDatabase(entity);
        }
    }
}