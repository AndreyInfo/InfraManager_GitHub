using InfraManager.DAL.OrganizationStructure;

namespace IM.Core.Import.BLL.Interface
{
    /// <summary>
    /// Интерфейс для сущности Должность пользователя
    /// </summary>
    public interface IPositionBLL
    {
        /// <summary>
        /// Метод производит создание должности в БД
        /// </summary>
        /// <param name="positionModel">модель должности для сохранения</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> модель должности </returns>

        Task<JobTitle> CreateAsync(PositionModel positionModel, CancellationToken cancellationToken);

        /// <summary>
        /// Метод производит получение должности из БД
        /// </summary>
        /// <param name="positionModel">модель должности для поиска</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> модель должности </returns>

        Task<JobTitle> GetByNameAsync(PositionModel positionModel, CancellationToken cancellationToken);
    }
}
