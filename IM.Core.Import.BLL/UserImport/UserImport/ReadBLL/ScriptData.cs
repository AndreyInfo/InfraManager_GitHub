using System.Reflection;
using System.Text.RegularExpressions;
using IM.Core.Import.BLL.Interface.Import.Models.Settings;
using IM.Core.Import.BLL.Interface.Import.View;
using InfraManager;
using Inframanager.DAL.ActiveDirectory.Import;

namespace IM.Core.Import.BLL.Import.OSU;

internal class ScriptData : IScriptData, ISelfRegisteredService<IScriptData>
{
    private static ConcordanceObjectType _allEnums = GetAllInputConcordances();

    /// <summary>
    /// Установка непередаваемых через интерфейс флагов
    /// </summary>
    /// <returns></returns>
    private static ConcordanceObjectType GetAllInputConcordances()
    {
        var typeNames =
            typeof(PolledObjectDetails).GetProperties(BindingFlags.Public | BindingFlags.Instance |
                                                      BindingFlags.GetProperty);
        var enums = typeNames.Select(x => System.Enum.Parse<ConcordanceObjectType>(x.Name));
        var allEnums = enums.FirstOrDefault();
        foreach (var current in enums.Skip(1))
        {
            allEnums |= current;
        }

        return allEnums;
    }
    
    public IEnumerable<ScriptDataDetails<ConcordanceObjectType>> GetScriptDataDetailsEnumerable<T>(IEnumerable<T> fields,
        ConcordanceObjectType type, Func<T, ScriptDataDetails<ConcordanceObjectType>> init)
    {
        IEnumerable<ScriptDataDetails<ConcordanceObjectType>> scriptDataDetailsEnumerable = fields.Select(init).ToArray();

        if (!type.HasFlag(ConcordanceObjectType.Organization))
            scriptDataDetailsEnumerable =
                scriptDataDetailsEnumerable.Where(x => !x.FieldEnum.HasFlag(ConcordanceObjectType.Organization));
        if (!type.HasFlag(ConcordanceObjectType.Subdivision))
            scriptDataDetailsEnumerable =
                scriptDataDetailsEnumerable.Where(x => !x.FieldEnum.HasFlag(ConcordanceObjectType.Subdivision));
        if (!type.HasFlag(ConcordanceObjectType.User))
            scriptDataDetailsEnumerable =
                scriptDataDetailsEnumerable.Where(x => !x.FieldEnum.HasFlag(ConcordanceObjectType.User));

        var dataDetailsEnumerable = scriptDataDetailsEnumerable.Where(x =>!_allEnums.HasFlag(x.FieldEnum) || type.HasFlag(x.FieldEnum));
        scriptDataDetailsEnumerable = dataDetailsEnumerable.ToArray();
        return scriptDataDetailsEnumerable;
    }
    
    public HashSet<string?> GetScriptRecordFields(IEnumerable<ScriptDataDetails<ConcordanceObjectType>> scriptDataDetailsEnumerable)
    {
        return new HashSet<string?>();
    }
}