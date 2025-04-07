using IM.Core.Import.BLL.Interface.Import;
using InfraManager.DAL.Import;
using InfraManager.ServiceBase.WebApiModes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Import
{
    /// <summary>
    /// Интерфейс для работы с импортом независимо от источника
    /// </summary>
    public interface IImportBLL
    {

        /// <summary>
        /// Метод главную вкладку импорта
        /// </summary>
        /// <param name="id">идентификатор задачи</param>
        Task<ImportMainTabDetails> GetMainDetailsAsync(Guid id, CancellationToken cancellationToken);
        /// <summary>
        /// Метод получает все задачи импорта
        /// </summary>
        Task<ImportTasksDetails[]> GetImportTasksAsync(CancellationToken cancellationToken);
        /// <summary>
        /// Метод создает главную вкладку импорта
        /// </summary>
        /// <param name="mainTabDetails">параметры главной вкладки</param>
        Task<Guid> CreateMainDetailsAsync(ImportMainTabDetails mainTabDetails, CancellationToken cancellationToken);
        /// <summary>
        /// Метод обновляет главную вкладку импорта
        /// </summary>
        /// <param name="id">идентификатор задачи</param>
        /// <param name="mainTabDetails">данные главной вкладки для изменения</param>
        Task UpdateMainDetailsAsync(Guid id, ImportMainTabDetails mainTabDetails, CancellationToken cancellationToken);
        /// <summary>
        /// Метод удаляет задачу импорта целиком
        /// </summary>
        /// <param name="id">идентификатор задачи импорта</param>
        Task<DeleteDetails> DeleteTaskAsync(Guid id, CancellationToken cancellationToken);
        /// <summary>
        /// Метод получает вкладку "Дополнительно" задачи импорта
        /// </summary>
        /// <param name="id">идентификатор задачи импорта</param>
        Task<AdditionalTabDetails> GetAdditionalDetailsAsync(Guid id, CancellationToken cancellationToken);
        /// <summary>
        /// Метод обновляет вкладку "Дополнительно" импорта
        /// </summary>
        /// <param name="id">идентификатор задачи</param>
        /// <param name="mainTabDetails">данные дополнительной вкладки для изменения</param>
        Task UpdateAdditionalDetailsAsync(Guid id, AdditionalTabData settings, CancellationToken cancellationToken);
        
    }
}
