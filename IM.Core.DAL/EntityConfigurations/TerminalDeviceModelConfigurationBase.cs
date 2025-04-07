using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class TerminalDeviceModelConfigurationBase : IEntityTypeConfiguration<TerminalDeviceModel>
    {
        protected abstract string PrimaryKeyName { get; }
        protected abstract string VendorForeignKeyName { get; }
        protected abstract string ProductCatalogTypeForeignKeyName { get; }
        protected abstract string HypervisorModelForeignKeyName { get; }
        protected abstract string ConnectorTypeForeignKeyName { get; }
        protected abstract string TechnologyTypeForeignKeyName { get; }
        protected abstract string IMObjIDIndexName { get; }
        protected abstract string ProductCatalogTypeIDIndexName { get; }
        protected abstract string DefaultValueID { get; }
        protected abstract string DefaultValueIMObjID { get; }

        public void Configure(EntityTypeBuilder<TerminalDeviceModel> builder)
        {
            builder.HasKey(e => e.ID).HasName(PrimaryKeyName);

            builder.Property(e => e.ID).HasDefaultValueSql(DefaultValueID);
            builder.Property(e => e.IMObjID).HasDefaultValueSql(DefaultValueIMObjID);
            builder.Property(e => e.Name).HasMaxLength(255).IsRequired(true);
            builder.Property(e => e.ImageCyrillic).HasMaxLength(1000).IsRequired(false);
            builder.Property(e => e.ProductNumberCyrillic).HasMaxLength(50).IsRequired(false);
            builder.Property(e => e.ProductNumber).HasMaxLength(250).IsRequired(false);
            builder.Property(e => e.ExternalID).HasMaxLength(250).IsRequired(true);
            builder.Property(e => e.Code).HasMaxLength(50).IsRequired(false);
            builder.Property(e => e.Note).HasMaxLength(255).IsRequired(false);

            builder.HasOne(d => d.ProductCatalogType)
                .WithMany(p => p.TerminalDeviceModel)
                .HasForeignKey(d => d.ProductCatalogTypeID)
                .HasPrincipalKey(x => x.IMObjID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName(ProductCatalogTypeForeignKeyName);

            builder.HasOne(d => d.Manufacturer)
                .WithMany(p => p.TerminalDeviceModel)
                .HasForeignKey(d => d.ManufacturerID)
                .HasPrincipalKey(x => x.ID)
                .HasConstraintName(VendorForeignKeyName);

            builder.HasOne(x => x.ConnectorType)
                .WithMany()
                .HasForeignKey(x => x.ConnectorTypeID)
                .HasPrincipalKey(x => x.ID)
                .HasConstraintName(ConnectorTypeForeignKeyName);

            builder.HasOne(x => x.TechnologyType)
                .WithMany()
                .HasForeignKey(x => x.TechnologyTypeID)
                .HasPrincipalKey(x => x.ID)
                .HasConstraintName(TechnologyTypeForeignKeyName);

            // todo: FK есть в БД. Настроить связь когда мигрирует сущность HypervisorModel.
            // builder.HasOne<HypervisorModel>(x => x.HypervisorModel)
            //     .WithMany()
            //     .HasForeignKey(x => x.HypervisorModelID)
            //     .HasPrincipalKey(x => x.ID)
            //     .HasConstraintName(HypervisorModelForeignKeyName);

            builder.HasIndex(e => e.IMObjID).HasDatabaseName(IMObjIDIndexName);
            builder.HasIndex(e => e.ProductCatalogTypeID).HasDatabaseName(ProductCatalogTypeIDIndexName);

            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<TerminalDeviceModel> builder);

    }
}