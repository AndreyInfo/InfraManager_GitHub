namespace InfraManager.DAL.Documents;

/// <summary>
/// Состояния документа
/// </summary>
public enum DocumentState : short
{
    /// <summary>
    /// Доступен
    /// </summary>
    Available = 0,

    /// <summary>
    /// Редактируется
    /// </summary>
    InUse = 1,

    /// <summary>
    /// Выгружен
    /// </summary>
    InCheckOut = 2
}
