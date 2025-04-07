using InfraManager.BLL.Localization;

namespace InfraManager.BLL.Messages;

public enum MessageTypeEnum
{
    [FriendlyName("Мониторинг")]
    Monitoring = 0,

    [FriendlyName("Опрос")]
    Inquiry = 1,

    [FriendlyName("Электронная почта")]
    Email = 2,

    [FriendlyName("Сводное сообщение импорта данных")]
    InquiryTaskForAsset = 3,

    [FriendlyName("Сводное сообщение импорта пользователей")]
    InquiryTaskForUsers = 4,

    [FriendlyName("Импорт оргструктуры")]
    OrganizationStructureImport = 5,
}
