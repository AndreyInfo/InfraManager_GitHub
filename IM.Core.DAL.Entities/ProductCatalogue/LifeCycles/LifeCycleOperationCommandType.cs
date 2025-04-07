namespace InfraManager.DAL.ProductCatalogue.LifeCycles;
public enum LifeCycleOperationCommandType : short
{
    /// <summary>
    /// Поставить на учет
    /// </summary>
    Registration = 0,

    /// <summary>
    /// Перемещение
    /// </summary>
    Move = 1,

    /// <summary>
    /// Отправить в ремонт
    /// </summary>
    ToRepair = 2,

    /// <summary>
    /// Забрать из ремонта
    /// </summary>
    FromRepair = 3,

    /// <summary>
    /// Списать
    /// </summary>
    WriteOff = 4,

    /// <summary>
    /// Конфигурация
    /// </summary>
    Configuration = 5,

    /// <summary>
    /// Использование
    /// </summary>
    Usage = 6,

    /// <summary>
    /// Установить со склада
    /// </summary>
    AddFromStorage = 7,

    /// <summary>
    /// Вернуть на склад
    /// </summary>
    ToStorage = 8,

    /// <summary>
    /// Израсходовать расходный материал
    /// </summary>
    MaterialConsumption = 9,

    /// <summary>
    /// Смена состояния
    /// </summary>
    ChangeLifeCycleState = 10,

    /// <summary>
    /// Выполнить задание
    /// </summary>
    RunWorkOrderTemplate = 11,

    /// <summary>
    /// Выдать лицензию
    /// </summary>
    LicenceConsumption = 12,

    /// <summary>
    /// Вернуть лицензию
    /// </summary>
    LicenceReturn = 13,

    /// <summary>
    /// Создать права на ПО
    /// </summary>
    CreateSoftwareLicencesByServiceContractLicences = 14,

    /// <summary>
    /// Подтвердить инвентаризацию
    /// </summary>
    InventoryConfirm = 15,

    /// <summary>
    /// Игнорировать инвентаризацию
    /// </summary>
    InventoryIgnore = 16,

    /// <summary>
    /// Активация подписки
    /// </summary>
    SoftwareLicenceSubscriptionActivate = 17,

    /// <summary>
    /// Обновить лицензию
    /// </summary>
    SoftwareLicenceUpgradeApply = 18,

    /// <summary>
    /// Применить продление
    /// </summary>
    SoftwareLicenceProlongationApply = 19,

    /// <summary>
    /// Применить доп. соглашение
    /// </summary>
    ContractAgreementApply = 20,

    /// <summary>
    /// Создать права аренды на ПО
    /// </summary>
    CreateSoftwareLicence = 21,

    /// <summary>
    /// Применить Upgrade
    /// </summary>
    SoftwareLicenceUpgrade = 22,
}
