namespace IM.Core.Import.BLL.Interface.Import.CSV;

public interface IStringReaderStateSwitcher : ICsvValueReader
{
    ICsvReaderState Normal { get; }
    ICsvReaderState InDoubleQuotes { get; }
    ICsvReaderState OnDoubleQuote { get; }
    ICsvReaderState OnBegin { get; }
    ICsvReaderState Current { get; }
    ICsvReaderState NewLine { get; }
    void Switch(ICsvReaderState state);
}