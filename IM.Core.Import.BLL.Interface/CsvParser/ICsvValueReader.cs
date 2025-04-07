namespace IM.Core.Import.BLL.Interface.Import.CSV;

public interface ICsvValueReader
{
    bool IsNewLine { get; }

    string ReadValue(StreamReader reader, char delimeter);
}