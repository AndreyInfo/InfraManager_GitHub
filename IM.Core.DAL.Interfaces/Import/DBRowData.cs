using System.Collections.Generic;
using System.Text;

namespace InfraManager.DAL.Import;

public class DBRowData
{
    public Dictionary<string, string?> Cells { get; set; } = new();
    
    public dynamic Record { get; }

    public override string ToString()
    {
        var builder = new StringBuilder();
        foreach (var cell in Cells)
        {
            builder.Append(cell.Key).Append(" = ").AppendLine(cell.Value?.ToString() ?? "(null)");
        }

        return builder.ToString();
    }
}