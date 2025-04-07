using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using Inframanager.DAL.ProductCatalogue.AdapterReference;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class AdapterModelReferenceConfiguration : AdapterModelReferenceConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_adapter_model_reference";

        protected override void ConfigureDatabase(EntityTypeBuilder<AdapterModelReference> builder)
        {
            builder.ToTable("adapter_model_reference", Options.Scheme);

            builder.Property(x => x.AdapterModelID).HasColumnName("adapter_model_id");

            builder.Property(x => x.ParentAdapterModelID).HasColumnName("parent_adapter_model_id");
        }
    }
}