namespace InfraManager.DAL.ServiceDesk.WorkOrders;

/// <summary>
/// Этот интерфейс описывает обект, который может быть поставлен на контроль исполнителям связанных заданий. 
/// </summary>
public interface IWorkOrderExecutorControl : IHaveWorkOrderReferences
{
    /// <summary>
    /// Возвращает признак, что объект поставлен на контроль исполнителям заданий.
    /// </summary>
    bool OnWorkOrderExecutorControl { get; }
}