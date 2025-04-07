namespace IM.Core.Import.BLL.Import.Array;

internal sealed record ExternalIDKey(string? Key) :StringKey(Key)
{
    public override string ToString()
    {
        return $"Внешний ID: {LogHelper.ToOutputFormat(Key)}";
    }
}