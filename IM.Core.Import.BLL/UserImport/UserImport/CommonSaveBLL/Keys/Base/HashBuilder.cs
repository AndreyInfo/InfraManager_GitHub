using System.Collections.Immutable;

namespace IM.Core.Import.BLL.Import.Array;

public class HashBuilder
{
    private int _hash;

    public HashBuilder(int hash)
    {
        _hash = hash;
    }

    public void Add(object? data)
    {
        if (data != null)
            _hash = HashCode.Combine(_hash, data);
    }

    public int Build() => _hash;
}