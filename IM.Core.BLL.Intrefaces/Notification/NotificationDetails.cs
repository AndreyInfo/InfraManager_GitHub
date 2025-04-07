using InfraManager.Core;
using InfraManager.CrossPlatform.WebApi.Contracts.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Notification
{
    /// <summary>
    /// Опоещение
    /// </summary>
    [EntityCompare((int)ObjectClass.Notification, "Оповещения",
        AddOperationID = (int)OperationID.Notification_Add,
        EditOperationID = (int)OperationID.Notification_Update,
        DeleteOperationID = (int)OperationID.Notification_Delete)]
    public class NotificationDetails
    {
        public Guid ID { get; init; }
        [FieldCompare("Название", 2)]
        public string Name { get; init; }
        [FieldCompare("Примечание", 3)]
        public string Note { get; init; }
        [FieldCompare("Заголовок", 4)]
        public string Subject { get; init; }
        [FieldCompare("Тело", 5)]
        public string Body { get; init; }
        [FieldCompare("Класс", 6)]
        public ObjectClass ClassID { get; init; }
        public byte[] RowVersion { get; init; }
        [FieldCompare("Роль", 7)]
        public int AvailableBusinessRole { get; set; }
        [FieldCompare("Роль по умолчанию", 7)]
        public int DefaultBusinessRole { get; set; }
        public Dictionary<int,string> SelectedRoles { get; init; } = new Dictionary<int, string>();
        public List<NotificationRecipientData> NotificationRecipient { get; init; }
    }
}
