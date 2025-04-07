using System.Reflection;
using IM.Core.Import.BLL.Interface.Import.Models.SaveOrUpdateData;
using IM.Core.Import.BLL.Interface.Import.Models.Settings;
using InfraManager;
using InfraManager.DAL;

namespace IM.Core.Import.BLL.Import.Importer.UploadData;

public class CommonPropertyData<TEntity,TCommonData> : ICommonPropertyData<TEntity, TCommonData>
{
    private readonly PropertyInfo[] _properties = typeof(TCommonData)
        .GetProperties(BindingFlags.Public |
                       BindingFlags.Instance |
                       BindingFlags.SetProperty)
        .Where(x => x.HasAttribute<FieldAssociateNameAttribute>()
                    && x.HasAttribute<ImportFieldAttribute>()).ToArray();

    public IReadOnlyDictionary<string, PropertyInfo> GetProperties()
    {
        return ReadOnlyDictionaryInternal(false);
    }

    private IReadOnlyDictionary<string, PropertyInfo> ReadOnlyDictionaryInternal(bool selectRequiredOnly)
    {
        var result = new Dictionary<string, PropertyInfo>();

        var type = typeof(TEntity);

        var searchingEntityPropertiesByName = GetEntityPropertiesByName(type);
        
        var searchingEntityName = type.HasAttribute<ImportTypeAttribute>()? 
            type.GetCustomAttribute<ImportTypeAttribute>()?.Name: 
            typeof(TEntity).Name;


        foreach (var commonDataProperty in _properties)
        {
            var importFieldAttribute = commonDataProperty.GetAttribute<ImportFieldAttribute>();
            var isRequired = importFieldAttribute.Required;
            var importFieldName = importFieldAttribute.Name;

            var fieldAssociateNameAttribute = commonDataProperty.GetAttribute<FieldAssociateNameAttribute>();
            var className = fieldAssociateNameAttribute.ClassName;
            var fieldName = fieldAssociateNameAttribute.FieldName;
            
            if (selectRequiredOnly && !isRequired)
                continue;
            
            if (className != searchingEntityName)
                continue;
            
            if (!searchingEntityPropertiesByName.TryGetValue(fieldName, out var property))
                continue;


            result[importFieldName] = property;
        }

        return result;
    }

    private static Dictionary<string, PropertyInfo> GetEntityPropertiesByName(Type type)
    {
        var searchingEntityPropertiesByName = type.GetProperties(BindingFlags.Public |
                                                                 BindingFlags.Instance |
                                                                 BindingFlags.SetProperty)
            .ToDictionary(x => x.Name);
        return searchingEntityPropertiesByName;
    }

    public HashSet<string> GetRequiredFields()
    {
        return (from data in _properties
            where data.HasAttribute<ImportFieldAttribute>()
            let attribute = data.GetAttribute<ImportFieldAttribute>()
            where attribute.Required
            select attribute.Name).ToHashSet();
    }

    public string? GetKeyName(string className, string propertyName)
    {
        return (from data in _properties
            where data.HasAttribute<ImportFieldAttribute>() && data.HasAttribute<FieldAssociateNameAttribute>()
            let attribute = data.GetAttribute<FieldAssociateNameAttribute>()
            let attributeImport = data.GetAttribute<ImportFieldAttribute>()
            where attribute.ClassName == className && attribute.FieldName == propertyName
            select attributeImport.Name).SingleOrDefault();
    }

    public IReadOnlyDictionary<string, PropertyInfo> GetProperties(IEnumerable<string> names)
    {
        var properties = GetEntityPropertiesByName(typeof(TEntity));
        var result = properties.Where(x => names.Contains(x.Key))
            .ToDictionary(x => x.Key, x => x.Value);
        return result;
    }
}