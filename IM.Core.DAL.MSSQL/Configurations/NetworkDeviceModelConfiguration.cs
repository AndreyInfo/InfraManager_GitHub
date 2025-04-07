using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using IM.Core.DAL.Microsoft.SqlServer;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal sealed class NetworkDeviceModelConfiguration : NetworkDeviceModelConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_Типы активного оборудования";
        protected override string ProductCatalogTypeForeignKey => "FK_NetworkDeviceModel_ProductCatalogType";
        protected override string ManufacturerForeignKey => "FK_Типы активного оборудования_Производители";
        protected override string IMObjIDIndex => "IX_NetworkDeviceModel_IMObjID";
        protected override string ProductCatalogTypeIDIndex => "IX_NetworkDeviceModel_ProductCatalogTypeID";
        protected override string DefaultValueIMObjID => "(newid())";

        protected override void ConfigureDatabase(EntityTypeBuilder<NetworkDeviceModel> entity)
        {
            entity.ToTable("Типы активного оборудования", Options.Scheme);

            entity.Property(e => e.ID).HasColumnName("Идентификатор");
            entity.Property(e => e.Name).HasColumnName("Название");
            entity.Property(e => e.ManufacturerID).HasColumnName("ИД производителя");
            entity.Property(e => e.ProductNumber).HasColumnName("ProductNumber");
            entity.Property(e => e.PortCount).HasColumnName("Количество портов");
            entity.Property(e => e.Oid).HasColumnName("OID");
            entity.Property(e => e.Code).HasColumnName("Code");
            entity.Property(e => e.Note).HasColumnName("Note");
            entity.Property(e => e.Removed).HasColumnName("Removed");
            entity.Property(e => e.ComplementaryID).HasColumnName("ComplementaryID");
            entity.Property(e => e.Depth).HasColumnName("Depth").HasColumnType("decimal(9, 2)");
            entity.Property(e => e.IsRackmount).HasColumnName("IsRackmount");
            entity.Property(e => e.HypervisorModelID).HasColumnName("HypervisorModelID");
            entity.Property(e => e.MaxLoad).HasColumnName("MaxLoad");
            entity.Property(e => e.NominalLoad).HasColumnName("NominalLoad");
            entity.Property(e => e.ColorPrint).HasColumnName("ColorPrint");
            entity.Property(e => e.MaxPrintFormat).HasColumnName("MaxPrintFormat");
            entity.Property(e => e.PrintSpeedUnit).HasColumnName("PrintSpeedUnit");
            entity.Property(e => e.RollNumber).HasColumnName("RollNumber");
            entity.Property(e => e.Speed).HasColumnType("decimal(10, 2)").HasColumnName("Speed");
            entity.Property(e => e.ProductCatalogTypeID).HasColumnName("ProductCatalogTypeID");
            entity.Property(e => e.Width).HasColumnType("decimal(9, 2)").HasColumnName("Ширина");
            entity.Property(e => e.Height).HasColumnType("decimal(9, 2)").HasColumnName("Высота");
            entity.Property(e => e.HeightInUnits).HasColumnType("decimal(9, 2)").HasColumnName("Размер по высоте");
            entity.Property(e => e.SlotCount).HasColumnName("Количество слотов");
            entity.Property(e => e.CanBuy).HasColumnName("CanBuy");
            entity.Property(e => e.ProductNumberCyrillic).HasColumnName("Номер продукта");
            entity.Property(e => e.ImageCyrillic).HasColumnName("Изображение");
            entity.Property(e => e.ExternalID).HasColumnName("ExternalID");
            entity.Property(e => e.IMObjID).HasColumnName("IMObjID");

            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .HasColumnName("RowVersion");

        }
    }
}
