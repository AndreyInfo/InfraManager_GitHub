using AutoMapper;
using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.Import.View;
using InfraManager;
using InfraManager.DAL.Import;
using InfraManager.DAL.OrganizationStructure;

namespace IM.Core.Import.BLL.Import
{
    internal class SubdivisionImportMapperFacade : IBaseImportMapper<ISubdivisionDetails, Subdivision>, ISelfRegisteredService<IBaseImportMapper<ISubdivisionDetails, Subdivision>>
    {
        private readonly IMapper _mapper;
        private readonly IImportMapperComparer<ISubdivisionDetails, Subdivision> _subdivisionMapperComparer;
        private readonly IImportMapper<ISubdivisionDetails,Subdivision> _subdivisionMapper;
        private readonly IImportParameterLogic<ISubdivisionDetails, Subdivision, SubdivisionComparisonEnum> _subdivisionParameterLogic;
        private readonly IImportEntityData<ISubdivisionDetails,Subdivision, SubdivisionComparisonEnum> _subdivisionImportEntityData;

        private static ObjectType CommonObjectType = ObjectType.SubdivisionParent | ObjectType.SubdivisionOrganization
            | ObjectType.SubdivisionOrganizationExternalID | ObjectType.SubdivisionParentExternalID;


        public SubdivisionImportMapperFacade(IMapper mapper,
            IImportMapperComparer<ISubdivisionDetails, Subdivision> subdivisionMapperComparer,
            IImportMapper<ISubdivisionDetails, Subdivision> subdivisionMapper,
            IImportParameterLogic<ISubdivisionDetails, Subdivision, SubdivisionComparisonEnum>
                subdivisionParameterLogic,
            IImportEntityData<ISubdivisionDetails, Subdivision, SubdivisionComparisonEnum> subdivisionImportEntityData)
        {
            _mapper = mapper;
            _subdivisionMapperComparer = subdivisionMapperComparer;
            _subdivisionMapper = subdivisionMapper;
            _subdivisionParameterLogic = subdivisionParameterLogic;
            _subdivisionImportEntityData = subdivisionImportEntityData;
        }

        public async Task<ImportData<ISubdivisionDetails, Subdivision>> Init(IEnumerable<ImportModel> models,
            UISetting setting, CancellationToken token)
        {
            var flags = (ObjectType)setting.ObjectType;
            flags |= CommonObjectType;
            var additionalDetails = _mapper.Map<AdditionalTabDetails>(setting);
            var fieldCompare = (SubdivisionComparisonEnum)additionalDetails.SubdivisionComparison;
            var comparerFunc = _subdivisionImportEntityData.GetComparerFunction(fieldCompare, false);
            var validate = _subdivisionParameterLogic.ValidateBeforeInitFunc(additionalDetails);
            var validateAfter = _subdivisionParameterLogic.ValidateAfterInitFunc();
            var detailsKey = _subdivisionParameterLogic.GetDetailsKey(fieldCompare);
            var modelKey = _subdivisionParameterLogic.GetModelKey(fieldCompare);
            var changeKey = _subdivisionMapperComparer.IsModelChanged(flags, false);
            var uniqueKeys =
                await _subdivisionImportEntityData.GetUniqueKeys(flags, additionalDetails.RestoreRemovedUsers, token);
            var validateBeforeCreate = _subdivisionParameterLogic.ValidateBeforeCreate();
            return new ImportData<ISubdivisionDetails, Subdivision>(models, flags, additionalDetails, setting.RestoreRemovedUsers, comparerFunc, validate, detailsKey,
                modelKey, changeKey, uniqueKeys.ToArray(), validateAfter, validateBeforeCreate);
        }

        public IEnumerable<Subdivision> Map(ImportData<ISubdivisionDetails, Subdivision> data,
            IEnumerable<ISubdivisionDetails> details)
        {
            return _subdivisionMapper.CreateMap(data, details);
        }

        public void Map(ImportData<ISubdivisionDetails, Subdivision> data, IEnumerable<(ISubdivisionDetails, Subdivision)> updatePairs)
        {
            _subdivisionMapper.UpdateMap(data, updatePairs);
        }
    }
}
