namespace IM.Core.Import.BLL.Import;

/// <summary>
/// Тип импорта
/// </summary>
public enum ImportTypeEnum : int
{
    /// <summary>
    /// Импорт оргструктуры
    /// </summary>
    Organization = 0,
    
    /// <summary>
    /// Импорт подразделения
    /// </summary>
    Subdivision = 1,
    
    /// <summary>
    /// Импорт пользователей
    /// </summary>
    User = 2,
    
    /// <summary>
    /// Импорт пользователей
    /// </summary>
    ITAsset = 3
}