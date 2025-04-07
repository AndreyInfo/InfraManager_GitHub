using System.Text;

namespace IM.Core.Import.BLL.Import;

public record PivottedLine
{
    private readonly string _pivotStep;
    private string _fullPivot = string.Empty;
    private string _currentPivot = string.Empty;

    public PivottedLine(string pivotStep)
    {
        _pivotStep = pivotStep;
    }
    
    public void SetPivotDepth(uint n)
    {
        _fullPivot = string.Empty;
        var builder = new StringBuilder();
        for (int i = 0; i < n; i++)
        {
            builder.Append(_pivotStep);
        }

        _fullPivot = builder.ToString();
        _currentPivot = _fullPivot;
    }

    public string GetPivot()
    {
        var pivot =  _currentPivot;
        _currentPivot = string.Empty;
        return pivot;
    }

    public void NewLine()
    {
        _currentPivot = _fullPivot;
    }
    
}