using InfraManager.DAL.EntityConfigurations;
using Inframanager.DAL.ProductCatalogue.AdapterReference;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class AdapterModelReferenceConfiguration : AdapterModelReferenceConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_AdapterModelReference";

        protected override void ConfigureDatabase(EntityTypeBuilder<AdapterModelReference> builder)
        {
            builder.ToTable("AdapterModelReference", "dbo");

            builder.Property(x => x.AdapterModelID).HasColumnName("AdapterModelID");

            builder.Property(x => x.ParentAdapterModelID).HasColumnName("ParentAdapterModelID");
        }
    }
}