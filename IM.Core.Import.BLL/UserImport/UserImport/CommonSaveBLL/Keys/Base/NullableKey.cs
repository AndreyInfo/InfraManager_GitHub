namespace IM.Core.Import.BLL.Import.Array;

internal record NullableKey<T>(T? Key):IIsSet where T:struct
{
    public bool IsSet()
    {
        return Key != null;
    }

    public string? GetKey()
    {
        return Key?.ToString();
    }
}