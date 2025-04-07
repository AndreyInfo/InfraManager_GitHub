using IM.Core.Import.BLL.Interface.Import.CSV;

namespace IM.Core.Import.BLL.Import.Csv;

internal class OnDQuoteReaderState:ICsvReaderState
{
    private readonly IStringReaderStateSwitcher _switcher;

    public OnDQuoteReaderState(IStringReaderStateSwitcher switcher)
    {
        _switcher = switcher;
    }

    public bool IsNewLine => false;

    public char? ReadChar(StreamReader reader, char delimeter)
    {
        var c = reader.ReadCharOrThrow();
        if (c == delimeter)
            return null;
        switch (c)
        {
            case '"':
                _switcher.Switch(_switcher.InDoubleQuotes);
                return '"';
            
            case '\r':
            case '\n':
                reader.ReadCharOrThrow();
                _switcher.Switch(_switcher.NewLine);
                return null;
            default:
                throw new InvalidDataException();
        }
    }
}