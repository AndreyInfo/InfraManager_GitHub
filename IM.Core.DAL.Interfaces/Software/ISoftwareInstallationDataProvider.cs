using InfraManager.DAL.Software.Installation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Software
{
    public interface ISoftwareInstallationDataProvider
    {
        /// <summary>
        /// Возвращает список инсталляций
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>список инсталляций</returns>
        Task<IQueryable<ViewSoftwareInstallation>> GetViewListAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает связь по идентификатор
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<SoftwareInstallation> GetAsync(Guid ID, CancellationToken cancellationToken);
        /// <summary>
        /// Добавляет новый элемент связи
        /// </summary>
        /// <param name="elpsetting"></param>
        void Add(SoftwareInstallation installation);
        /// <summary>
        /// ВОзвращет связь по имени
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<SoftwareInstallation> GetByNameAsync(string name, CancellationToken cancellationToken);
        void Remove(SoftwareInstallation installation);
        Task<IList<SoftwareInstallationDependances>> GetDependancesAsync(Guid installationID, CancellationToken cancellationToken);
        void AddDependant(SoftwareInstallationDependances dependances);
        void RemoveDependant(SoftwareInstallationDependances dependances);

    }
}
