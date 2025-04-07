using IM.Core.Import.BLL.Import.Array;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Log;
using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.Import.View;
using InfraManager;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DataStructures.Graphs;
using IronPython.Modules;

namespace IM.Core.Import.BLL.Import;

internal class SubdivisionParameterLogic : IImportParameterLogic<ISubdivisionDetails, Subdivision, SubdivisionComparisonEnum>, ISelfRegisteredService<IImportParameterLogic<ISubdivisionDetails, Subdivision, SubdivisionComparisonEnum>>
{
    private readonly ILocalLogger<SubdivisionParameterLogic> _logger;

    public SubdivisionParameterLogic(ILocalLogger<SubdivisionParameterLogic> logger)
    {
        _logger = logger;
    }

   

    public ImportKeyData<ISubdivisionDetails,Subdivision> GetModelKey(SubdivisionComparisonEnum parameter)
    {
        return parameter switch
        {
            SubdivisionComparisonEnum.Name => new ImportKeyData<ISubdivisionDetails, Subdivision>(new()
            {
                {
                    x =>new SubdivisionRelativeNameKey(x.OrganizationID, x.SubdivisionID, x.Name),
                    x=>new SubdivisionRelativeNameKey(x.OrganizationID,x.SubdivisionID,x.Name)

                }
            }, nameof(Subdivision.Name)),
                
            SubdivisionComparisonEnum.ExternalID => new ImportKeyData<ISubdivisionDetails, Subdivision>(new()
            {
                {
                    x => new ExternalIDKey(x.ExternalID), 
                    x=>new ExternalIDKey(x.ExternalID)
                }
            }, nameof(Subdivision.ExternalID)),
            //todo: проверить эквивалентность ключей
            SubdivisionComparisonEnum.NameOrExternalID =>new ImportKeyData<ISubdivisionDetails, Subdivision>( new()
            {
                {
                    x => new SubdivisionSimpleNameKey(x.Name),
                    x=>new SubdivisionSimpleNameKey(x.Name)
                },
                {
                    x=> new ExternalIDKey(x.ExternalID),
                    x=>new ExternalIDKey(x.ExternalID)
                }
            }, $"{nameof(Subdivision.Name)} и {nameof(Subdivision.ExternalID)}"),
                
            _ => throw new NotSupportedException()
        };
    }

    public Func<ISubdivisionDetails, IIsSet> GetDetailsKey(SubdivisionComparisonEnum parameter)
    {
        return parameter switch
        {
            SubdivisionComparisonEnum.Name => x => new SubdivisionRelativeNameKey(x.OrganizationID,x.SubdivisionID, x.Name),
            SubdivisionComparisonEnum.ExternalID => x => new ExternalIDKey(x.ExternalID),
            SubdivisionComparisonEnum.NameOrExternalID => x=>new SubdivisionNameOrExternalIDKey(x.OrganizationID,x.SubdivisionID,x.Name, x.ExternalID),
            _ => throw new NotSupportedException()
        };
    }

    public Func<ISubdivisionDetails, bool> ValidateAfterInitFunc() => x =>
        !x.OrganizationID.HasValue;

    public Func<Subdivision, bool> ValidateBeforeCreate() => x => x.OrganizationID == default||
                                                                          string.IsNullOrWhiteSpace(x.Name);
    
    public Func<ISubdivisionDetails, bool> ValidateBeforeInitFunc(AdditionalTabDetails data)
    {
        return (SubdivisionComparisonEnum)data.SubdivisionComparison switch
        {
            SubdivisionComparisonEnum.Name =>x=> string.IsNullOrWhiteSpace(x.Name),
            SubdivisionComparisonEnum.ExternalID => x => string.IsNullOrWhiteSpace(x.ExternalID),
            SubdivisionComparisonEnum.NameOrExternalID => x=>string.IsNullOrWhiteSpace(x.Name) && string.IsNullOrWhiteSpace(x.ExternalID),
            _ => throw new NotSupportedException()
        };
    }
}