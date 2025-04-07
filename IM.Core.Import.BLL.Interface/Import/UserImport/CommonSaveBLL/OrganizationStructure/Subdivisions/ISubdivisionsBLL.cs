using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models;
using InfraManager.DAL.Import;
using InfraManager.DAL.OrganizationStructure;

namespace IM.Core.Import.BLL.Interface.OrganizationStructure.Subdivisions;
/// <summary>
/// Интерфейс для сущности Подразделение
/// </summary>
public interface ISubdivisionsBLL
{
    /// <summary>
    /// Метод создает подразделения
    /// </summary>
    /// <param name="subdivisions">список подразделений</param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns> результат операции </returns>
    public Task<int> CreateSubdivisionsAsync(IEnumerable<Subdivision> subdivisions,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Метод обновляет подразделения
    /// </summary>
    /// <param name="subdivisions">список подразделений для обновления и список подразделени, которые обновятся</param>
    /// <param name="importData"></param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns> результат операции </returns>
    public Task<int> UpdateSubdivisionsAsync(Dictionary<ISubdivisionDetails, Subdivision> subdivisions,
        ImportData<ISubdivisionDetails, Subdivision> importData,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// Метод обновляет подразделение
    /// </summary>
    /// <param name="subdivision">подразделение для обновления</param>
    /// <param name="entity">подразделение, которое обновится</param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns> результат операции </returns>
    public Task UpdateAsync(ISubdivisionDetails subdivision, Subdivision entity, ImportData<ISubdivisionDetails, Subdivision> importData, CancellationToken cancellationToken = default);
    /// <summary>
    /// Метод получает подразделение по идентификатору или имени
    /// </summary>
    /// <param name="subdivision">подразделение</param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns> найденое подразделение </returns>
    public Task<Subdivision> GetSubdivisionByIDOrNameAsync(SubdivisionDetails subdivision, CancellationToken cancellationToken = default);
    /// <summary>
    /// Метод получает подразделение по идентификатору
    /// </summary>
    /// <param name="subdivision">подразделение</param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns> найденое подразделение </returns>
    public Task<Subdivision?> GetSubdivisionByIDAsync(Guid id, CancellationToken cancellationToken = default);
    /// <summary>
    /// Метод насыщает данные сущность Подразделение перед созданием
    /// </summary>
    /// <param name="subdivision">подразделение</param>
    /// <returns> найденные подразделения </returns>
    public void EnrichSubdivisionForCreate(Subdivision subdivision);
}