using IM.Core.Import.BLL.Interface.Import.CSV;

namespace IM.Core.Import.BLL.Import.Csv;

public class NewLineReaderState : ICsvReaderState
{
    private readonly IStringReaderStateSwitcher _csvStringReader;
    private char[] newlineChars = new char[] {'\r', '\n'};
    public NewLineReaderState(IStringReaderStateSwitcher csvStringReader)
    {
        _csvStringReader = csvStringReader;
    }

    public bool IsNewLine => true;

    public char? ReadChar(StreamReader reader, char delimeter)
    {
        char? current;
        do
        {
            _csvStringReader.Switch(_csvStringReader.OnBegin);
            current = _csvStringReader.Current.ReadChar(reader, delimeter);
        } while (!(reader.EndOfStream || current.HasValue));

        return current;
    }
}