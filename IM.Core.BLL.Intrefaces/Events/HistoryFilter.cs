using System;

namespace InfraManager.BLL.Events;

/// <summary>
/// Фильтр событий.
/// </summary>
public class HistoryFilter
{
    /// <summary>
    /// Возвращает или задает минимальную дату событий для выгрузки.
    /// </summary>
    public DateTime? DateFrom { get; init; }
    
    /// <summary>
    /// Возвращает или задает максимальную дату событий для выгрузки.
    /// </summary>
    public DateTime? DateTill { get; init; }
    
    /// <summary>
    /// Возвращает или задает список имен параметров объекта, изменения которых должны быть выгружены.
    /// </summary>
    public string[] Parameters { get; init; }
}