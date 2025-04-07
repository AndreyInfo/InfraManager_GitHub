using IM.Core.Import.BLL.Interface.Import;

namespace IM.Core.Import.BLL.Import.CheckNotFoundFields;

public class FieldNotFoundException:Exception
{
    public string FieldName { get; init; }
    
    public CheckerTypeEnum CheckerTypeEnum { get; init; }

    public FieldNotFoundException(string fieldName, CheckerTypeEnum checkerType)
        : base($"При парсинге {System.Enum.GetName(typeof(CheckerTypeEnum), checkerType)} скрипта не найдено поле {fieldName}")
    {
        FieldName = fieldName;
        CheckerTypeEnum = checkerType;
    }
}