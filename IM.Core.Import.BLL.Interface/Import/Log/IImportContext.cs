namespace IM.Core.Import.BLL.Interface.Import.Log;

public interface IImportContext
{
    ILogAdapter? AdditionalContextLogger { get; set; }
}