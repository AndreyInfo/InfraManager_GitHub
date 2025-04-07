using IM.Core.Import.BLL.Import;

namespace IM.Core.Import.BLL.Interface.Import.Models.Settings;

public class ScriptDataDetails<T>
{
    public string FieldName => FieldEnum.ToString();

    public string ClassName { get; set; }
    
    public string Script { get; set; }

    public T FieldEnum { get; init; }
    
    public ImportTypeEnum ImportTypeEnum { get; init; }
 
}