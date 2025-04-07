using System.Reflection;

namespace IM.Core.Import.BLL.Interface.Import.Models.Settings;

public interface ICommonPropertyData<TEntity, TCommonData>
{
    IReadOnlyDictionary<string, PropertyInfo> GetProperties();
    HashSet<string> GetRequiredFields();

    string? GetKeyName(string className, string propertyName);

    IReadOnlyDictionary<string, PropertyInfo> GetProperties(IEnumerable<string> names);
}