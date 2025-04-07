using InfraManager.DAL.EntityConfigurations;
using Inframanager.DAL.ProductCatalogue.Synonyms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class SynonymConfiguration : SynonymConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_Synonim_SubDevice";

        protected override void ConfigureDatabase(EntityTypeBuilder<Synonym> builder)
        {
            builder.ToTable("SynonymSubDevice", "dbo");

            builder.Property(x => x.ID).HasColumnName("ID");

            builder.Property(x => x.ClassID).HasColumnName("ClassID");

            builder.Property(x => x.ModelID).HasColumnName("ModelID");

            builder.Property(x => x.ModelName).HasColumnName("ModelName");

            builder.Property(x => x.ModelProducer).HasColumnName("ModelProducer");
        }
    }
}