using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL.OrganizationStructure;

namespace InfraManager.BLL.OrganizationStructure
{
    public interface IOrganizationBLL
    {
        /// <summary>
        /// Удалить Организацию по IMObjId
        /// </summary>
        /// <param name="organizationId">Id организации</param>
        Task DeleteByIdAsync(Guid organizationId,
            CancellationToken cancellationToken = default);


        [Obsolete("Use DetailsAsync instead")]
        Task<Organization> GetAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение организации по IMObjId
        /// </summary>
        /// <param name="id">Id организации</param>
        Task<OrganizationDetails> GetOrganizationDetailsAsync(Guid id, CancellationToken cancellationToken);
        
        /// <summary>
        /// Добавление новой организации в базу
        /// </summary>
        /// <param name="organization">DTO Организации</param>
        Task<Guid> AddOrganizationAsync(OrganizationData organization, CancellationToken cancellationToken);

        /// <summary>
        /// Обовление организации в базе
        /// </summary>
        /// <param name="id">Идентификатор Организации</param>
        /// <param name="organizationDetails">DTO Организации</param>
        Task UpdateOrganizationAsync(Guid id, OrganizationData organizationDetails, CancellationToken cancellationToken);
        
        /// <summary>
        /// Получение всех организаций
        /// </summary>
        /// <returns>Массив данных организаций</returns>
        Task<OrganizationDetails[]> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получает данные организации по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор организации</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Данные организаций</returns>
        Task<OrganizationDetails> DetailsAsync(Guid id, CancellationToken cancellationToken);
        
        /// <summary>
        /// Получение организаций для таблицы
        /// </summary>
        /// <param name="filter">фильтр поиска организаций</param>
        /// <param name="cancellationToken"></param>
        /// <returns>список организаций</returns>
        Task<OrganizationDetails[]> GetListAsync(BaseFilter filter, CancellationToken cancellationToken);
    }
}
