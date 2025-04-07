namespace IM.Core.Import.BLL.Interface.Import.CSV;

public interface ICsvReaderState
{
    // Char? GotChar(Char c);
    // void GotDoubleQuote();
    bool IsNewLine { get; }
    char? ReadChar(StreamReader reader, char delimeter);
}