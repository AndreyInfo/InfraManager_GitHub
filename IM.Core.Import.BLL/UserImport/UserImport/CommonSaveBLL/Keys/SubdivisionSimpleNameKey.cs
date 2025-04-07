namespace IM.Core.Import.BLL.Import.Array;

internal sealed record SubdivisionSimpleNameKey(string? Key) :NamedKey(Key, "Название подразделения")
{
    public override string ToString()
    {
        return base.ToString();
    }
}