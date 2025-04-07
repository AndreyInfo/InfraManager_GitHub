namespace IM.Core.Import.BLL.Import.Array;

internal sealed record SIDKey(string? Key) :NamedKey(Key, "SID")
{
    public override string ToString()
    {
        return base.ToString();
    }
}