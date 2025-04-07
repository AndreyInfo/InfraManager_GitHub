using IM.Core.Import.BLL.Import.Array;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Log;
using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.Import.View;
using InfraManager;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.Extensions.Logging;

namespace IM.Core.Import.BLL.Import;

internal class OrganizationParameterLogic : IImportParameterLogic<OrganizationDetails, Organization, OrganisationComparisonEnum>, ISelfRegisteredService<IImportParameterLogic<OrganizationDetails, Organization, OrganisationComparisonEnum>>
{
    public OrganizationParameterLogic(ILocalLogger<OrganizationParameterLogic> logger)
    {
    }

    public ImportKeyData<OrganizationDetails, Organization> GetModelKey(OrganisationComparisonEnum parameter)
    {
        return parameter switch
        {
            OrganisationComparisonEnum.Name => new ImportKeyData<OrganizationDetails, Organization>(new()
            {
                {
                    x =>new OrganizationNameKey(x.Name),
                    x => new OrganizationNameKey(x.Name)
                }
            }, nameof(Organization.Name)),

            OrganisationComparisonEnum.ExternalID => new ImportKeyData<OrganizationDetails, Organization>(new()
            {
                {
                    x => new ExternalIDKey(x.ExternalId),
                    x => new ExternalIDKey(x.ExternalId)
                }
            }, nameof(Organization.ExternalId)),

            OrganisationComparisonEnum.NameOrExternalID => new ImportKeyData<OrganizationDetails, Organization>(
                new()
                {
                    {
                        x => new OrganizationNameKey(x.Name),
                        x => new OrganizationNameKey(x.Name)
                    },
                    {
                        x => new ExternalIDKey(x.ExternalId),
                        x => new ExternalIDKey(x.ExternalId)
                    }
                }, $"{nameof(Organization.Name)} и {nameof(OrganizationDetails.ExternalId)}"),

            _ => throw new NotSupportedException()
        };
    }

    public Func<OrganizationDetails, bool> ValidateAfterInitFunc() => x => false;
    //todo:разделить слои
    public Func<Organization, bool> ValidateBeforeCreate() => x=> !OrganizationNameKey.IsSet(x.Name);

    public Func<OrganizationDetails, IIsSet> GetDetailsKey(OrganisationComparisonEnum parameter)
    {
        return parameter switch
        {
            OrganisationComparisonEnum.Name => x => new OrganizationNameKey(x.Name),
            OrganisationComparisonEnum.ExternalID => x => new ExternalIDKey(x.ExternalId),
            OrganisationComparisonEnum.NameOrExternalID => x=>new OrganizationNameOrExternalIDKey(x.Name,x.ExternalId),
            _ => throw new NotSupportedException()
        };
    }

    public Func<OrganizationDetails, bool> ValidateBeforeInitFunc(AdditionalTabDetails parameter)
    {
        return (OrganisationComparisonEnum)parameter.OrganizationComparison switch
        {
            OrganisationComparisonEnum.Name => x => !OrganizationNameKey.IsSet(x.Name),
            OrganisationComparisonEnum.ExternalID => x =>  !ExternalIDKey.IsSet(x.ExternalId),
            OrganisationComparisonEnum.NameOrExternalID => x => !OrganizationNameKey.IsSet(x.Name) && !ExternalIDKey.IsSet(x.ExternalId),
            _ => throw new NotSupportedException()
        };
    }
}