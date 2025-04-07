namespace IM.Core.Import.BLL.Import.Array;

internal sealed record TabNumKey(string? Key) :NamedKey(Key, "Табельный номер")
{
    public override string ToString()
    {
        return base.ToString();
    }
}