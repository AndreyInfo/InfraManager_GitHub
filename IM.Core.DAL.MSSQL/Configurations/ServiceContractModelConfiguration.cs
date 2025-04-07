using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class ServiceContractModelConfiguration : ServiceContractModelConfigurationBase
{
    protected override string PrimaryKeyName => "PK_ServiceContractModel";
    protected override string ProductCatalogTypeForeignKeyName => "FK_ServiceContractModel_ProductCatalogType";

    protected override void ConfigureDatabase(EntityTypeBuilder<ServiceContractModel> builder)
    {
        builder.ToTable("ServiceContractModel", Options.Scheme);
        
        builder.Property(x => x.IMObjID).HasColumnName("ID");
        builder.Property(x => x.Name).HasColumnName("Name");
        builder.Property(x => x.Note).HasColumnName("Note");
        builder.Property(x => x.CanBuy).HasColumnName("CanBuy");
        builder.Property(x => x.Removed).HasColumnName("Removed");
        builder.Property(x => x.ExternalID).HasColumnName("ExternalID");
        builder.Property(x => x.ManufacturerID).HasColumnName("ManufacturerID");
        builder.Property(x => x.UpdateAvailable).HasColumnName("UpdateAvailable");
        builder.Property(x => x.ContractSubject).HasColumnName("ContractSubject");
        builder.Property(x => x.ProductCatalogTypeID).HasColumnName("ProductCatalogTypeID");

        builder.Property(x => x.RowVersion)
            .IsRowVersion()
            .HasColumnType("timestamp")
            .HasColumnName("RowVersion");
    }
}