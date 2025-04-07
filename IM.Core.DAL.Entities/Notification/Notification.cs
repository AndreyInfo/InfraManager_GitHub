using System;
using System.Collections.Generic;
using Inframanager;
using InfraManager.DAL.Settings;

namespace InfraManager.DAL.Notification;

[ObjectClassMapping(ObjectClass.Notification)]
[OperationIdMapping(ObjectAction.Insert, OperationID.Notification_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.Notification_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.Notification_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.Notification_Properties)]
public class Notification
{
    public Guid ID { get; init; }
    public string Name { get; set; }
    public string Note { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public ObjectClass ClassID { get; set; }
    public byte[] RowVersion { get; set; }
    public BusinessRole AvailableBusinessRole { get; set; }
    public BusinessRole DefaultBusinessRole { get; set; }

    public virtual InframanagerObjectClass Class { get; set; }
    public virtual ICollection<NotificationRecipient> NotificationRecipients { get; private set; }
    public virtual ICollection<NotificationUser> NotificationUsers { get; private set; }
}
