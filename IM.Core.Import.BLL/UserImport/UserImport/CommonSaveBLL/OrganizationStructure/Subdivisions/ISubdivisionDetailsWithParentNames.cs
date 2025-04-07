using IM.Core.Import.BLL.Interface.Import;

namespace IM.Core.Import.BLL.Import.Array;

public interface ISubdivisionDetailsWithParentNames:ISubdivisionDetails
{
    IEnumerable<string> SubdivisionParent { get; set; }
}