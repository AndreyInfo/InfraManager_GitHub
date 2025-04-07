using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.Import.Models.FieldUpdater;
using IM.Core.Import.BLL.Interface.Import.Models.Import;
using IM.Core.Import.BLL.Interface.Import.Models.Settings;

namespace IM.Core.Import.BLL.Import.Importer.UploadData;

public class EntityFieldModelDataCommonDataUpdater<TData,TEntity> : IEntityUpdater<TData,TEntity>
    where TData:IModelDataTryGet
{
    //TODO:закэшировать
    private readonly ICommonPropertyData<TEntity, CommonData> _commonPropertyData;

    public EntityFieldModelDataCommonDataUpdater(ICommonPropertyData<TEntity, CommonData> commonPropertyData)
    {
        _commonPropertyData = commonPropertyData;
    }

    public Task<bool> SetFieldsAsync(TData data, TEntity entity,
        CancellationToken token)
    {
        var setters = _commonPropertyData.GetProperties();

        var requiredFields = _commonPropertyData.GetRequiredFields().Intersect(setters.Keys).ToHashSet();
        try
        {
            foreach (var setter in setters)
            {
                if (!data.TryGetValue(setter.Key, out var dataElement))
                    continue;
                var type = setter.Value.PropertyType;
                
                var resultValue = Convert.ChangeType(dataElement, type);
                setter.Value.SetValue(entity, resultValue);
                requiredFields.Remove(setter.Key);
            }
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
       
        return Task.FromResult(!requiredFields.Any());
    }
}