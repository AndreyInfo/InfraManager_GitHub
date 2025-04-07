using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    /// <summary>
    /// Этот интерфейс описывает BLL сервис для сущности "Канал приема информации о массовом инциденте"
    /// </summary>
    public interface IMassIncidentInformationChannelBLL
    {
        /// <summary>
        /// Возвращает все каналы приема массовых инцидентов
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Массив данных каналон приема массовых инцидентов</returns>
        Task<LookupListItem<short>[]> AllAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// Возвращает все каналы приема массовых инцидентов
        /// </summary>
        /// <returns>Массив данных каналон приема массовых инцидентов</returns>
        LookupListItem<short>[] All();
        /// <summary>
        /// Ищет канал приема массовых инцидентов
        /// </summary>
        /// <param name="id">Идентификатор канала приема массовых инцидентов</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Данные канала приема массовых инцидентов</returns>
        Task<LookupListItem<short>> FindAsync(short id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Ищет канал приема массовых инцидентов
        /// </summary>
        /// <param name="id">Идентификатор канала приема массовых инцидентов</param>
        /// <returns>Данные канала приема массовых инцидентов</returns>
        LookupListItem<short> Find(short id);
    }
}
