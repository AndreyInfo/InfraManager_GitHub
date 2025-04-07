using System.Collections;
using System.Diagnostics;
using System.Text;

namespace IM.Core.Import.BLL.Interface.Import;

public class ErrorDetails<TDetails>
{
    private readonly Dictionary<TDetails, string> _collection = new();

    public void Add(TDetails details, string message)
    {
        _collection[details] =  message;
    }

    public IEnumerable<string?> GetMessages(Func<TDetails?, string> keyGetter) =>
        _collection.Select(x => $"{keyGetter(x.Key)?.ToString()}: {x.Value}");

    public long Count => _collection.Count;

    public void Clear() => _collection.Clear();
}