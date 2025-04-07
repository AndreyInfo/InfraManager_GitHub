using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class CabinetTypeConfigurationBase:IEntityTypeConfiguration<CabinetType>
    {
        protected abstract string NameIndexName { get; }
        protected abstract string PrimaryKeyName { get; }
        
        protected abstract void ConfigureDatabase(EntityTypeBuilder<CabinetType> entity);

        public void Configure(EntityTypeBuilder<CabinetType> builder)
        {
            builder.HasKey(x => x.ID).HasName(PrimaryKeyName);
            
            builder.Property(x => x.Name).HasMaxLength(510);

            builder.Property(x => x.Image).HasMaxLength(1000);

            builder.Property(x => x.Category).HasMaxLength(50);

            builder.Property(x => x.Code).HasMaxLength(50);

            builder.Property(x => x.Note).HasMaxLength(255);

            builder.Property(x => x.ProductNumber).HasMaxLength(255);

            builder.Property(x => x.IMObjID).IsRequired(true);

            builder.Property(x => x.ComplementaryID);

            builder.Property(x => x.VerticalSize).IsRequired(true);

            builder.Property(x => x.WidthI).IsRequired(true);

            builder.Property(x => x.Height);

            builder.Property(x => x.NumberingScheme).IsRequired(true);

            builder.Property(x => x.ProductCatalogTypeID);

            builder.Property(x => x.RowVersion).IsRequired(true);

            builder.Property(x => x.ExternalID).IsRequired(true);

            builder.HasOne(x => x.Manufacturer)
                .WithMany(x => x.RackTypes)
                .HasForeignKey(x => x.ManufacturerID);

            builder.HasOne(x => x.ProductCatalogType)
                .WithMany(x => x.RackTypes)
                .HasForeignKey(x => x.ProductCatalogTypeID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.Name).IsUnique().HasDatabaseName(NameIndexName);
            
            ConfigureDatabase(builder);
        }
    }
}