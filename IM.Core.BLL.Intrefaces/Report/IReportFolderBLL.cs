using InfraManager.DAL;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Report
{
    public interface IReportFolderBLL
    {
        /// <summary>
        /// Получение списка папок с отчетами
        /// </summary>
        /// <param name="filter">Фильтр сортировки</param>
        /// <param name="cancellationToken"> токен отмены </param>
        /// <returns> список папок </returns>
        Task<ReportFolderDetails[]> GetReportFoldersAsync(ReportFolderFilter filter,
            CancellationToken cancellationToken);
        /// <summary>
        /// Получение папки с отчетами по идентификатору
        /// </summary>
        /// <param name="id"> идентификатор папки</param>
        /// <param name="cancellationToken"> токен отмены </param>
        /// <returns> отчет </returns>
        Task<ReportFolderDetails> GetReportFolderAsync(Guid id, CancellationToken cancellationToken);
        /// <summary>
        /// Удаление папки отчетов из базы данных
        /// </summary>
        /// <param name="id"> идентификатор папки</param>
        /// <param name="cancellationToken"> токен отмены </param>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        /// <summary>
        /// Добавление папки отчетов в базу данных
        /// </summary>
        /// <param name="folder"> папка</param>
        /// <param name="cancellationToken"> токен отмены </param>
        /// <returns>Модель созданной папки</returns>

        Task<ReportFolderDetails> InsertAsync(ReportFolderData folder, CancellationToken cancellationToken);
        /// <summary>
        /// Обновление папки отчетов в базе данных
        /// </summary>
        /// <param name="folder"> папка</param>
        /// <param name="cancellationToken"> токен отмены </param>
        Task PutAsync(Guid ID, ReportFolderData folder, CancellationToken cancellationToken);
        
        /// <summary>
        /// Получение идентификаторов всех дочернихпоколений, включая родительский
        /// </summary>
        /// <param name="parentFolderID">ID папки, от которой начинаем поиск дочерних</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Идентификаторы дочерних папок всех поколений</returns>
        Task<Guid[]> GetAllGenerationsChildsFoldersIDsAsync(Guid parentFolderID, CancellationToken cancellationToken);
    }
}
