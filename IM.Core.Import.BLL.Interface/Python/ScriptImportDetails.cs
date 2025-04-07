namespace IM.Core.Import.BLL.Interface.Import;

public class ScriptImportDetails
{
    /// <summary>
    /// Название поля результата
    /// </summary>
    public string FieldName { get; init; }

    /// <summary>
    /// Скрипт для вычисления результата
    /// </summary>
    public string Script { get; init; }
}