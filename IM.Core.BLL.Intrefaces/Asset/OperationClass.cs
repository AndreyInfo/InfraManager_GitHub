using System.ComponentModel;
using InfraManager.BLL.Localization;

namespace InfraManager.BLL.Asset
{
    public enum OperationType : byte
    {
        /// <summary>
        /// Поставить на учет
        /// </summary>
        [FriendlyName("поставил на учет")]
        [Description("Поставить на учет")]
        Registration = 0,

        /// <summary>
        /// Перемещение
        /// </summary>
        [FriendlyName("переместил")]
        [Description("Переместить")]
        Move = 1,

        /// <summary>
        /// Отправить в ремонт
        /// </summary>
        [FriendlyName("отправил в ремонт")]
        [Description("Отправить в ремонт")]
        ToRepair = 2,

        /// <summary>
        /// Забрать из ремонта
        /// </summary>
        [FriendlyName("забрал из ремонта")]
        [Description("Забрать из ремонта")]
        FromRepair = 3,

        /// <summary>
        /// Списать
        /// </summary>
        [FriendlyName("списал")]
        [Description("Списать")]
        WriteOff = 4,

        /// <summary>
        /// Конфигурация
        /// </summary>
        [FriendlyName("изменил конфигурацию")]
        [Description("Изменить конфигурацию")]
        Configuration = 5,

        /// <summary>
        /// Использование
        /// </summary>
        [FriendlyName("передал")]
        [Description("Передать")]
        Usage = 6,

        /// <summary>
        /// Установить со склада
        /// </summary>
        [FriendlyName("установил со склада")]
        [Description("Установить со склада")]
        AddFromStorage = 7,

        /// <summary>
        /// Вернуть на склад
        /// </summary>
        [FriendlyName("вернул на склад")]
        [Description("Вернуть на склад")]
        ToStorage = 8,

        /// <summary>
        /// Израсходовать расходный материал
        /// </summary>
        [FriendlyName("израсходовал")]
        [Description("Расход материала")]
        MaterialConsumption = 9,

        /// <summary>
        /// Сменить имущественное состояние
        /// </summary>
        [FriendlyName("сменил состояние")]
        [Description("Смена состояния")]
        ChangeLifeCycleState = 10,

        /// <summary>
        /// Выполнить задание
        /// </summary>
        [FriendlyName("выполнил задание")]
        [Description("Выполнение задания")]
        RunWorkOrderTemplate = 11,


        /// <summary>
        /// Израсходовать лицензию
        /// </summary>
        [FriendlyName("выдал лицензию")]
        [Description("Выдать лицензию")]
        LicenceConsumption = 12,


        /// <summary>
        /// Вернуть лицензию
        /// </summary>
        [FriendlyName("вернул лицензию")]
        [Description("Вернуть лицензию")]
        LicenceReturn = 13,

        /// <summary>
        /// Создать права на ПО
        /// </summary>
        [FriendlyName("cоздал права на ПО")]
        [Description("Создать права на ПО")]
        CreateSoftwareLicencesByServiceContractLicences = 14,

        /// <summary>
        /// Подтвердить инвентаризацию
        /// </summary>
        [FriendlyName("подтвердил инвентаризацию")]
        [Description("Подтвердить инвентаризацию")]
        InventoryConfirm = 15,

        /// <summary>
        /// Игнорировать инвентаризацию
        /// </summary>
        [FriendlyName("игнорировал инвентаризацию")]
        [Description("Игнорировать инвентаризацию")]
        InventoryIgnore = 16,

        /// <summary>
        /// Активация подписки
        /// </summary>
        [FriendlyName("активировал подписку")]
        [Description("Активация подписки")]
        SoftwareLicenceSubscriptionActivate = 17,

        /// <summary>
        /// Применить Upgrade
        /// </summary>
        [FriendlyName("обновил лицензию")]
        [Description("Обновить лицензию")]
        SoftwareLicenceUpgradeApply = 18,

        /// <summary>
        /// Применить продление
        /// </summary>
        [FriendlyName("применил продление")]
        [Description("Применить продление")]
        SoftwareLicenceProlongationApply = 19,

        /// <summary>
        /// Применить доп. соглашение
        /// </summary>
        [FriendlyName("применил доп. соглашение")]
        [Description("Применить доп. соглашение")]
        ContractAgreementApply = 20,

        /// <summary>
        /// Создать лицензию ПО
        /// </summary>
        [FriendlyName("создал права аренды на ПО")]
        [Description("Создать права аренды на ПО")]
        CreateSoftwareLicence = 21,

        /// <summary>
        /// Применить Upgrade
        /// </summary>
        [FriendlyName("применил Upgrade")]
        [Description("Применить Upgrade")]
        SoftwareLicenceUpgrade_UpgradeApply = 22,
    }
}
