using IM.Core.WebApi.Contracts.Common.Models;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.CrossPlatform.WebApi.Contracts.SoftwareInstallation;
using InfraManager.CrossPlatform.WebApi.Contracts.SoftwareInstallation.Models;
using InfraManager.DAL.Software.Installation;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Software
{
    /// <summary>
    /// Поставщик данных для сущности "Инсталляции"
    /// </summary>
    public interface ISoftwareInstallationBLL
    {
        /// <summary>
        /// Получение списка инсталляций
        /// </summary>
        /// <param name="filter">  фильтер </param>        
        /// <param name="cancellationToken"> токен отмены </param>
        /// <returns> список схем инсталляции </returns>
        Task<SoftwareInstallationListItem[]> GetListAsync(SoftwareInstallationListFilter filter, CancellationToken cancellationToken);
        Task<BaseResult<SoftwareInstallationItem, BaseError>> GetAsync(Guid id, CancellationToken cancellationToken);
        Task<BaseResult<Guid, SoftwareInstallationRules>> SaveAsync(SoftwareInstallationItem item, CancellationToken cancellationToken);
        Task<BaseResult<bool, BaseError>> DeleteDependantAsync(Guid installationID, GuidList list, CancellationToken cancellationToken);
        Task<BaseResult<bool, BaseError>> AddDependantAsync(Guid installationID, GuidList list, CancellationToken cancellationToken);
        Task<BaseResult<List<SoftwareLicenceUseListItem>, BaseError>> GetLicenceUseAsync(Guid id, CancellationToken cancellationToken);
    }
}
