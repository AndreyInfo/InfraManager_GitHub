namespace InfraManager.DAL.ProductCatalogue.LifeCycles;
public enum LifeCycleTransitionMode : byte
{
    /// <summary>
    /// По умолчанию
    /// </summary>
    ElseOrDefault = 0,

    /// <summary>
    /// Размещение на склад
    /// </summary>
    NewLocationIsStorage = 1,

    /// <summary>
    /// Объект является оборудованием
    /// </summary>
    TargetIsDevice = 2
}