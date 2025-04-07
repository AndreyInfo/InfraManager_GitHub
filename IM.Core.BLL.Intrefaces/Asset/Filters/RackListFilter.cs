using System;

namespace InfraManager.BLL.Asset.Filters;

/// <summary>
/// Фильтр для выборки шкафов
/// </summary>
public class RackListFilter
{
    /// <summary>
    /// Идентификатор комнаты.
    /// </summary>
    public Guid? RoomIMObjID { get; init; }
    
    /// <summary>
    /// Глобальный идентификатор.
    /// </summary>
    public Guid? IMObjID { get; init; }
}