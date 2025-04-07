using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ServiceDesk.WorkOrders;

namespace InfraManager.BLL.ServiceDesk.CustomControl;

/// <summary>
/// Этот интерфейс предоставляет методы для постановки на контроль /снятия с контроля заданного объекта.
/// </summary>
public interface IModifyWorkOrderExecutorControl
{
    /// <summary>
    /// Поставить на контроль указанный объект если требуется.
    /// </summary>
    /// <param name="objectID">Уникальный идентификатор объекта.</param>
    /// <param name="userID">Уникальный идентификатор пользователя.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    Task SetUnderControlIfNeededAsync(Guid objectID, Guid userID, CancellationToken cancellationToken = default);

    /// <summary>
    /// Снять с контроля связанный с заданием объект если требуется.
    /// </summary>
    /// <param name="reference">Ссылка на Задание.</param>
    /// <param name="userID">Уникальный идентификатор пользователя, у которого снимается с контроля.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    Task RemoveUnderControlIfNeededAsync(WorkOrderReference reference, Guid userID, CancellationToken cancellationToken = default);

    /// <summary>
    /// Поставить / снять на контроля исполнителям связанных с объектом заданий.
    /// </summary>
    /// <param name="objectID">Уникальный идентификатор объекта.</param>
    /// <param name="underControl"><c>true</c> - поставить на контроль; <c>false</c> - снять с контроля.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    Task SetUnderControlAsync(Guid objectID, bool underControl, CancellationToken cancellationToken);
}