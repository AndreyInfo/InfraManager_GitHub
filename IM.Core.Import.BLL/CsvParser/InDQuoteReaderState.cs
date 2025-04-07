using IM.Core.Import.BLL.Interface.Import.CSV;

namespace IM.Core.Import.BLL.Import.Csv;

public class InDQuoteReaderState : ICsvReaderState
{
    private readonly IStringReaderStateSwitcher _csvStringReader;

    public InDQuoteReaderState(IStringReaderStateSwitcher csvStringReader)
    {
        _csvStringReader = csvStringReader;
    }

    public bool IsNewLine => false;

    public char? ReadChar(StreamReader reader, char delimeter)
    {
        var c = reader.ReadCharOrThrow();
        
        if (c != '"') return c;
        
        _csvStringReader.Switch(_csvStringReader.OnDoubleQuote);
        
        return _csvStringReader.Current.ReadChar(reader, delimeter);
    }
}