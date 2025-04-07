using AutoMapper;
using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.Import.View;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.Import;

namespace IM.Core.Import.BLL.Import;


public class UserImportMapperFacade : IBaseImportMapper<IUserDetails, User>, ISelfRegisteredService<IBaseImportMapper<IUserDetails, User>>
{
    private readonly IMapper _mapper;
    private readonly IImportMapperComparer<IUserDetails,User> _userMapperComparer;
    private readonly IImportMapper<IUserDetails,User> _userMapper;
    private readonly IImportEntityData<IUserDetails,User,UserComparisonEnum>? _userImportEntityData;
    private readonly IImportParameterLogic<IUserDetails,User, UserComparisonEnum> _userParameterLogic;

    public UserImportMapperFacade(IMapper mapper,
        IImportMapperComparer<IUserDetails, User> userMapperComparer,
        IImportMapper<IUserDetails, User> userMapper,
        IImportEntityData<IUserDetails, User, UserComparisonEnum>? userImportEntityData,
        IImportParameterLogic<IUserDetails, User, UserComparisonEnum> userParameterLogic)
    {
        _mapper = mapper;
        _userMapperComparer = userMapperComparer;
        _userMapper = userMapper;
        _userImportEntityData = userImportEntityData;
        _userParameterLogic = userParameterLogic;
    }
    
    public IEnumerable<User> Map(ImportData<IUserDetails, User> data, IEnumerable<IUserDetails> userDetails)
    {
        return _userMapper.CreateMap(data, userDetails);
    }
    
    public void Map(ImportData<IUserDetails, User> data, IEnumerable<(IUserDetails, User)> updatePairs)
    {
        _userMapper.UpdateMap(data, updatePairs);
    }

    public async Task<ImportData<IUserDetails, User>> Init(IEnumerable<ImportModel> models, UISetting setting,
        CancellationToken token)
    {
        var flags = (ObjectType)setting.ObjectType;
        var additionalDetails = _mapper.Map<AdditionalTabDetails>(setting);
        var fieldCompare = (UserComparisonEnum)additionalDetails.UserComparison;
        var withRemoved = additionalDetails.RestoreRemovedUsers;
        var func = _userImportEntityData.GetComparerFunction(fieldCompare, true);
        var preValidate = _userParameterLogic.ValidateBeforeInitFunc(additionalDetails);
        var detailsKey = _userParameterLogic.GetDetailsKey(fieldCompare);
        var userKey = _userParameterLogic.GetModelKey(fieldCompare);
        var changeKey = _userMapperComparer.IsModelChanged(flags, withRemoved);
        var uniqueKeys = (await _userImportEntityData.GetUniqueKeys(flags, withRemoved, token)).ToArray();


        var validateAfterInitFunc = _userParameterLogic.ValidateAfterInitFunc();
        var validateBeforeCreate = _userParameterLogic.ValidateBeforeCreate();
        return new ImportData<IUserDetails, User>(models, flags, additionalDetails, setting.RestoreRemovedUsers, func, preValidate, detailsKey,
            userKey, changeKey,uniqueKeys, validateAfterInitFunc, validateBeforeCreate);
    }
}