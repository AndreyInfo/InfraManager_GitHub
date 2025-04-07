using System.Text;
using IM.Core.Import.BLL.Interface.Import.CSV;
using IM.Core.Import.BLL.Interface.Import.Models.Import;
using IM.Core.Import.BLL.Interface.Import.Models.Settings;
using InfraManager;

namespace IM.Core.Import.BLL.Import.Csv;

public class CsvValueReader: IStringReaderStateSwitcher,ISelfRegisteredService<ICsvValueReader>
{
    public ICsvReaderState Normal { get; }

    public ICsvReaderState InDoubleQuotes { get; }

    public ICsvReaderState OnDoubleQuote { get; }
    
    public ICsvReaderState OnBegin { get; }

    public ICsvReaderState Current { get; private set; }

    public ICsvReaderState NewLine { get; }

    public readonly IScriptDataBLL _optionsBLL;
    
    public bool IsNewLine => Current.IsNewLine;

    public CsvValueReader()
    {
        Normal = new NormalReaderState(this);
        InDoubleQuotes = new InDQuoteReaderState(this);
        OnDoubleQuote = new OnDQuoteReaderState(this);
        OnBegin = new OnBeginReaderState(this);
        NewLine = new NewLineReaderState(this);
        Current = OnBegin;
    }
    
    public string ReadValue(StreamReader reader, char delimeter)
    {
        var stringBuilder = new StringBuilder();
        while (!reader.EndOfStream)
        {
            char? c = Current.ReadChar(reader, delimeter);
            if (c.HasValue)
                stringBuilder.Append(c);
            else
                return stringBuilder.ToString();
        }

        return string.Empty;
    }

   
    public void Switch(ICsvReaderState state)
    {
        Current = state;
    }
}