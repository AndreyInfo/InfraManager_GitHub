using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Manhours
{
    /// <summary>
    /// Бизнес-логика функционала трудозатрат
    /// </summary>
    public interface IManhoursWorkBLL
    {
        /// <summary>
        /// Добавляет описание трудозатраты
        /// </summary>
        /// <param name="parentObject">Информация об объекте, к котрому относится трудозатрата</param>
        /// <param name="data">Информация о трудозатрате</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Состояние трудозатраты</returns>
        Task<ManhoursWorkDetails> AddWorkAsync(InframanagerObject parentObject, ManhoursWorkData data,
            CancellationToken cancellationToken);

        /// <summary>
        /// Обновляет существующую трудозатрату
        /// </summary>
        /// <param name="id">Идентификатор трудозатраты</param>
        /// <param name="data">Информация о трудозатрате</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Обновленное состояние трудозатраты</returns>
        Task<ManhoursWorkDetails> UpdateWorkAsync(Guid id, ManhoursWorkData data, CancellationToken cancellationToken);

        /// <summary>
        /// Добавляет информацию о затраченном времени
        /// </summary>
        /// <param name="id">Идентификатор трудозатраты</param>
        /// <param name="data">Информация о затраченном времени</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Обновленное состояние трудозатраты</returns>
        Task<ManhoursWorkDetails> AddManhourEntryAsync(Guid id, ManhourData data, CancellationToken cancellationToken);

        /// <summary>
        /// Обновляет информацию о затраченном времени
        /// </summary>
        /// <param name="id">Идентификатор трудозатраты</param>
        /// <param name="data">Информация о затраченном времени</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Обновленное состояние трудозатраты</returns>
        Task<ManhoursWorkDetails> UpdateManhourEntryAsync(Guid id, ManhourData data, CancellationToken cancellationToken);

        /// <summary>
        /// Получает список трудозатрат по указанному фильтру
        /// </summary>
        /// <param name="manhoursListFilter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Список трудозатрат</returns>
        Task<IReadOnlyList<ManhoursWorkDetails>> GetWorkDetailsArrayAsync(ManhoursListFilter manhoursListFilter,
            CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет трудозатрату
        /// </summary>
        /// <param name="workID">Идентификатор трудозатраты</param>
        /// <param name="cancellationToken"></param>
        Task DeleteWorkAsync(Guid workID, CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет информацию о затраченном времени
        /// </summary>
        /// <param name="workID">Идентификатор трудозатраты</param>
        /// <param name="id">Идентификатор затраченного времени</param>
        /// <param name="cancellationToken"></param>
        Task DeleteManhourEntryAsync(Guid workID, Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Получает информацию о трудозатрате
        /// </summary>
        /// <param name="id">Идентификатор трудозатраты</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ManhoursWorkDetails> GetWorkDetailsAsync(Guid id, CancellationToken cancellationToken);
    }
}