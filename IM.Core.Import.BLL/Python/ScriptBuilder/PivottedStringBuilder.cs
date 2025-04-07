using System.Text;

namespace IM.Core.Import.BLL.Import;

public class PivottedStringBuilder
{
    private const string PivotStep = " ";
    private uint _pivotDepth = 0;

    private readonly StringBuilder _builder;
    private readonly PivottedLine _line;

    public PivottedStringBuilder(StringBuilder builder)
    {
        _builder = builder;
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        _line = new PivottedLine(PivotStep);
    }

    public void AddPivot(uint n = 1)
    {
        _pivotDepth += n;
        _line.SetPivotDepth(_pivotDepth);
    }
    
    public void SubPivot(uint n = 1)
    {
        _pivotDepth -= n;
        _line.SetPivotDepth(_pivotDepth);
    }

    public PivottedStringBuilder Append(object? data = null)
    {
        _builder.Append(_line.GetPivot()).Append(data?.ToString() ?? string.Empty);
        return this;
    }

    public PivottedStringBuilder Append(char c)
    {
        _builder.Append(_line.GetPivot()).Append(c);
        return this;
    }

    public PivottedStringBuilder AppendLine(object? data = null)
    {
        Append(data);
        _builder.AppendLine();
        _line.NewLine();
        return this;
    }

    public override string ToString()
    {
        return _builder.ToString();
    }
}