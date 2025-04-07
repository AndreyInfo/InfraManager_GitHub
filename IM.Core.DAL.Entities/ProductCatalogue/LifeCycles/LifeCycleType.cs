namespace InfraManager.DAL.ProductCatalogue.LifeCycles;
public enum LifeCycleType : byte
{
    /// <summary>
    /// ИТ-активы
    /// </summary>
    ITActive = 0,

    /// <summary>
    /// Лицензии ПО
    /// </summary>
    SoftwareLicence = 1,

    /// <summary>
    /// Расходный материал
    /// </summary>
    Material = 2,

    /// <summary>
    /// Контракт
    /// </summary>
    //ServiceContract = 3, //TODO понять почему в легаси был закомменчен

    /// <summary>
    /// Приложение
    /// </summary>
    DeviceApplication = 4,

    /// <summary>
    /// Дополнительное соглашение
    /// </summary>
    ServiceContractAgreement = 5,

    /// <summary>
    /// Контракт (простой)
    /// </summary>
    ServiceContractSimple = 6,

    /// <summary>
    /// Контракт (поставка ПО)
    /// </summary>
    ServiceContractSoftwareSupply = 7,

    /// <summary>
    /// Контракт (аренда ПО)
    /// </summary>
    ServiceContractSoftwareRent = 8,

    /// <summary>
    /// Контракт (обновление ПО)
    /// </summary>
    ServiceContractSoftwareUpdate = 9,

    /// <summary>
    /// Контракт (поставка и обновление ПО)
    /// </summary>
    ServiceContractSoftwareMaintenance = 10,

    /// <summary>
    /// Контракт (аренда и обновление)
    /// </summary>
    ServiceContractMaintenance = 11,

    /// <summary>
    /// Лицензии ПО (подписка)
    /// </summary>
    SoftwareLicenceSubscribe = 12,

    /// <summary>
    /// Лицензии ПО (upgrade)
    /// </summary>
    SoftwareLicenceUpgrade = 13,

    /// <summary>
    /// Лицензии ПО (продление подписки)
    /// </summary>
    SoftwareLicenceProlongation = 14,

    /// <summary>
    /// Конфигурационная единица
    /// </summary>
    ConfigurationUnit = 15,

    /// <summary>
    /// Логический объект
    /// </summary>
    LogicalAsset = 16,

    /// <summary>
    /// Информационный объект
    /// </summary>
    DataEntity = 17,

    /// <summary>
    /// База знаний
    /// </summary>
    KBArticle = 18,
}