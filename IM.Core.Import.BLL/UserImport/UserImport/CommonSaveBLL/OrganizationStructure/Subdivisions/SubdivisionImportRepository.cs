using IM.Core.Import.BLL.Comparers;
using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.Import;
using InfraManager.DAL.OrganizationStructure;

namespace IM.Core.Import.BLL.Import;

internal class SubdivisionImportRepository : ISubdivisionImportRepository, ISelfRegisteredService<ISubdivisionImportRepository>
{
    private readonly IRepository<Subdivision> _repository;
    public SubdivisionImportRepository(IRepository<Subdivision> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Subdivision>> FromNameAsync(ICollection<ISubdivisionDetails> subdivisions, IAdditionalParametersForSelect additionalParametersForSelect, CancellationToken cancellationToken = default)
    {
        var subdivisionsNames = subdivisions.Select(x => x.Name);
        return await _repository.ToArrayAsync(x => subdivisionsNames.Contains(x.Name), cancellationToken);
    }

    public async Task<IEnumerable<Subdivision>> FromExternalIDAsync(ICollection<ISubdivisionDetails> subdivisions, IAdditionalParametersForSelect additionalParametersForSelect, CancellationToken cancellationToken = default)
    {
        var subdivisionsExternalIDs = subdivisions.Select(x => x.ExternalID);
        return await _repository.ToArrayAsync(x => subdivisionsExternalIDs.Contains(x.ExternalID), cancellationToken);
    }

    public async Task<IEnumerable<Subdivision>> FromOrganizationsByIDOrNameAsync(ICollection<ISubdivisionDetails> subdivisions, IAdditionalParametersForSelect additionalParametersForSelect, CancellationToken cancellationToken = default)
    {
        List<Subdivision> resultSubdivisions = new List<Subdivision>();
        var subdivisionsByExternalID = await FromExternalIDAsync(subdivisions, additionalParametersForSelect, cancellationToken);
        var subdivisionsByName = await FromNameAsync(subdivisions, additionalParametersForSelect, cancellationToken);
        resultSubdivisions.AddRange(subdivisionsByExternalID);
        resultSubdivisions.AddRange(subdivisionsByName);

        return (resultSubdivisions.Count() > 1)
            ? resultSubdivisions.Distinct(new SubdivisionComparer()).ToList()
            : resultSubdivisions;
    }
}