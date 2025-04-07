using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.Messages;

/// <summary>
/// Бизнес логика с Сообщениями
/// </summary>
public interface IMessageBLL
{

    /// <summary>
    /// Поулчение данных для таблицы
    /// Поддерживает скролинг и поиск, так же сортировку по столбцам
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<MessageDetails[]> GetListByFilterAsync(BaseFilter filter, CancellationToken cancellationToken);

    /// <summary>
    /// Получение данных о сообщение по id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<MessageDetails> GetByIDAsync(Guid id, CancellationToken cancellationToken);
    /// <summary>
    /// Обнвление данных по сообщению
    /// </summary>
    /// <param name="id">Идентификатор сообщения</param>
    /// <param name="model">Данные для обновления</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<MessageDetails> UpdateAsync(Guid id, MessageData model, CancellationToken cancellationToken);
}
