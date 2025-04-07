namespace IM.Core.Import.BLL.Interface.Import.Models;

public interface IDataFactory<TData>
{
    TData BuildData(string[] header, string[] row);
}