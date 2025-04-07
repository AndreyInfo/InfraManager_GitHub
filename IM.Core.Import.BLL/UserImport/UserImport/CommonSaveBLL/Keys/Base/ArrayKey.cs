using System.Configuration;
using System.Text;

namespace IM.Core.Import.BLL.Import.Array;

internal class ArrayKey<T>: IIsSet,IEquatable<ArrayKey<T>>
{
    private readonly List<T>? _list;

    public ArrayKey(IEnumerable<T> enumerable)
    {
        _list = enumerable as List<T> ?? enumerable?.ToList();
    }
    
    public ArrayKey(IEnumerable<T> enumerable, T additional)
    {
        _list = enumerable?.ToList();
        if (additional!=null)
            _list?.Add(additional);
    }

    public override int GetHashCode()
    {
        return _list?.GetHashCode() ?? 0;
    }

    public override bool Equals(object? obj)
    {
        switch (obj)
        {
            case IEnumerable<T> data when data is not null:
                return AreEquals(data);
            default:
                    return false;
        }
    }

    private bool AreEquals(IEnumerable<T>? data)
    {
        if (data is null)
            return false;
        if (_list is null)
            return false;
        var list = data as List<T> ?? data.ToList();
        if (list.Count != _list.Count)
            return false;
        for (int i = 0; i < list.Count; i++)
            if (!(list[i]?.Equals(_list[i]) ?? false))
                return false;
        return true;
    }

    public override string ToString()
    {
        if (_list == null)
            return "(null)";
        return string.Join('/', _list.Select(x=>LogHelper.ToOutputFormat(x.ToString())));
    }

    public bool IsSet()
    {
        return _list != null &&  _list.Any() && _list.All(x => !string.IsNullOrWhiteSpace(x?.ToString()));
    }

    public string GetKey()
    {
        return ToString();
    }

    public bool Equals(ArrayKey<T>? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return AreEquals(other._list);
    }

    public static bool operator ==(ArrayKey<T> first, ArrayKey<T> second)
    {
        return first?.AreEquals(second?._list) ?? false;
    }

    public static bool operator !=(ArrayKey<T> first, ArrayKey<T> second)
    {
        return first?.AreEquals(second?._list) ?? true;
    }
}