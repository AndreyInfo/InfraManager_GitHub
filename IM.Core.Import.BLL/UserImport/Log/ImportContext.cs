using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import.Log;

namespace IM.Core.Import.BLL.UserImport.Log;

public class ImportContext : IImportContext
{
    public ILogAdapter AdditionalContextLogger { get; set; }
}