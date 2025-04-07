using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    public class CabinetTypeConfiguration : CabinetTypeConfigurationBase
    {
        protected override string NameIndexName => "ux_name";

        protected override string PrimaryKeyName => "PK_Типы шкафов";
        protected override void ConfigureDatabase(EntityTypeBuilder<CabinetType> entity)
        {
            entity.ToTable("Типы шкафов", "dbo");
            
            entity.Property(x => x.ID).HasColumnName("Идентификатор").HasDefaultValueSql("NEXT VALUE FOR \"Seq Типы шкафов\"");

            entity.Property(x => x.Name).HasColumnName("Название");

            entity.Property(x => x.ManufacturerID).HasColumnName("ИД производителя");
            
            entity.Property(x => x.VerticalSize).HasColumnName("Размер по вертикали");
            
            entity.Property(x => x.DepthSize).HasColumnName("Размер в глубину").HasColumnType("decimal(9,2)");
            
            entity.Property(x => x.Image).HasColumnName("Изображение");

            entity.Property(x => x.ProductNumberCyrillic).HasColumnName("Номер продукта");

            entity.Property(x => x.Category).HasColumnName("Category").HasDefaultValueSql("N'Шкаф'");

            entity.Property(x => x.Code).HasColumnName("Code");

            entity.Property(x => x.Note).HasColumnName("Note");

            entity.Property(x => x.ProductNumber).HasColumnName("ProductNumber");

            entity.Property(x => x.IMObjID).HasColumnName("IMObjID").HasDefaultValueSql("NEWID()");

            entity.Property(x => x.ComplementaryID).HasColumnName("ComplementaryID");

            entity.Property(x => x.WidthI).HasColumnName("WidthI").HasColumnType("decimal(9,2)");

            entity.Property(x => x.Height).HasColumnName("Height").HasColumnType("decimal(9,2)");

            entity.Property(x => x.Width).HasColumnName("Width").HasColumnType("decimal(9,2)");

            entity.Property(x => x.NumberingScheme).HasColumnName("NumberingScheme");

            entity.Property(x => x.ProductCatalogTypeID).HasColumnName("ProductCatalogTypeID");
            
            entity.Property(x => x.ExternalID).HasColumnName("ExternalID").HasDefaultValueSql("N'Шкаф'");
            
            //TODO поправить реализацию с IProductModel(у таблицы нет RowVersion, но интерфейс требует)
            entity.Ignore(x => x.RowVersion);
        }
    }
}