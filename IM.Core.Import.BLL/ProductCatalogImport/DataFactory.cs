using IM.Core.Import.BLL.Interface.Import.Models;

namespace IM.Core.Import.BLL.Import.Importer;

public class DataFactory:IDataFactory<TmpModelData>
{
    //TODO:сделать легковес
    public TmpModelData BuildData(string[] header, string[] row)
    {
        var dictionary = header.Zip(row).ToDictionary(x => x.First, x => x.Second);
        var result = new TmpModelData(dictionary);
        return result;
    }
}