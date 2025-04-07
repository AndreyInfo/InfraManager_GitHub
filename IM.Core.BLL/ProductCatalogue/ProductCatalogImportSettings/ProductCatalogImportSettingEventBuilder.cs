using Inframanager.BLL.Events;
using Inframanager.BLL.EventsOld;
using InfraManager.DAL.ProductCatalogue.Import;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogImportSettings;

public class ProductCatalogImportSettingEventBuilder :
    IConfigureEventBuilder<ProductCatalogImportSetting>
{
    private static string BoolString(bool value) => value ? "да" : "нет";

    private static string BoolString(bool? value) => value.HasValue ? BoolString(value.Value) : "не определено";


    public void Configure(IBuildEvent<ProductCatalogImportSetting> config)
    {
        config.HasEntityName(nameof(ProductCatalogImportSetting));
        config.HasId(x => x.ID);
        config.HasInstanceName(x => x.Name);
        config.HasProperty(x => x.Name).HasName("");

        config.HasProperty(x => x.Note).HasName("");

        config.HasProperty(x => x.RestoreRemovedModels).HasName("").HasConverter(BoolString);
    }

    public void WhenInserted(IBuildEventOperation<ProductCatalogImportSetting> insertConfig)
    {
        insertConfig.HasOperation(OperationID.ProductCatalogImportSetting_Insert,
            x => $"Создан [задача импорта каталога продуктов] '{x.Name}'");
    }

    public void WhenUpdated(IBuildEventOperation<ProductCatalogImportSetting> updateConfig)
    {
        updateConfig.HasOperation(OperationID.ProductCatalogImportSetting_Update,
            x => $"Обновлен [задача импорта каталога продуктов] '{x.Name}'");
    }

    public void WhenDeleted(IBuildEventOperation<ProductCatalogImportSetting> deleteConfig)
    {
        deleteConfig.HasOperation(OperationID.ProductCatalogImportSetting_Delete,
            x => $"Удален [задача импорта каталога продуктов] '{x.Name}'");
    }
}