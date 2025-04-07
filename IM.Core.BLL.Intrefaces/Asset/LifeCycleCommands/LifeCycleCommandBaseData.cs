using System;

namespace InfraManager.BLL.Asset.LifeCycleCommands;
public class LifeCycleCommandBaseData
{
    /// <summary>
    /// Класс объекта.
    /// </summary>
    public ObjectClass ClassID { get; init; }
    /// <summary>
    /// Идентификатор комнаты.
    /// </summary>
    public Guid? RoomID { get; init; }
    /// <summary>
    /// Идентификатор шкафа.
    /// </summary>
    public Guid? RackID { get; init; } 
    /// <summary>
    /// Идентификатор сетевого оборудования.
    /// </summary>
    public Guid? NetworkDeviceID { get; init; } 
    /// <summary>
    /// ID класса нового места размещения.
    /// </summary>
    public ObjectClass LocationClassID { get; init; } 
    /// <summary>
    /// Причина.
    /// </summary>
    public Guid? ReasonID { get; init; } 
    /// <summary>
    /// Владелец.
    /// </summary>
    public Guid? OwnerID { get; init; } 
    /// <summary>
    /// Класс владельца.
    /// </summary>
    public ObjectClass? OwnerClassID { get; init; } 
    /// <summary>
    /// ID пользователя, имеющих роль с опцией «Быть материально-ответственным лицом».
    /// </summary>
    public int? MOL { get; init; } 
    /// <summary>
    /// ID пользователя, кто использует объект.
    /// </summary>
    public Guid? UtilizerID { get; init; } 
    /// <summary>
    /// ID класса пользователя, кто использует объект.
    /// </summary>
    public ObjectClass? UtilizerClassID { get; init; } 
    /// <summary>
    /// Основание.
    /// </summary>
    public string Founding { get; init; } 

    /// <summary>
    /// Дата и время предположительного окончания ремонта.
    /// </summary>
    public DateTime? UtcDateAnticipated { get; init; } 
    /// <summary>
    /// ID сервисного центра.
    /// </summary>
    public Guid? ServiceCenterID { get; init; } 
    /// <summary>
    ///  ID сервисного контракта
    /// </summary>
    public Guid? ServiceContractID { get; init; } 
    /// <summary>
    /// Возникшие проблемы.
    /// </summary>
    public string Problems { get; init; } 

    /// <summary>
    /// Тип ремонта.
    /// </summary>
    public string RepairType { get; init; } 
    /// <summary>
    /// Стоимость ремонта.
    /// </summary>
    public float Cost { get; init; } 
    /// <summary>
    /// Качество ремонта.
    /// </summary>
    public string Quality { get; init; } 
    /// <summary>
    /// Номер квитанции.
    /// </summary>
    public string Agreement { get; init; } 

    /// <summary>
    /// Присвоить инвентарный номер.
    /// </summary>
    public bool? AssignInventoryNumber { get; init; } 
    /// <summary>
    /// Присвоить инвентарный номер компонентам.
    /// </summary>
    public bool? AssignInventoryNumberToComponents { get; init; } 
    /// <summary>
    /// Присвоить код.
    /// </summary>
    public bool? AssignCode { get; init; } 
    /// <summary>
    /// Присвоить код компонентам.
    /// </summary>
    public bool? AssignCodeToComponents { get; init; } 
}
