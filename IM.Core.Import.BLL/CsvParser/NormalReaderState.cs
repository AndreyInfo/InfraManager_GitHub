using IM.Core.Import.BLL.Interface.Import.CSV;

namespace IM.Core.Import.BLL.Import.Csv;

internal class NormalReaderState: ICsvReaderState
{
    private readonly IStringReaderStateSwitcher _switcher;

    public NormalReaderState(IStringReaderStateSwitcher switcher)
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
            case '\r':
            case '\n':
                _switcher.Switch(_switcher.NewLine);
                return null;
            case '"':
                throw new InvalidDataException("Кавычки внутри неэкранированного выражения CSV файла");
            default:
                return c;
        }
    }
}