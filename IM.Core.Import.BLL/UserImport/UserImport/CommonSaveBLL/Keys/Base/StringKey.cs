namespace IM.Core.Import.BLL.Import.Array;

internal record StringKey(string? Key):IIsSet
{
    public static bool IsSet(string data)
    {
        return !string.IsNullOrWhiteSpace(data);
    }

    public static string GetKey(string data) => data;

    public bool IsSet()
    {
        return IsSet(Key);
    }

    public string GetKey()
    {
        return Key.Trim();
    }

    public override string? ToString()
    {
        return Key;
    }
}