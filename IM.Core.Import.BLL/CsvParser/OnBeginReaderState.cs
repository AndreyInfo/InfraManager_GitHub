using IM.Core.Import.BLL.Interface.Import.CSV;

namespace IM.Core.Import.BLL.Import.Csv;

internal class OnBeginReaderState:ICsvReaderState
{
    private readonly IStringReaderStateSwitcher _switcher;

    public OnBeginReaderState(IStringReaderStateSwitcher switcher)
    {
        _switcher = switcher;
    }

    public bool IsNewLine => false;

    public char? ReadChar(StreamReader reader, char delimeter)
    {
        var c = reader.ReadCharOrThrow();
        switch (c)
        {
            case '"':
                _switcher.Switch(_switcher.InDoubleQuotes);
                return _switcher.Current.ReadChar(reader, delimeter);
            case '\r':
            case '\n':
                _switcher.Switch(_switcher.NewLine);
                return null;
            default:
                _switcher.Switch(_switcher.Normal);
                return c;
        }
    }
}