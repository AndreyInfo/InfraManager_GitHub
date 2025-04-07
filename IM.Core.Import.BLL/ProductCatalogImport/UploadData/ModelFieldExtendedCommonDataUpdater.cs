using System.Reflection;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.Import.Models.Import;
using IM.Core.Import.BLL.Interface.Import.Models.Settings;
using InfraManager.DAL;
using Newtonsoft.Json;

namespace IM.Core.Import.BLL.Import.Importer.UploadData;

public class ModelFieldExtendedCommonDataUpdater<TData,TEntity>:ModelFieldCommonDataUpdater<TData,TEntity>
where TEntity:IImportExtendedParameters,IImportModelParameters
where TData:IModelDataKeys
{
    public ModelFieldExtendedCommonDataUpdater(ICommonPropertyData<TEntity, CommonData> commonPropertyData,
        IFieldsService fieldsService) 
        : base(commonPropertyData, fieldsService)
    {
    }

    protected override void UpdateExtendedParameters(TData data, TEntity entity, string[] extendedArray,
        IReadOnlyDictionary<string, PropertyInfo> extendedProperties)
    {
        var parametersData = new Dictionary<string, string>();

        var notModelProperties = extendedArray.Except(extendedProperties.Keys).ToList();

        foreach (var parameterProperty in notModelProperties)
        {
            parametersData[parameterProperty] = data[parameterProperty];
        }

        var parameterResult = JsonConvert.SerializeObject(parametersData);

        entity.Parameters = parameterResult;
    }
}