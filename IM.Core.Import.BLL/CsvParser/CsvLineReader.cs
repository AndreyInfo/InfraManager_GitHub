using IM.Core.Import.BLL.Interface.Import.CSV;
using InfraManager;

namespace IM.Core.Import.BLL.Import.Csv;

public class CsvLineReader:ICsvStringReader,ISelfRegisteredService<ICsvStringReader>
{
    private readonly ICsvValueReader _valueReader;

    public CsvLineReader(ICsvValueReader valueReader)
    {
        _valueReader = valueReader;
    }

    public List<string> GetData(StreamReader reader, char delimeter)
    {
        var list = new List<string>();
        if (!reader.EndOfStream)
        {
            var value = _valueReader.ReadValue(reader, delimeter);
            list.Add(value);
        }
        while (!(reader.EndOfStream || _valueReader.IsNewLine))
        {
            var value = _valueReader.ReadValue(reader, delimeter);
            list.Add(value);
        }

        return list;
    }
}