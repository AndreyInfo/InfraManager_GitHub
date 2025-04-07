using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import;
using InfraManager.DAL.OrganizationStructure;

namespace IM.Core.Import.BLL.Import;

public interface IOrganizationsImportRepository
{
    Task<IEnumerable<Organization>> FromNameAsync(ICollection<OrganizationDetails> organizations,
        IAdditionalParametersForSelect additionalParametersForSelect, CancellationToken cancellationToken = default);

    Task<IEnumerable<Organization>> FromExternalIDAsync(ICollection<OrganizationDetails> organizations,
        IAdditionalParametersForSelect additionalParametersForSelect, CancellationToken cancellationToken = default);

    Task<IEnumerable<Organization>> FromOrganizationsByIDOrNameAsync(
        ICollection<OrganizationDetails> organizations,
        IAdditionalParametersForSelect additionalParametersForSelect, CancellationToken cancellationToken = default);
}