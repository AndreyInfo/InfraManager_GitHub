using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using Inframanager.DAL.ProductCatalogue.Synonyms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class SynonymConfiguration : SynonymConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_synonim_sub_device";

        protected override void ConfigureDatabase(EntityTypeBuilder<Synonym> builder)
        {
            builder.ToTable("synonym_sub_device", Options.Scheme);

            builder.Property(x => x.ID).HasColumnName("id");

            builder.Property(x => x.ClassID).HasColumnName("class_id");

            builder.Property(x => x.ModelID).HasColumnName("model_id");

            builder.Property(x => x.ModelName).HasColumnName("model_name");

            builder.Property(x => x.ModelProducer).HasColumnName("model_producer");
        }
    }
}