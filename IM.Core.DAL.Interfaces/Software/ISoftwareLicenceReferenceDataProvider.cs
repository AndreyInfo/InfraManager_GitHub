using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Software
{
    public interface ISoftwareLicenceReferencesDataProvider
    {
        /// <summary>
        /// Возвращает список связий Лицензий по объекту
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<SoftwareLicenceReference>> GetListForObjectAsync(int objectClassID, Guid objectID, CancellationToken cancellationToken);
    }
}
