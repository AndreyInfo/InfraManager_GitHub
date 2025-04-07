using System.Management;

namespace IM.Core.Import.BLL.Import.Array;

internal class OrKeys<TFirst,TSecond>:IEquatable<OrKeys<TFirst,TSecond>>,IIsSet
    where TFirst:IIsSet,IEquatable<TFirst>
    where TSecond:IIsSet,IEquatable<TSecond>
{
    public OrKeys(TFirst key, TSecond orKey)
    {
        Key = key;
        OrKey = orKey;
    }

    public TFirst Key { get; init; } 
    public TSecond OrKey { get; init; }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
            return true;
        switch (obj)
        {
            case OrKeys<TFirst,TSecond> keys when keys is not null:
                return AreEqual(keys);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Key, OrKey);
    }

    public override string? ToString()
    {
        return Key.IsSet() ? Key.ToString() : OrKey.ToString();
    }

    public bool IsSet()
    {
        return Key.IsSet() || OrKey.IsSet();
    }

    public string GetKey()
    {
        if (Key.IsSet())
            return Key.GetKey();
        return OrKey.GetKey();
    }

    private bool AreEqual(OrKeys<TFirst,TSecond> other)
    {
        return (Key.IsSet() && Key.Equals(other.Key)) ||
               (OrKey.IsSet() && OrKey.Equals(other.OrKey));
    }

    public bool Equals(OrKeys<TFirst,TSecond>? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return AreEqual(other);
    }

    public static bool operator ==(OrKeys<TFirst,TSecond>? first, OrKeys<TFirst,TSecond> second)
    {
        return first.Equals(second);
    }

    public static bool operator !=(OrKeys<TFirst,TSecond> first, OrKeys<TFirst,TSecond> second)
    {
        return !first.Equals(second);
    }
}