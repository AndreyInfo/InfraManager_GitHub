using IM.Core.Import.BLL.Interface.Import;
using InfraManager;
using InfraManager.DAL.OrganizationStructure;
using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import.Models;
using InfraManager.DAL.Import;
using AutoMapper;
using IM.Core.Import.BLL.Interface.Import.View;
using IM.Core.Import.BLL.Comparers;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;

namespace IM.Core.Import.BLL.Import
{
    internal class OrganizationsImportMapper : IBaseImportMapper<OrganizationDetails, Organization>,
        ISelfRegisteredService<IBaseImportMapper<OrganizationDetails, Organization>>
    {
        private readonly IMapper _mapper;
        private readonly IAdditionalParametersForSelect _addittionalParametersForSelect;

        private readonly IImportMapperComparer<OrganizationDetails,Organization> _organizationImportComparer;
        private readonly IImportMapper<OrganizationDetails, Organization> _organizationMapper;
        private readonly IImportParameterLogic<OrganizationDetails, Organization, OrganisationComparisonEnum> _organizationParameterLogic;
        private readonly IImportEntityData<OrganizationDetails, Organization, OrganisationComparisonEnum> _organizationImportEntityData;


        public OrganizationsImportMapper(IMapper mapper,
            ILogger<OrganizationsImportMapper> logger,
            IAdditionalParametersForSelect addittionalParametersForSelect,
            IImportMapperComparer<OrganizationDetails,Organization> organizationImportComparer, 
            IImportMapper<OrganizationDetails, Organization> organizationMapper, 
            IImportParameterLogic<OrganizationDetails, Organization, OrganisationComparisonEnum> organizationParameterLogic, 
            IImportEntityData<OrganizationDetails, Organization, OrganisationComparisonEnum> organizationImportEntityData)
        {
            _mapper = mapper;
            _addittionalParametersForSelect = addittionalParametersForSelect;
            _organizationImportComparer = organizationImportComparer;
            _organizationMapper = organizationMapper;
            _organizationParameterLogic = organizationParameterLogic;
            _organizationImportEntityData = organizationImportEntityData;
        }

        
        public async Task<ImportData<OrganizationDetails, Organization>> Init(IEnumerable<ImportModel> models,
            UISetting setting, CancellationToken token)
        {
            var flags = (ObjectType) setting.ObjectType;
            var additionalDetails = _mapper.Map<AdditionalTabDetails>(setting);
            var fieldCompare = (OrganisationComparisonEnum)additionalDetails.OrganizationComparison;
            var comparerFunc = _organizationImportEntityData.GetComparerFunction(fieldCompare, false);
            var filter = _organizationParameterLogic.ValidateBeforeInitFunc(additionalDetails);
            var detailsKey = _organizationParameterLogic.GetDetailsKey(fieldCompare);
            var modelKey = _organizationParameterLogic.GetModelKey(fieldCompare);
            var changeKey = _organizationImportComparer.IsModelChanged(flags, false);
            var uniqueKeys =
                await _organizationImportEntityData.GetUniqueKeys(flags, additionalDetails.RestoreRemovedUsers, token);
            var validateAfterInitFunc = _organizationParameterLogic.ValidateAfterInitFunc();
            var validateBeforeCreateFunc = _organizationParameterLogic.ValidateBeforeCreate();
            return new ImportData<OrganizationDetails, Organization>(models, flags, additionalDetails,
                setting.RestoreRemovedUsers, comparerFunc, filter, detailsKey,
                modelKey, changeKey, uniqueKeys.ToArray(), validateAfterInitFunc, validateBeforeCreateFunc);
        }

       
        #region Mapping

        public IEnumerable<Organization> Map(ImportData<OrganizationDetails, Organization> data, IEnumerable<OrganizationDetails> details)
        {
            return _organizationMapper.CreateMap(data, details);
        }

        public void Map(ImportData<OrganizationDetails, Organization> data, IEnumerable<(OrganizationDetails, Organization)> updatePairs)
        {
            _organizationMapper.UpdateMap(data, updatePairs);
        }
        
        #endregion
    }
}