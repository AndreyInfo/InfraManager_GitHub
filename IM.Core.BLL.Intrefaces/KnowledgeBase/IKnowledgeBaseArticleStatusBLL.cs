using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.KnowledgeBase;


/// <summary>
/// Абстракция над работой с сущностью Статус базы знания
/// </summary>
public interface IKnowledgeBaseArticleStatusBLL
{
    /// <summary>
    /// Удаляет статус базы знания(проверяет на использования где то удаляемого статуса)
    /// </summary>
    /// <param name="id">Идентификатор статуса базы знания</param>
    /// <param name="cancellationToken"></param>
    Task RemoveAsync(Guid id, CancellationToken cancellationToken = default);
    
    
    /// <summary>
    /// Получение списка статусов базы знаний
    /// </summary>
    /// <param name="filterBy">фильтр</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Статусы базы знания</returns>
    Task<KBArticleStatusDetails[]> GetDetailsArrayAsync(LookupListFilter filterBy,
        CancellationToken cancellationToken = default);

    
    /// <summary>
    /// Получение одного статусы базы знаний
    /// </summary>
    /// <param name="id">Идентификатор статуса базы знания</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Статус базы знаний</returns>
    Task<KBArticleStatusDetails> DetailsAsync(Guid id, CancellationToken cancellationToken = default);

    
    /// <summary>
    /// Добавляет статус базы знаний
    /// </summary>
    /// <param name="data">Входной контракт</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Добавленный статус</returns>
    Task<KBArticleStatusDetails> AddAsync(LookupData data, CancellationToken cancellationToken = default);


    /// <summary>
    /// Изменяет статус базы знаний
    /// </summary>
    /// <param name="id">Идентификатор изменяемого статуса</param>
    /// <param name="data">Входной контракт</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Измененный статус</returns>
    Task<KBArticleStatusDetails> UpdateAsync(Guid id, LookupData data,
        CancellationToken cancellationToken = default);
}