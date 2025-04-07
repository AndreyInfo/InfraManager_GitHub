using Inframanager.BLL.Events;
using Inframanager.BLL.EventsOld;
using InfraManager.DAL.ProductCatalogue.Import;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogCsvConfiguration;

public class ProductCatalogImportCSVConfigurationEventBuilder :
    IConfigureEventBuilder<ProductCatalogImportCSVConfiguration>
{
    private static string BoolString(bool value) => value ? "да" : "нет";

    private static string BoolString(bool? value) => value.HasValue ? BoolString(value.Value) : "не определено";


    public void Configure(IBuildEvent<ProductCatalogImportCSVConfiguration> config)
    {
        config.HasEntityName(nameof(ProductCatalogImportCSVConfiguration));
        config.HasId(x => x.ID);
        config.HasInstanceName(x => x.Name);
        config.HasProperty(x => x.Name).HasName("");

        config.HasProperty(x => x.Note).HasName("");
    }

    public void WhenInserted(IBuildEventOperation<ProductCatalogImportCSVConfiguration> insertConfig)
    {
        insertConfig.HasOperation(OperationID.ProductCatalogImportCSVConfiguration_Insert,
            x => $"Создан [конфигурация csv задачи импорта каталога продуктов] '{x.Name}'");
    }

    public void WhenUpdated(IBuildEventOperation<ProductCatalogImportCSVConfiguration> updateConfig)
    {
        updateConfig.HasOperation(OperationID.ProductCatalogImportCSVConfiguration_Update,
            x => $"Обновлен [конфигурация csv задачи импорта каталога продуктов] '{x.Name}'");
    }

    public void WhenDeleted(IBuildEventOperation<ProductCatalogImportCSVConfiguration> deleteConfig)
    {
        deleteConfig.HasOperation(OperationID.ProductCatalogImportCSVConfiguration_Delete,
            x => $"Удален [конфигурация csv задачи импорта каталога продуктов] '{x.Name}'");
    }
}