using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Import.ServiceCatalogue;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class ServiceCatalogueImportCSVConfigurationConfigurationBase : IEntityTypeConfiguration<ServiceCatalogueImportCSVConfiguration>
    {
        protected abstract string PrimaryKeyName { get; }
        protected abstract string UI_Name { get; }
        public void Configure(EntityTypeBuilder<ServiceCatalogueImportCSVConfiguration> builder)
        {
            builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

            builder.Property(x => x.ID)
                .IsRequired(true);

            builder.Property(e => e.Name)
                .HasMaxLength(250)
                .IsRequired(true);

            builder.Property(e => e.Note)
                .IsRequired(true)
                .HasMaxLength(500);

            builder.Property(x => x.Delimeter)
                .IsRequired(true)
                .HasMaxLength(1);

            builder.Property(x => x.RowVersion)
                .IsRowVersion()
                .IsRequired(true)
                .HasColumnType("timestamp");
            builder.HasIndex(x => x.Name).HasDatabaseName(UI_Name).IsUnique();

            OnConfigurePartial(builder);
        }

        protected abstract void OnConfigurePartial(EntityTypeBuilder<ServiceCatalogueImportCSVConfiguration> builder);
    }
}
