using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    /// <summary>
    /// Этот интерфейс описывает BLL сервис сущности "Тип массового инцидента"
    /// </summary>
    public interface IMassIncidentTypeBLL
    {
        /// <summary>
        /// Получает список типов массовых инцидентов
        /// </summary>
        /// <param name="filterBy">Критерии сортировки и фильтрации типов массовых инцидентов</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Массив данных типов массовых инцидентов</returns>
        Task<MassIncidentTypeDetails[]> GetDetailsPageAsync(MassIncidentTypeListFilter filterBy, CancellationToken cancellationToken = default);
        /// <summary>
        /// Ищет тип массовых инцидентов
        /// </summary>
        /// <param name="id">Идентификатор типа массовых инцидентов</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Данные типа массовых инцидентов</returns>
        Task<MassIncidentTypeDetails> DetailsAsync(int id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Создает новый тип массовых инцидентов
        /// </summary>
        /// <param name="data">Данные нового типа массовых инцидентов</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Данные нового типа массовых инцидентов</returns>
        Task<MassIncidentTypeDetails> AddAsync(MassIncidentTypeData data, CancellationToken cancellationToken = default);
        /// <summary>
        /// Изменяет тип массовых иницидентов
        /// </summary>
        /// <param name="id">Идентификатор типа массовых инцидентов</param>
        /// <param name="data">Новые данные типа массовых инцидентов</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Данные типа массовых инцидентов после изменения</returns>
        Task<MassIncidentTypeDetails> UpdateAsync(int id, MassIncidentTypeData data, CancellationToken cancellationToken = default);
        /// <summary>
        /// Удаляет тип массовых инцидентов
        /// </summary>
        /// <param name="id">Идентификатор типа массовых инцидентов</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
