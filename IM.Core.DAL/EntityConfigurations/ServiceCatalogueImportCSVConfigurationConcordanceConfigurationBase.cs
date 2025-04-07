using InfraManager.DAL.Import.ServiceCatalogue;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class ServiceCatalogueImportCSVConfigurationConcordanceConfigurationBase : IEntityTypeConfiguration<ServiceCatalogueImportCSVConfigurationConcordance>
    {
        protected abstract string PrimaryKeyName { get; }
        protected abstract string ForeignKeyName { get; }


        public void Configure(EntityTypeBuilder<ServiceCatalogueImportCSVConfigurationConcordance> builder)
        {
            builder.HasKey(x => new { x.ServiceCatalogueImportCSVConfigurationID, x.Field }).HasName(PrimaryKeyName);

            builder.Property(x => x.ServiceCatalogueImportCSVConfigurationID)
                .IsRequired(true);
            builder.Property(x => x.Field)
                .IsRequired(true)
                .HasMaxLength(50);
            builder.Property(x => x.Expression)
                .IsRequired(true)
                .HasMaxLength(2048);

            builder.HasOne(x => x.Configuration)
                .WithMany()
                .HasForeignKey(e => e.ServiceCatalogueImportCSVConfigurationID)
                .HasConstraintName(ForeignKeyName);


            OnConfigurePartial(builder);
        }

        protected abstract void OnConfigurePartial(EntityTypeBuilder<ServiceCatalogueImportCSVConfigurationConcordance> builder);
    }
}
