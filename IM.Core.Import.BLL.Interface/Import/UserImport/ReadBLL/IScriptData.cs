using IM.Core.Import.BLL.Interface.Import.Models.Settings;
using InfraManager;

namespace IM.Core.Import.BLL.Import.OSU;

/// <summary>
/// Содержит функции для работы со скриптами
/// </summary>
public interface IScriptData
{
    /// <summary>
    /// Фильтрует поля исходя из выставленных флагов 
    /// </summary>
    /// <param name="fields">Тип, содержащий информацию о полях и скриптах</param>
    /// <param name="type">Содержит информацию об активных скриптах</param>
    /// <param name="init">Преобразует данные к внутренней структуре</param>
    /// <typeparam name="T">Тип данных источника</typeparam>
    /// <returns>Отфильтрованные данные для обработки скриптов</returns>
    IEnumerable<ScriptDataDetails<ConcordanceObjectType>> GetScriptDataDetailsEnumerable<T>(IEnumerable<T> fields, 
        ConcordanceObjectType type, 
        Func<T, ScriptDataDetails<ConcordanceObjectType>> init);

    /// <summary>
    /// Возвращает список полей объекта, вызываемых в скриптах
    /// </summary>
    /// <param name="scriptDataDetailsEnumerable">Информация о полях импорта и скриптах</param>
    /// <returns>Список полей объекта скриптов</returns>
    HashSet<string?> GetScriptRecordFields(
        IEnumerable<ScriptDataDetails<ConcordanceObjectType>> scriptDataDetailsEnumerable);
}