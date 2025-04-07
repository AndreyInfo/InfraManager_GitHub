namespace IM.Core.Import.BLL.Import.Array;

internal sealed record EmailKey(string? Key) :NamedKey(Key, "Email")
{
    public override string ToString()
    {
        return base.ToString();
    }
}