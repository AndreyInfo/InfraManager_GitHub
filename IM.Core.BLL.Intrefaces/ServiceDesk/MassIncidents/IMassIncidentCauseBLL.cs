using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    /// <summary>
    /// Этот интерфейс описывает BLL сервис сущности "Причина массовых инцидентов"
    /// </summary>
    public interface IMassIncidentCauseBLL
    {
        /// <summary>
        /// Получает список причин массовых инцидентов
        /// </summary>
        /// <param name="filterBy">Критерии сортировки и фильтрации данных</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Массив данных причин массовых инцидентов</returns>
        Task<MassIncidentCauseDetails[]> GetDetailsPageAsync(MassIncidentCauseListFilter filterBy, CancellationToken cancellationToken = default);
        /// <summary>
        /// Получает данные причины массового инцидента
        /// </summary>
        /// <param name="id">Идентификатор причины массового инцидента</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Данные причины массового инцидента</returns>
        Task<MassIncidentCauseDetails> DetailsAsync(int id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Добавляет в систему новую причину массовых инцидентов
        /// </summary>
        /// <param name="data">Данные новой причины массовых инцидентов</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Данные созданной причины массовых инцидентов</returns>
        Task<MassIncidentCauseDetails> AddAsync(LookupData data, CancellationToken cancellationToken = default);
        /// <summary>
        /// Изменяет причину массовых инцидентов
        /// </summary>
        /// <param name="id">Идентификатор причины массовых инцидентов</param>
        /// <param name="data">Новое состояние причины массового инцидента</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Данные причины массовых инцидентов после изменения</returns>
        Task<MassIncidentCauseDetails> UpdateAsync(int id, LookupData data, CancellationToken cancellationToken = default);
        /// <summary>
        /// Удаляет причину массовых инцидентов
        /// </summary>
        /// <param name="id">Идентификатор причины массовых инцидентов</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
