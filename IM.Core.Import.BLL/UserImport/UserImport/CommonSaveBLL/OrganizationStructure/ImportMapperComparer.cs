using InfraManager;
using InfraManager.DAL;

namespace IM.Core.Import.BLL.Import;

internal abstract class ImportMapperComparer<TDetails, TEntity> : IImportMapperComparer<TDetails,TEntity>
{
    protected abstract List<KeyValuePair<ObjectType, Func<TDetails, TEntity, bool>>> Comparers { get; } 

    protected static void IgnoreComparerIf(ObjectType flags,
        ObjectType flag,
        List<KeyValuePair<ObjectType, Func<TDetails, TEntity, bool>>> filedComaprerDictionary
    )
    {
        if (!flags.HasFlag(flag))
        {
            if (filedComaprerDictionary.Any(x=>x.Key.Equals(flag)))
                filedComaprerDictionary.RemoveAll(x=>x.Key.Equals(flag));
        }
    }

    public IEnumerable<KeyValuePair<ObjectType, Func<TDetails, TEntity, bool>>> GetMappingComparer(ObjectType flags)
    {        
        //копия словаря
        var fieldComparers = Comparers.ToList();

        SetIgnoreCompareFields(flags, fieldComparers);

        return fieldComparers;
    }

    protected abstract void SetIgnoreCompareFields(ObjectType flags,
        List<KeyValuePair<ObjectType, Func<TDetails, TEntity, bool>>> fieldComparers);
    

    public Func<TDetails, TEntity, bool> IsModelChanged(ObjectType flags, bool withRemoved)
    {
        
        var changedParts = GetMappingComparer(flags);
        
        Func<TDetails, TEntity, bool>? current = null;
        
        //декорирование
        //все функции через &&
        foreach (var part in changedParts.Select(x=>x.Value))
        {
            if (current == null)
            {
                current = part;
                continue;
            }

            var func = current;
            current = (x, y) => func(x, y) && part(x, y);
        }       
        
        //по умолчанию
        if (current == null)
            return (_, _) => false;
        
        //отрицание
        var tmp = current;
        if (withRemoved && typeof(IMarkableForDelete).IsAssignableFrom(typeof(TEntity)))
        {
            return (x, y) => !tmp(x, y) || ((IMarkableForDelete)y).Removed;
        }
        
        return (x, y) => !tmp(x, y);
    }
}