using InfraManager.DAL.Import.CSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Import
{
    public interface IImportCSVBLL
    {
        /// <summary>
        /// Метод получает список всех CSV конфигурации 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<CSVConfigurationTableAPI[]> GetConfigurationTableAsync(CancellationToken cancellationToken);
        /// <summary>
        /// Метод получает путь для файла CSV 
        /// </summary>
        /// <param name="id">идентификатор задачи</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> GetPathAsync(Guid id, CancellationToken cancellationToken);
        /// <summary>
        /// ОМетод обновляет путь CSV файла
        /// </summary>
        /// <param name="id">идентификатор задачи</param>
        /// <param name="path">путь до файла</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpdatePathAsync(Guid id, string path, CancellationToken cancellationToken);
    }
}
