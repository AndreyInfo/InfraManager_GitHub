using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using IM.Core.DAL.Microsoft.SqlServer;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class ManufacturerConfiguration : ManufacturerConfigurationBase
{
    protected override string UiName => "ui_name";
    protected override string ImObjIDIndexName => "AK__Производители_IMObjID";
    protected override string PrimaryKeyName => "PK_Производители";
    protected override string DefaultValueImObjID => "(newid())";
    protected override string DefaultValueID => "NEXT VALUE FOR[pk_manufacturers_seq]";

    protected override void ConfigureDatabase(EntityTypeBuilder<Manufacturer> entity)
    {
        entity.ToTable("Производители", Options.Scheme);
        
        entity.Property(e => e.ID).HasColumnName("Идентификатор");
        entity.Property(e => e.IsRack).HasColumnName("Шкаф");
        entity.Property(e => e.Name).HasColumnName("Название");
        entity.Property(e => e.IsPanel).HasColumnName("Панель");
        entity.Property(e => e.IsCable).HasColumnName("Кабель");
        entity.Property(e => e.ImObjID).HasColumnName("IMObjID");
        entity.Property(e => e.IsOutlet).HasColumnName("Розетки");
        entity.Property(e => e.IsSoftware).HasColumnName("Software");
        entity.Property(e => e.IsCableCanal).HasColumnName("Каналы");
        entity.Property(e => e.IsMaterials).HasColumnName("Materials");
        entity.Property(e => e.IsComputer).HasColumnName("Компьютер");
        entity.Property(e => e.ExternalID).HasColumnName("ExternalID");
        entity.Property(e => e.ComplementaryID).HasColumnName("ComplementaryID");
        entity.Property(e => e.IsNetworkDevice).HasColumnName("Активное оборудование");
        entity.Property(e => e.ComplementaryGuidID).HasColumnName("ComplementaryGuidID");
    }
}
