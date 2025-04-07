using IM.Core.Import.BLL.Import.Array;
using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models;
using InfraManager;
using InfraManager.DAL.Import;
using InfraManager.DAL.OrganizationStructure;
using IronPython.Modules;

namespace IM.Core.Import.BLL.Import;

internal class SubdivisionImportEntityData : IImportEntityData<ISubdivisionDetails,Subdivision, SubdivisionComparisonEnum>, ISelfRegisteredService<IImportEntityData<ISubdivisionDetails,Subdivision, SubdivisionComparisonEnum>>
{
    private readonly ISubdivisionImportRepository _subdivisionImportRepository;

    private readonly Dictionary<ObjectType, ImportKeyData<ISubdivisionDetails,Subdivision>> _uniqueDetailsChecks = new()
    {
        {ObjectType.SubdivisionName, new ImportKeyData<ISubdivisionDetails,Subdivision>(new()
        {
            {
                x => new SubdivisionRelativeNameKey(x.OrganizationID,x.SubdivisionID,x.Name),
                x=>new SubdivisionRelativeNameKey(x.OrganizationID,x.SubdivisionID,x.Name)
            }
        }, "Имя, организация, подразделение")},
        {ObjectType.SubdivisionExternalID, new ImportKeyData<ISubdivisionDetails, Subdivision>(new()
        {
            {x=>new ExternalIDKey(x.ExternalID), x=>new ExternalIDKey(x.ExternalID)}
        }, nameof(Subdivision.ExternalID))}
    };
    public SubdivisionImportEntityData(ISubdivisionImportRepository subdivisionImportRepository)
    {
        _subdivisionImportRepository = subdivisionImportRepository;
    }

    public Func<ICollection<ISubdivisionDetails>, IAdditionalParametersForSelect, CancellationToken,
        Task<IEnumerable<Subdivision>>> GetComparerFunction(SubdivisionComparisonEnum parameter, bool withRemoved)
    {
        return parameter switch
        {
            SubdivisionComparisonEnum.Name => _subdivisionImportRepository.FromNameAsync,
            SubdivisionComparisonEnum.ExternalID => _subdivisionImportRepository.FromExternalIDAsync,
            SubdivisionComparisonEnum.NameOrExternalID => _subdivisionImportRepository.FromOrganizationsByIDOrNameAsync,
            _ => throw new NotSupportedException()
        };
    }
    
    public Func<ICollection<ISubdivisionDetails>,IAdditionalParametersForSelect, CancellationToken, Task<IEnumerable<Subdivision>>> GetSubdivisionGetterForUniqueKeys(ObjectType type)
    {
        return type switch
        {
            ObjectType.SubdivisionName => _subdivisionImportRepository.FromNameAsync,
            ObjectType.SubdivisionExternalID => _subdivisionImportRepository.FromExternalIDAsync
        };
    }

    public async Task<IEnumerable<IDuplicateKeyData<ISubdivisionDetails, Subdivision>>> GetUniqueKeys(ObjectType flags, bool getRemoved, CancellationToken token)
    {
        return from uniqueCheck in _uniqueDetailsChecks 
            where flags.HasFlag(uniqueCheck.Key) || uniqueCheck.Key == ObjectType.SubdivisionName
            select new DuplicateKeyData<ISubdivisionDetails,Subdivision>(uniqueCheck.Value, GetSubdivisionGetterForUniqueKeys(uniqueCheck.Key));
    }
}