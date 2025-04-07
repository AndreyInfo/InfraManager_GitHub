using Inframanager.BLL.EventsOld;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.ProductCatalogue.Manufactures;

public class ManufacturerEventBuilder : IConfigureEventBuilder<Manufacturer>
{
    private static string FromBool(bool value) => value ? "Да" : "Нет";
    public void Configure(IBuildEvent<Manufacturer> config)
    {
        config.HasId(x => x.ImObjID!.Value);
        config.HasEntityName(nameof(Manufacturer));
        config.HasInstanceName(x => x.Name);
        config.HasProperty(x => x.Name).HasName("Название");
        config.HasProperty(x => x.ExternalID).HasName("Внешний ИД");
        config.HasProperty(x => x.IsCable).HasConverter(FromBool).HasName("Кабельный");
        config.HasProperty(x => x.IsComputer).HasConverter(FromBool).HasName("Компьютерный");
        config.HasProperty(x => x.IsMaterials).HasConverter(FromBool).HasName("Расходных материалов");
        config.HasProperty(x => x.IsOutlet).HasConverter(FromBool).HasName("Розеток");
        config.HasProperty(x => x.IsPanel).HasConverter(FromBool).HasName("Пультов");
        config.HasProperty(x => x.IsRack).HasConverter(FromBool).HasName("Шкафов");
        config.HasProperty(x => x.IsSoftware).HasConverter(FromBool).HasName("ПО");
        config.HasProperty(x => x.IsCableCanal).HasConverter(FromBool).HasName("Кабель каналов");
        config.HasProperty(x => x.IsNetworkDevice).HasConverter(FromBool).HasName("Сетевых устройств");
    }

    public void WhenInserted(IBuildEventOperation<Manufacturer> insertConfig)
    {
        insertConfig.HasOperation(OperationID.Manufacturer_Add, manufacturer => $"Добавлен [Производитель] '{manufacturer.Name}");
    }

    public void WhenUpdated(IBuildEventOperation<Manufacturer> updateConfig)
    {
        updateConfig.HasOperation(OperationID.Manufacturer_Update, manufacturer => $"Обновлен [Производитель] '{manufacturer.Name}");
    }

    public void WhenDeleted(IBuildEventOperation<Manufacturer> deleteConfig)
    {
        deleteConfig.HasOperation(OperationID.Manufacturer_Delete, manufacturer => $"Удален [Производитель] '{manufacturer.Name}");
    }
}