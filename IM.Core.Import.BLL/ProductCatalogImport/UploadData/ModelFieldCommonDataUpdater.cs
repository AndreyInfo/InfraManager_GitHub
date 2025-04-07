using System.Reflection;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.Import.Models.FieldUpdater;
using IM.Core.Import.BLL.Interface.Import.Models.Import;
using IM.Core.Import.BLL.Interface.Import.Models.Settings;
using InfraManager.DAL;

namespace IM.Core.Import.BLL.Import.Importer.UploadData;

public class ModelFieldCommonDataUpdater<TData, TEntity> : IModelFieldUpdater<TData,TEntity>
    where TEntity :IImportModelParameters
    where TData: IModelDataKeys
{
    private readonly ICommonPropertyData<TEntity, CommonData> _commonPropertyData;
    private readonly IFieldsService _fieldsService;
    public ModelFieldCommonDataUpdater(ICommonPropertyData<TEntity, CommonData> commonPropertyData,
        IFieldsService fieldsService)
    {
        _commonPropertyData = commonPropertyData;
        _fieldsService = fieldsService;
    }

    public Task<bool> SetFieldsAsync(TData data, TEntity entity,
        CancellationToken token)
    {
        //TODO:закэшировать
        var entityCommonFields = _fieldsService.GetCommonFields();

        var extended = data.Keys.Except(entityCommonFields);

        var extendedArray = extended as string[] ?? extended.ToArray();
        var extendedProperties = _commonPropertyData.GetProperties(extendedArray);
        
        foreach (var modelProperty in extendedProperties)
        {
            modelProperty.Value.SetValue(entity, data[modelProperty.Key]);
        }

        UpdateExtendedParameters(data, entity, extendedArray, extendedProperties);

        return Task.FromResult(true);
    }

    //TODO:разделить на два независимых класса
    protected virtual void UpdateExtendedParameters(TData data, TEntity entity, string[] extendedArray,
        IReadOnlyDictionary<string, PropertyInfo> extendedProperties)
    { }
}