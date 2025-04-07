namespace IM.Core.Import.BLL.Import.Array;

internal sealed record LoginKey(string? Key) :NamedKey(Key, "Логин")
{
    public override string ToString()
    {
        return base.ToString();
    }
}