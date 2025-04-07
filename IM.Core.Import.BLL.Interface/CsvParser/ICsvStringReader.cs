namespace IM.Core.Import.BLL.Interface.Import.CSV;

public interface ICsvStringReader
{
    public List<string> GetData(StreamReader reader, char delimeter);
}