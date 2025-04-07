using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models;
using InfraManager.DAL.OrganizationStructure;

namespace IM.Core.Import.BLL.Interface.OrganizationStructure.Organizations;
/// <summary>
/// Интерфейс для сущности Организация
/// </summary>
public interface IOrganizationsBLL
{
    /// <summary>
    /// Метод создает организации
    /// </summary>
    /// <param name="organizations">список организации</param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns> результат операции </returns>
    public Task<int> CreateOrganizationsAsync(IEnumerable<Organization> organizations,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Метод обновляет организации
    /// </summary>
    /// <param name="updateOrganizations">словарь найденных организации в базе и организации для обновления</param>
    /// <param name="importData"></param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns> результат операции </returns>
    public Task<int> UpdateOrganizationsAsync(Dictionary<OrganizationDetails, Organization> updateOrganizations,
        ImportData<OrganizationDetails, Organization> importData,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// Метод получает список организации по внешнему идентификатору
    /// </summary>
    /// <param name="organizations">список организации</param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns> найденные организации</returns>
    public Task<IEnumerable<Organization>> GetOrganizationsByExternalIDAsync(List<OrganizationDetails> organizations, CancellationToken cancellationToken = default);
    /// <summary>
    /// Метод получает список организации по имени
    /// </summary>
    /// <param name="organizations">список организации</param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns> найденные организации</returns>
    public Task<IEnumerable<Organization>> GetOrganizationsByNameAsync(List<OrganizationDetails> organizations, CancellationToken cancellationToken = default);
    /// <summary>
    /// Метод получает список организации по внутреннему идентификатору или имени
    /// </summary>
    /// <param name="organizations">список организации</param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns> найденные организации</returns>
    public Task<IEnumerable<Organization>> GetOrganizationsByIDOrNameAsync(List<OrganizationDetails> organizations, CancellationToken cancellationToken = default);
    /// <summary>
    /// Метод получает организацию по внутреннему идентификатору
    /// </summary>
    /// <param name="id">идентификатор организации</param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns> найденная организация </returns>
    public Task<Organization> GetOrganizationByIDAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Метод получает организацию по внутреннему идентификатору или имени
    /// </summary>
    /// <param name="organization"></param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <param name="organizations">организация</param>
    /// <returns> найденная организация </returns>
    public Task<Organization?> GetOrganizationByIDOrNameAsync(OrganizationDetails organization,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// Метод насыщает организацию перед созданием
    /// </summary>
    /// <param name="organization">организация для насыщения</param>
    /// <param name="cancellationToken">отмена задачи</param>
    public void EnrichOrganizationForCreate(Organization organization);
}
