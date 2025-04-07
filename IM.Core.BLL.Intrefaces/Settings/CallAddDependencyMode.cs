using InfraManager.Core;

namespace InfraManager.BLL.Settings;

public enum CallAddDependencyMode
{
    [FriendlyName("Навигатор по местоположению")]
    SKSNavigator = 1,
    [FriendlyName("Перечень имущества")]
    AssetList = 2,
    [FriendlyName("Логическое оборудование и порты")]
    LogicalDeviceNavigator = 4,
    [FriendlyName("Информационные объекты")]
    DataEntityList = 8,
    [FriendlyName("Приложения")]
    DeviceApplicationList = 16,
    [FriendlyName("Разделы дисковых массивов")]
    LogicalUnitList = 32,
    [FriendlyName("Лицензии")]
    SoftwareLicense = 64,
    [FriendlyName("ИТ-Системы")]
    ITSystem = 128,
    [FriendlyName("Сервисные контракты")]
    ServiceContract = 256
}