using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import;
using InfraManager.DAL.Import;
using InfraManager.DAL.OrganizationStructure;

namespace IM.Core.Import.BLL.Import;

public interface ISubdivisionImportRepository
{
    Task<IEnumerable<Subdivision>> FromNameAsync(ICollection<ISubdivisionDetails> subdivisions, IAdditionalParametersForSelect additionalParametersForSelect, CancellationToken cancellationToken = default);
    Task<IEnumerable<Subdivision>> FromExternalIDAsync(ICollection<ISubdivisionDetails> subdivisions, IAdditionalParametersForSelect additionalParametersForSelect, CancellationToken cancellationToken = default);
    Task<IEnumerable<Subdivision>> FromOrganizationsByIDOrNameAsync(ICollection<ISubdivisionDetails> subdivisions, IAdditionalParametersForSelect additionalParametersForSelect, CancellationToken cancellationToken = default);
}