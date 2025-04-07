using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.CustomControl;

/// <summary>
/// Этот интерфейс предоставляет постановки на контроль / снятия с контроля. 
/// </summary>
public interface IEditCustomControl
{
    /// <summary>
    /// Поставить на контроль / снять с контроля.
    /// </summary>
    /// <param name="imObject">Заданный объект.</param>
    /// <param name="userID">Уникальный идентификатор пользователя.</param>
    /// <param name="underControl"><c>true</c> - поставить на контроль; <c>false</c> - снять с контроля.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    Task SetCustomControlAsync(InframanagerObject imObject, Guid userID, bool underControl, CancellationToken cancellationToken = default);
}