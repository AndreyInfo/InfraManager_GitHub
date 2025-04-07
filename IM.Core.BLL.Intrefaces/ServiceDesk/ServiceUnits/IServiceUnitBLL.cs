using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.ServiceUnits;

/// <summary>
/// Бизнес логика по сервисным блокам
/// </summary>
public interface IServiceUnitBLL
{

    /// <summary>
    /// Получение сервисного блока по идентификатору, со всеми исполнителями
    /// Если не существует, то выбрасывает ошибку ObjectNotFoundException
    /// </summary>
    /// <param name="id">идентификатор сервисного блока</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Модель сервисного блока</returns>
    Task<ServiceUnitDetails> GetByIDAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Добавление со всеми исполнителями
    /// </summary>
    /// <param name="serviceUnitDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>добавленная модель</returns>
    public Task<ServiceUnitDetails> AddAsync(ServiceUnitInsertDetails serviceUnitDetails, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление вместе со списком исполнителей
    /// </summary>
    /// <param name="serviceUnitDetails">обновленная модель</param>
    /// <param name="id">идентификатор обновляемой сущности</param>
    /// <param name="cancellationToken"></param>
    /// <returns>обновленная сущность</returns>
    public Task<ServiceUnitDetails> UpdateAsync(ServiceUnitDetails serviceUnitDetails, Guid id, CancellationToken cancellationToken);



}
