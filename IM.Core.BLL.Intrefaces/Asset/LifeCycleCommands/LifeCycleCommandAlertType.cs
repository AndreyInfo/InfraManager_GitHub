namespace InfraManager.BLL.Asset.LifeCycleCommands;
/// <summary>
/// Тип алерта, при выполнении команды жизненного цикла.
/// </summary>
public enum LifeCycleCommandAlertType
{
    /// <summary>
    /// Не выбран.
    /// </summary>
    None = 0,

    /// <summary>
    /// Инвентарный номер.
    /// </summary>
    InventoryNumber = 1,

    /// <summary>
    /// Код.
    /// </summary>
    Code = 2,
}
